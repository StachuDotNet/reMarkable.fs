namespace reMarkable.fs.Keyboard

open System
open System.Collections.Generic
open reMarkable.fs.PhysicalButtons
open reMarkable.fs.UnixInput
open reMarkable.fs.Util

/// Event codes an attached keyboard can consume through the LED event
type KeyboardLed = | NumLock = 0 | CapsLock = 1 | ScrollLock = 2

/// Event types an attached keyboard can raise
type KeyboardEventType =
    | Syn = 0us
    | Key = 1us
    | Msc = 2us
    | Led = 17us

/// Data related to generic key events raised by keyboard
type KeyEventArgs(key: KeyboardKey) =
    /// The key that raised the event
    member _.Key = key
    
/// Data related to repeatable key events raised by keyboards
/// <param name="key">The key that raised the event</param>
/// <param name="repeat">Whether or not the raised event was a repeat event</param>
type KeyPressEventArgs(key: KeyboardKey, repeat: bool) =
    inherit KeyEventArgs(key)

    /// Whether or not the raised event was a software repeat or a hardware state change
    member _.Repeat = repeat

/// Provides an interface through which a physical keyboard can be accessed
type IKeyboardDriver =
    /// Fired when a resting key is pressed
    abstract member Pressed: IEvent<KeyPressEventArgs>
    
    /// Fired when a pressed key is released
    abstract member Released: IEvent<KeyEventArgs>
    
    /// Contains a map of all instantaneous key states
    abstract member KeyStates: Dictionary<KeyboardKey, ButtonState>

 
/// Provides methods for monitoring a physical keyboard attached to the device
/// <param name="device">The device event stream to poll for new events</param>
type HardwareKeyboardDriver(device: string) =
    inherit UnixInputDriver(device)
    
    let keyStates = Dictionary<KeyboardKey, ButtonState>()
    let pressed, released = Event<KeyPressEventArgs>(), Event<KeyEventArgs>()
    
    interface IKeyboardDriver with
        member _.Pressed = pressed.Publish
        member _.Released = released.Publish
        member _.KeyStates = keyStates

    override _.DataAvailable( e: DataAvailableEventArgs<EvEvent>) =
        let data = e.Data

        let eventType: KeyboardEventType = data.Type |> LanguagePrimitives.EnumOfValue

        match eventType with
        | KeyboardEventType.Syn
        | KeyboardEventType.Msc
        | KeyboardEventType.Led ->
            ()
        | KeyboardEventType.Key ->
            let key: KeyboardKey = data.Code |> LanguagePrimitives.EnumOfValue 
            let state: ButtonState = data.Value |> LanguagePrimitives.EnumOfValue 

            match state with
            | ButtonState.Released -> released.Trigger (KeyEventArgs(key))
            | ButtonState.Pressed ->  pressed.Trigger (KeyPressEventArgs(key, false))
            | ButtonState.Repeat ->   pressed.Trigger (KeyPressEventArgs(key, true))
            | _ -> raise <| ArgumentOutOfRangeException(nameof(state), state, state.GetType().Name)

        | _ -> raise <| ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name)
