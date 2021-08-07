namespace reMarkable.fs.Digitizer

open System
open System.Collections.Generic
open SixLabors.ImageSharp
open reMarkable.fs.PhysicalButtons
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

/// Defines the individual tools able to be represented by the device
type StylusTool =
    /// The pen tool
    | Pen

    /// The eraser tool
    | Eraser

/// Defines the possible event types the digitizer can raise
type DigitizerEventType =
    | Syn = 0
    | Key = 1
    | Abs = 3

/// Defines the possible event codes the digitizer can raise through the KEY event
type DigitizerEventKeyCode =
    /// Reports a transition to the stylus nib tool
    | ToolPen = 320

    /// Reports a transition to the stylus "eraser" tool
    | ToolRubber = 321

    /// Reports that the stylus has pressed the screen
    | Touch = 330

    /// Reports a press of the first stylus button
    | Stylus = 331

    /// Reports a press of the second stylus button
    | Stylus2 = 332

type DigitizerEventAbsCode =
    /// Reports the X position of the stylus
    | AbsX = 0

    /// Reports the Y position of the stylus
    | AbsY = 1

    /// Reports the pressure of the stylus
    | AbsPressure = 24

    /// Reports the distance from the stylus to the digitizer
    | AbsDistance = 25

    /// Reports the tilt of the stylus in the X direction
    | AbsTiltX = 26

    /// Reports the tilt of the stylus in the Y direction
    | AbsTiltY = 27

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

    /// The tilt of the stylus tool from -9000 (horizontal one direction) to 9000 (horizontal the opposing direction) 
    member _.Tilt = tilt

    /// The tool currently employed by the stylus 
    member _.Tool = tool

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

    /// The instantaneous state of the stylus
    abstract member State: StylusState option

    // ///  Fired when the stylus tool changes 
    // event EventHandler<StylusTool> ToolChanged
    //
    // ///  The instantaneous states of the stylus tools and buttons 
    // Dictionary<DigitizerEventKeyCode, ButtonState> ButtonStates { get }


    // /// The vertical resolution of the device 
    // int Height { get }

    // /// The horizontal resolution of the device 
    // int Width { get }


/// Provides methods for monitoring the digitizer installed in the device
/// 
/// <param name="devicePath">The device event stream to poll for new events</param>
/// <param name="width">The virtual width of the device</param>
/// <param name="height">The virtual height of the device</param>
type HardwareDigitizerDriver(devicePath: string, width: int, height: int) =
    inherit UnixInputDriver(devicePath)
    
    let buttonStates = Dictionary<DigitizerEventKeyCode, ButtonState>()
    
    let pressed, released = Event<DigitizerEventKeyCode>(), Event<DigitizerEventKeyCode>()
    let stylusUpdate = Event<StylusState>()
    let toolChanged = Event<StylusTool option>()
    
    
    /// Temporary distance value accumulated for event dispatch
    let mutable currentDistance: int = 0

    /// Temporary position value accumulated for event dispatch
    let mutable currentPosition: Point = Point.Empty

    /// Temporary pressure value accumulated for event dispatch
    let mutable currentPressure: int = 0

    /// Temporary tilt value accumulated for event dispatch
    let mutable currentTilt: Point = Point.Empty

    /// Temporary tool value accumulated for event dispatch
    let mutable currentTool: StylusTool option = None
    
    let mutable stylusState: StylusState option = None
    
    member _.Height = height

    member _.Width = width

    member _.ButtonStates = buttonStates
    
    interface IDigitizerDriver with
        /// Fired when the stylus makes physical contact with the device
        member _.Pressed = pressed.Publish

        /// Fired when the stylus breaks physical contact with the device
        member _.Released = released.Publish

        /// Fired when the stylus changes state
        member _.StylusUpdate = stylusUpdate.Publish

        /// The instantaneous state of the stylus
        member _.State = stylusState

    member _.ToolChanged = toolChanged.Publish

    override this.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data

        let eventType: DigitizerEventType =
            LanguagePrimitives.EnumOfValue (data.Type |> int)

        match eventType with
        | DigitizerEventType.Syn ->
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
            let key: DigitizerEventKeyCode = LanguagePrimitives.EnumOfValue (int data.Code)
            let state: ButtonState = LanguagePrimitives.EnumOfValue (int data.Value)

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
            | DigitizerEventKeyCode.Touch ->
                ()
                // Stylus touch input unreliable, and data is redundant
                // because of ABS_PRESSURE
            | DigitizerEventKeyCode.Stylus
            | DigitizerEventKeyCode.Stylus2 ->
                match state with
                | ButtonState.Pressed -> pressed.Trigger key
                | _ ->                   released.Trigger key
            | _ -> raise <| ArgumentOutOfRangeException(nameof(key), key, key.GetType().Name)
        
        | DigitizerEventType.Abs ->
            let eventCode: DigitizerEventAbsCode = LanguagePrimitives.EnumOfValue (int data.Code)

            match eventCode with
            | DigitizerEventAbsCode.AbsX ->        currentPosition.X <- data.Value
            | DigitizerEventAbsCode.AbsY ->        currentPosition.Y <- data.Value
            | DigitizerEventAbsCode.AbsPressure -> currentPressure <- data.Value
            | DigitizerEventAbsCode.AbsDistance -> currentDistance <- data.Value
            | DigitizerEventAbsCode.AbsTiltX ->    currentTilt.X <- data.Value
            | DigitizerEventAbsCode.AbsTiltY ->    currentTilt.Y <- data.Value
            | _ -> raise <| ArgumentOutOfRangeException(nameof(eventCode), eventCode, eventCode.GetType().Name)
        
        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name)
