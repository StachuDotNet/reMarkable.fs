module reMarkable.fs.Digitizer

open System
open System.Collections.Generic
open SixLabors.ImageSharp
open reMarkable.fs.PhysicalButtons
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

/// Tools able to be represented by the device
type StylusTool =
    /// The pen tool
    | Pen

    /// The eraser tool - (only some styluses have this)
    | Eraser

/// Possible event types the digitizer can raise
type DigitizerEventType =
    /// Used to separate events by time
    | Syn = 0us
    
    /// Used to describe state changes of keyboards, buttons, or other key-like devices
    | Key = 1us
    
    /// Used to describe absolute axis value changes, e.g. describing the coordinates of a touch on a touchscreen
    | Abs = 3us

/// Defines the possible event codes the digitizer can raise through the KEY event
type DigitizerEventKeyCode =
    /// Reports a transition to the stylus nib tool
    | ToolPen = 320us

    /// Reports a transition to the stylus "eraser" tool
    | ToolRubber = 321us

    /// Reports that the stylus has pressed the screen
    | Touch = 330us

type DigitizerEventAbsCode =
    /// Reports the X position of the stylus
    | AbsX = 0us

    /// Reports the Y position of the stylus
    | AbsY = 1us

    /// Reports the pressure of the stylus
    | AbsPressure = 24us

    /// Reports the distance from the stylus to the digitizer
    | AbsDistance = 25us

    /// Reports the tilt of the stylus in the X direction
    | AbsTiltX = 26us

    /// Reports the tilt of the stylus in the Y direction
    | AbsTiltY = 27us

type TouchscreenCalibration =
    { Kx1: float32; Kx2: float32; Kx3: float32
      Ky1: float32; Ky2: float32; Ky3: float32 }
module TouchscreenCalibration =
    let Default =
        { Kx1 = 1f
          Kx2 = 0f
          Kx3 = -0.5f
          Ky1 = 0f
          Ky2 = 1f
          Ky3 = -0.5f }
    
    let ReMarkableMarker =
      { Kx1 = 0.0002023094f
        Kx2 = 0.08924796f
        Kx3 = -0.86863464f
        Ky1 = -0.08942877f
        Ky2 = -0.00000043777243f
        Ky3 = 1870.3854f }

/// Represents a stylus' complete immediate state
type StylusState(tool: StylusTool option, position: Point, pressure: int, distance: int, tilt: Point) =
    /// The proximity of the stylus tool to the digitizer.
    /// Ranges between:
    /// - 0 (touching screen)
    /// - 255 (furthest detectable distance, around 5mm)
    member _.Distance = distance

    /// The position of the stylus tool 
    member _.Position = position

    /// The pressure of the stylus tool 
    member _.Pressure = pressure

    /// The tilt of the stylus tool
    /// Ranges from -9000 to 9000
    member _.Tilt = tilt

    /// The tool currently employed by the stylus 
    member _.Tool = tool
    
    member this.NormalizedPosition =
        let calibration = TouchscreenCalibration.ReMarkableMarker
        let x =
            calibration.Kx1 * (float32 this.Position.X) +
            calibration.Kx2 * (float32 this.Position.Y) +
            calibration.Kx3 +
            0.5f
            
        let y =
            calibration.Ky1 * (float32 this.Position.X) +
            calibration.Ky2 * (float32 this.Position.Y) +
            calibration.Ky3 +
            0.5f
        
        // ... but then we need this for some reason :) (if portrait)
        let x, y = x, 1870f - y
        
        PointF(x, y)

    override _.ToString() =
        $"{nameof(tool)}: {tool}, {nameof(position)}: {position}, {nameof(pressure)}: {pressure}, {nameof(distance)}: {distance}, {nameof(tilt)}: {tilt}"


/// Provides an interface through which to access the device's digitizer
type IDigitizerDriver =
    /// Fired when the stylus makes physical contact with the device
    abstract member Pressed: IEvent<DigitizerEventKeyCode>

    /// Fired when the stylus breaks physical contact with the device
    abstract member Released: IEvent<DigitizerEventKeyCode>

    /// Fired when the stylus changes state
    abstract member StylusUpdate: IEvent<StylusState>

    /// Fired when the stylus tool changes 
    abstract member ToolChanged: IEvent<StylusTool option>

    /// The instantaneous state of the stylus
    abstract member State: StylusState option
    
    ///  The instantaneous states of the stylus tools and buttons 
    abstract member ButtonStates: Dictionary<DigitizerEventKeyCode, ButtonState>

    /// The vertical resolution of the device 
    abstract member Height: int

    /// The horizontal resolution of the device 
    abstract member Width: int


