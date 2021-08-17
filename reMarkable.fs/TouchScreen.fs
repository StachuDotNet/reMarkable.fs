namespace reMarkable.fs.Touchscreen

open System
open SixLabors.ImageSharp
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

// The reMarkable devices feature multi-touch technology, tracking the movements of multiple fingers.
// ...
// Advice: read through https://www.kernel.org/doc/Documentation/input/multi-touch-protocol.txt

/// Possible states in which the touchscreen can represent a finger
type FingerStatus =
    /// The finger is not in contact with the touchscreen
    | Untracked

    /// The finger has just been removed from the touchscreen
    | Up

    /// The finger has just come into contact with the touchscreen
    | Down

    /// The finger is translating across the touchscreen
    | Moving

/// Possible event codes the touchscreen can raise through the ABS event
type TouchscreenEventAbsCode =
    /// Reports the distance of a finger from the touchscreen
    | Distance = 25

    /// Reports the slot ID of a finger
    | MultiTouchSlot = 47

    /// Reports the major axis of a multi-touch pair
    | MultiTouchTouchMajor = 48

    /// Reports the minor axis of a multi-touch pair
    | MultiTouchTouchMinor = 49
    
    /// Reports the orientation of a multi-touch pair
    | MultiTouchOrientation = 52

    /// Reports the X position of a finger
    | MultiTouchPositionX = 53

    /// Reports the Y position of a finger
    | MultiTouchPositionY = 54

    /// Reports the tool ID of a finger
    | MultiTouchToolType = 55

    /// Reports the tracking ID of the finger event
    | MultiTouchTrackingId = 57

    /// Reports the pressure of the finger
    | MultiTouchPressure = 58
    
/// Possible event types the touchscreen can raise
type TouchscreenEventType =
    | Syn = 0
    | Key = 1
    | Relative = 2
    | Absolute = 3


/// A finger's complete immediate state
type FingerState =
  struct
    /// Current display position
    val mutable DevicePosition: Point

    /// Current device position
    val mutable RawPosition: Point

    /// Pressure, from 0-255
    val mutable Pressure: int

    /// Previous display position
    val mutable PreviousDevicePosition: Point

    /// Previous device position
    val mutable PreviousRawPosition: Point

    /// Previous pressure, from 0-255
    val mutable PreviousPressure: int

    /// Current status
    val mutable Status: FingerStatus

    /// Tracking ID (used for tracking multi-touch)
    val mutable Id: int
  end


/// Provides a set of methods for monitoring the device's physical touchscreen
type HardwareTouchscreenDriver(devicePath: string, width: int, height: int, maxFingers: int, shouldInvertWidth: bool) =
    inherit UnixInputDriver(devicePath)

    /// Temporary finger position accumulated for event dispatch
    let mutable position: Point = Point.Empty
    let fingers = Array.zeroCreate<FingerState> maxFingers
    let moved, pressed, released = Event<FingerState>(), Event<FingerState>(), Event<FingerState>()
    
    /// Temporary finger slot index accumulated for event dispatch
    let mutable slot: int = 0
    
    
    member _.Height = height
    member _.MaxFingers = maxFingers
    member _.Width = width
    
    /// The status of each finger arranged according to their slot index
    member _.Fingers = fingers
    
    /// Fired when a finger which is in contact with the screen moves
    member _.Moved = moved.Publish
    
    /// Fired when a finger contacts the screen
    member _.Pressed = pressed.Publish
    
    /// Fired when a finger is removed from the screen
    member _.Released = released.Publish

    member private this.ProcessAbsoluteTouch(code: TouchscreenEventAbsCode, value: int) =
        match code with
        | TouchscreenEventAbsCode.Distance -> ()
        | TouchscreenEventAbsCode.MultiTouchSlot ->
            slot <- value
            
            if slot >= fingers.Length then
                slot <- fingers.Length - 1 //sink
            
        | TouchscreenEventAbsCode.MultiTouchTouchMajor -> ()
        | TouchscreenEventAbsCode.MultiTouchTouchMinor -> ()
        | TouchscreenEventAbsCode.MultiTouchOrientation -> ()
        | TouchscreenEventAbsCode.MultiTouchPositionX -> 
            fingers.[slot].PreviousDevicePosition.X <- fingers.[slot].DevicePosition.X
            fingers.[slot].PreviousRawPosition.X <- fingers.[slot].RawPosition.X

            let pos = // why float before?
                if shouldInvertWidth then
                    this.Width - 1 - value
                else
                    value

            let width = 1404; // todo: OutputDevices.Display.VisibleWidth
            position.X <- (int)(pos / this.Width * width);

            fingers.[slot].DevicePosition.X <- position.X
            fingers.[slot].RawPosition.X <- value
        
        | TouchscreenEventAbsCode.MultiTouchPositionY ->
            fingers.[slot].PreviousDevicePosition.Y <- fingers.[slot].DevicePosition.Y;
            fingers.[slot].PreviousRawPosition.Y <- fingers.[slot].RawPosition.Y;

            let pos = // this was float?
                this.Height - 1 - value
            
            let height = 1872 // todo: OutputDevices.Display.VisibleHeight
            position.Y <- (int)(pos / this.Height * height)

            fingers.[slot].DevicePosition.Y <- position.Y
            fingers.[slot].RawPosition.Y <- value
        
        | TouchscreenEventAbsCode.MultiTouchToolType -> ()
        | TouchscreenEventAbsCode.MultiTouchTrackingId ->
            printfn "slot: %A" slot
            printfn "fingers: %A" fingers
            if value = -1 then
                fingers.[slot].Status <- FingerStatus.Up
            elif (fingers.[slot].Status = FingerStatus.Untracked) then
                fingers.[slot].Id <- value
                fingers.[slot].Status <- FingerStatus.Down
        
        | TouchscreenEventAbsCode.MultiTouchPressure ->
            fingers.[slot].PreviousPressure <- fingers.[slot].Pressure
            fingers.[slot].Pressure <- value
            
        | _ -> raise <| ArgumentOutOfRangeException(nameof(code), code, code.GetType().Name);

    override this.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data

        let eventType: TouchscreenEventType = LanguagePrimitives.EnumOfValue <| int data.Type // is this int safe?

        match eventType with
        | TouchscreenEventType.Syn ->
            match fingers.[slot].Status with
            | FingerStatus.Down -> pressed.Trigger fingers.[slot]
            | FingerStatus.Up -> released.Trigger fingers.[slot]
            | FingerStatus.Moving -> moved.Trigger fingers.[slot]
            | FingerStatus.Untracked -> ()

            fingers.[slot].Status <-
                match fingers.[slot].Status with
                | FingerStatus.Up -> FingerStatus.Untracked
                | FingerStatus.Down -> FingerStatus.Moving
                | _ -> fingers.[slot].Status

            slot <- 0

        | TouchscreenEventType.Absolute ->
            let absCode: TouchscreenEventAbsCode = LanguagePrimitives.EnumOfValue (int data.Code)
            this.ProcessAbsoluteTouch(absCode, data.Value);
                
        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
