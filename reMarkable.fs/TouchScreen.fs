// The reMarkable devices feature multi-touch technology, tracking the movements of multiple fingers.
// ...
// Advice: read through https://www.kernel.org/doc/Documentation/input/multi-touch-protocol.txt
module reMarkable.fs.Touchscreen

open System
open SixLabors.ImageSharp
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

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
    /// Reports the tracking ID of the finger event
    /// This is fired 'first' within a group of events; the events that follow correspond to that tracked finger
    /// 
    /// If the event's "value" corresponding to this event is a positive number, this denotes a tracked finger
    /// If the corresponding "value" is -1, that denotes the _removal_ of the finger in focus  
    | TrackingId = 57
    
    /// Reports the slot ID of a finger
    | Slot = 47

    /// Reports the major axis of a multi-touch pair
    | MajorAxis = 48

    /// Reports the minor axis of a multi-touch pair
    | MinorAxis = 49
    
    /// Reports the orientation of a multi-touch pair
    | Orientation = 52

    /// Reports the X position of a finger
    | PositionX = 53

    /// Reports the Y position of a finger
    | PositionY = 54

    /// Reports the pressure of the finger
    | Pressure = 58
    
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
type HardwareTouchscreenDriver(devicePath: string, width: int, height: int,shouldInvertWidth: bool) =
    inherit UnixInputDriver(devicePath)

    /// Temporary finger position accumulated for event dispatch
    let mutable position: Point = Point.Empty
    let fingers = Array.zeroCreate<FingerState> 32
    let moved, pressed, released = Event<FingerState>(), Event<FingerState>(), Event<FingerState>()
    
    /// Temporary finger slot index accumulated for event dispatch
    let mutable currentSlotOfFocus: int = 0
    
    member _.Height = height
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
        | TouchscreenEventAbsCode.Slot ->
            currentSlotOfFocus <- value
            
            if currentSlotOfFocus >= fingers.Length then
                currentSlotOfFocus <- fingers.Length - 1 //sink
            
        | TouchscreenEventAbsCode.MajorAxis -> ()
        | TouchscreenEventAbsCode.MinorAxis -> ()
        | TouchscreenEventAbsCode.Orientation -> ()
        | TouchscreenEventAbsCode.PositionX -> 
            fingers.[currentSlotOfFocus].PreviousDevicePosition.X <- fingers.[currentSlotOfFocus].DevicePosition.X
            fingers.[currentSlotOfFocus].PreviousRawPosition.X <- fingers.[currentSlotOfFocus].RawPosition.X

            let pos = // why float before?
                if shouldInvertWidth then
                    this.Width - 1 - value
                else
                    value

            let width = 1404; // todo: OutputDevices.Display.VisibleWidth
            position.X <- (int)(pos / this.Width * width);

            fingers.[currentSlotOfFocus].DevicePosition.X <- position.X
            fingers.[currentSlotOfFocus].RawPosition.X <- value
        
        | TouchscreenEventAbsCode.PositionY ->
            fingers.[currentSlotOfFocus].PreviousDevicePosition.Y <- fingers.[currentSlotOfFocus].DevicePosition.Y;
            fingers.[currentSlotOfFocus].PreviousRawPosition.Y <- fingers.[currentSlotOfFocus].RawPosition.Y;

            let pos = // this was float?
                this.Height - 1 - value
            
            let height = 1872 // todo: OutputDevices.Display.VisibleHeight
            position.Y <- (int)(pos / this.Height * height)

            fingers.[currentSlotOfFocus].DevicePosition.Y <- position.Y
            fingers.[currentSlotOfFocus].RawPosition.Y <- value
        
        | TouchscreenEventAbsCode.TrackingId ->
            if value = -1 then
                fingers.[currentSlotOfFocus].Status <- FingerStatus.Up
            elif (fingers.[currentSlotOfFocus].Status = FingerStatus.Untracked) then
                fingers.[currentSlotOfFocus].Id <- value
                fingers.[currentSlotOfFocus].Status <- FingerStatus.Down
        
        | TouchscreenEventAbsCode.Pressure ->
            fingers.[currentSlotOfFocus].PreviousPressure <- fingers.[currentSlotOfFocus].Pressure
            fingers.[currentSlotOfFocus].Pressure <- value
            
        | _ -> raise <| ArgumentOutOfRangeException(nameof(code), code, code.GetType().Name);

    override this.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data

        let eventType: TouchscreenEventType = LanguagePrimitives.EnumOfValue <| int data.Type

        match eventType with
        | TouchscreenEventType.Syn ->
            match fingers.[currentSlotOfFocus].Status with
            | FingerStatus.Down -> pressed.Trigger fingers.[currentSlotOfFocus]
            | FingerStatus.Up -> released.Trigger fingers.[currentSlotOfFocus]
            | FingerStatus.Moving -> moved.Trigger fingers.[currentSlotOfFocus]
            | FingerStatus.Untracked -> ()

            fingers.[currentSlotOfFocus].Status <-
                match fingers.[currentSlotOfFocus].Status with
                | FingerStatus.Up -> FingerStatus.Untracked
                | FingerStatus.Down -> FingerStatus.Moving
                | _ -> fingers.[currentSlotOfFocus].Status

            currentSlotOfFocus <- 0

        | TouchscreenEventType.Absolute ->
            let absCode: TouchscreenEventAbsCode = LanguagePrimitives.EnumOfValue (int data.Code)
            this.ProcessAbsoluteTouch(absCode, data.Value);
                
        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