/// Provides methods for monitoring the digitizer installed in the device
/// 
/// <param name="devicePath">The device event stream to poll for new events</param>
type HardwareDigitizerDriver(devicePath: string, width: int, height: int) =
    inherit UnixInputDriver(devicePath)
    
    let buttonStates = Dictionary<DigitizerEventKeyCode, ButtonState>()
    
    let pressed, released = Event<DigitizerEventKeyCode>(), Event<DigitizerEventKeyCode>()
    let stylusUpdate = Event<StylusState>()
    let toolChanged = Event<StylusTool option>()
    
    /// distance value accumulated for event dispatch
    let mutable currentDistance: int = 0

    /// position value accumulated for event dispatch
    let mutable currentPosition = Point.Empty

    /// pressure value accumulated for event dispatch
    let mutable currentPressure: int = 0

    /// tilt value accumulated for event dispatch
    let mutable currentTilt: Point = Point.Empty

    /// What tool are we currently working with?
    let mutable currentTool: StylusTool option = None

    let mutable stylusState: StylusState option = None
    
    interface IDigitizerDriver with
        member _.Pressed = pressed.Publish
        member _.Released = released.Publish
        member _.StylusUpdate = stylusUpdate.Publish
        member _.State = stylusState
        member _.Height = height
        member _.Width = width
        member _.ButtonStates = buttonStates
        member _.ToolChanged = toolChanged.Publish

    override this.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data
        
        let eventType: DigitizerEventType = data.Type |> LanguagePrimitives.EnumOfValue 
            
        // printfn "Event data: (Code: " e.Data.Type

        match eventType with
        | DigitizerEventType.Syn -> // "Separator" event
            stylusState <- Some <| StylusState(currentTool, currentPosition, currentPressure, currentDistance, currentTilt)
            stylusUpdate.Trigger stylusState.Value

            if currentTool = None then
                currentDistance <- 255
                currentPressure <- 0
                currentPosition <- Point.Empty
                currentTilt <- Point.Empty
            elif currentDistance > 0 then
                currentPressure <- 0
            elif currentPressure > 0 then
                currentDistance <- 0
        
        | DigitizerEventType.Key ->
            let key: DigitizerEventKeyCode = data.Code |> LanguagePrimitives.EnumOfValue  
            let state: ButtonState = data.Value |> LanguagePrimitives.EnumOfValue 

            buttonStates.[key] <- state

            match key with
            | DigitizerEventKeyCode.ToolPen ->
                currentTool <-
                    match state with
                    | ButtonState.Pressed -> Some StylusTool.Pen
                    | _ -> None
                        
                toolChanged.Trigger currentTool
            | DigitizerEventKeyCode.ToolRubber ->
                currentTool <-
                    match state with
                    | ButtonState.Pressed -> Some StylusTool.Eraser
                    | _ ->  None
                toolChanged.Trigger currentTool
            | DigitizerEventKeyCode.Touch -> ()
            | _ -> raise <| ArgumentOutOfRangeException(nameof key, key, key.GetType().Name)
        
        | DigitizerEventType.Abs ->
            let eventCode: DigitizerEventAbsCode = data.Code |> LanguagePrimitives.EnumOfValue 

            // x = 
            
            match eventCode with
            | DigitizerEventAbsCode.AbsX ->        currentPosition.X <- data.Value
            | DigitizerEventAbsCode.AbsY ->        currentPosition.Y <- data.Value
            | DigitizerEventAbsCode.AbsPressure -> currentPressure <- data.Value
            | DigitizerEventAbsCode.AbsDistance -> currentDistance <- data.Value
            | DigitizerEventAbsCode.AbsTiltX ->    currentTilt.X <- data.Value
            | DigitizerEventAbsCode.AbsTiltY ->    currentTilt.Y <- data.Value
            | _ -> raise <| ArgumentOutOfRangeException(nameof(eventCode), eventCode, eventCode.GetType().Name)
        
        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name)


// todo: create abstraction over the tilts & pressure to capture general _classes_ of such.
// e.g. "pressure: not touching, light, medium, heavy"
// e.g. "tiltX: hard left, slightly left, none, slightly right, right"
// note: within this module, we can just include the mappings of value->generalization, and allow consumer to utilize if interested