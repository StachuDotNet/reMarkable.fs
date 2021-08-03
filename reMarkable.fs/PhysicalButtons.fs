namespace reMarkable.fs.PhysicalButtons

open System
open System.Collections.Generic
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

/// Defines the individual buttons present on the device
type PhysicalButton =
    /// The home button
    | Home = 102us

    /// The left button
    | Left = 105us

    /// The right button
    | Right = 106us

    /// The power button
    | Power = 116us

    /// Not mapped to a physical button
    /// <remarks>
    /// It's possible that this key code is used in place of an actual key code when a device is awoken from "light sleep"
    /// </remarks>
    | WakeUp = 143us

/// Defines the possible event types the buttons can raise
type PhysicalButtonEventType =
    | Syn = 0us
    | Key = 1us

/// Defines the possible instantaneous states a button can have
type ButtonState =
    /// The button is not pressed
    | Released = 0

    /// The button is pressed
    | Pressed = 1

    /// If applicable, the button has been pressed long enough to produce repeating press events
    | Repeat = 2
    
/// Provides methods for monitoring the physical buttons installed in the device
/// <param name="devicePath">The device event stream to poll for new events</param>
type PhysicalButtonDriver(devicePath: string) =
    inherit UnixInputDriver(devicePath)
    
    let buttonStates = Dictionary<PhysicalButton, ButtonState>()
    let pressed, released = Event<PhysicalButton>(), Event<PhysicalButton>()
    
    
    /// Fired when a resting button is pressed
    member _.Pressed = pressed.Publish
    
    /// Fired when a resting button is released
    member _.Released = released.Publish
    
    /// Contains a map of all instantaneous button states
    member _.ButtonStates = buttonStates
    
    override _.DataAvailable(e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data

        let eventType: PhysicalButtonEventType =
            LanguagePrimitives.EnumOfValue data.Type

        match eventType with
        | PhysicalButtonEventType.Syn -> ()
        | PhysicalButtonEventType.Key ->
            let button: PhysicalButton = LanguagePrimitives.EnumOfValue data.Code
            let buttonState: ButtonState = LanguagePrimitives.EnumOfValue data.Value

            buttonStates.[button] <- ButtonState.Pressed

            match (buttonState) with
            | ButtonState.Released ->
                released.Trigger button
                
            | ButtonState.Pressed ->
                pressed.Trigger button
            | _ -> raise <| ArgumentOutOfRangeException(nameof(buttonState), buttonState, buttonState.GetType().Name)
        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name)
