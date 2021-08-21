// The reMarkable devices feature multi-touch technology, tracking the movements of multiple fingers.
//
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
    | TrackingId = 57us
    
    /// Reports the slot ID of a finger
    /// The reMarkable supports up to 32 slots (wow!?)
    | Slot = 47us

    /// Reports the major axis of a multi-touch pair
    | MajorAxis = 48us

    /// Reports the minor axis of a multi-touch pair
    | MinorAxis = 49us
    
    /// Reports the orientation of a multi-touch pair
    | Orientation = 52us

    /// Reports the X position of a finger
    | PositionX = 53us

    /// Reports the Y position of a finger
    | PositionY = 54us

    /// Reports the pressure of the finger
    | Pressure = 58us
    
/// Possible event types the touchscreen can raise
type TouchscreenEventType =
    | Syn = 0us
    | Absolute = 3us

/// A finger's complete immediate state
[<Struct>]
type FingerState =
  { /// Tracking ID (used for tracking multi-touch)
    mutable Id: int
    
    /// Current position
    mutable Position: Point

    /// Pressure, from 0-255
    mutable Pressure: int

    /// Previous position
    mutable PreviousPosition: Point

    /// Previous pressure, from 0-255
    mutable PreviousPressure: int

    /// Current status
    mutable Status: FingerStatus }
  
  member this.UpdatePressure newPressure =
      this.PreviousPressure <- this.Pressure
      this.Pressure <- newPressure
  
  member this.UpdateXPosition newX =
      this.PreviousPosition.X <- this.Position.X
      this.Position.X <- newX
  
  member this.UpdateYPosition newY =
      this.PreviousPosition.Y <- this.Position.Y
      this.Position.Y <- newY

/// Provides a set of methods for monitoring the device's physical touchscreen
type HardwareTouchscreenDriver(devicePath: string, deviceHeight: int) =
    inherit UnixInputDriver(devicePath)

    /// Temporary finger position accumulated for event dispatch
    let fingers = Array.zeroCreate<FingerState> 32 |> Array.map(fun finger -> { finger with Status = Untracked }) 
    let moved, pressed, released = Event<FingerState>(), Event<FingerState>(), Event<FingerState>()
    
    /// Which 'finger slot' are we currently focused on?
    /// This changes over time as fingers are lifted, moved, have pressures changed, etc.
    let mutable currentSlotOfFocus: int = 0
    
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
        
        | TouchscreenEventAbsCode.PositionX ->
            fingers.[currentSlotOfFocus].UpdateXPosition value
        
        | TouchscreenEventAbsCode.PositionY ->
            let pos =  deviceHeight - 1 - value // ???
            fingers.[currentSlotOfFocus].UpdateYPosition pos
        
        | TouchscreenEventAbsCode.TrackingId ->
            if value = -1 then
                fingers.[currentSlotOfFocus].Status <- FingerStatus.Up
            elif fingers.[currentSlotOfFocus].Status = FingerStatus.Untracked then
                fingers.[currentSlotOfFocus].Id <- value
                fingers.[currentSlotOfFocus].Status <- FingerStatus.Down
        
        | TouchscreenEventAbsCode.Pressure ->
            fingers.[currentSlotOfFocus].UpdatePressure value
            
        // so far, I'm not handling these :) todo.
        | TouchscreenEventAbsCode.MajorAxis -> ()
        | TouchscreenEventAbsCode.MinorAxis -> ()
        | TouchscreenEventAbsCode.Orientation -> ()
        
        | _ -> raise <| ArgumentOutOfRangeException(nameof code, code, code.GetType().Name)

    override this.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let eventData = e.Data

        let eventType: TouchscreenEventType = eventData.Type |> LanguagePrimitives.EnumOfValue

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
            let absCode: TouchscreenEventAbsCode = eventData.Code |> LanguagePrimitives.EnumOfValue
            this.ProcessAbsoluteTouch(absCode, eventData.Value)
                
        | _ -> raise <| ArgumentOutOfRangeException(nameof eventType, eventType, eventType.GetType().Name)
