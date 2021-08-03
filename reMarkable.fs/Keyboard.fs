namespace reMarkable.fs.Keyboard

/// Defines the possible event codes an attached keyboard can raise through the KEY event
type KeyboardKey =
    /// "KEY_ESC" in input-event-codes.h
    | Esc = 1

    /// "KEY_1" in input-event-codes.h
    | NumberRow1 = 2

    /// "KEY_2" in input-event-codes.h
    | NumberRow2 = 3

    /// "KEY_3" in input-event-codes.h
    | NumberRow3 = 4

    /// "KEY_4" in input-event-codes.h
    | NumberRow4 = 5

    /// "KEY_5" in input-event-codes.h
    | NumberRow5 = 6

    /// "KEY_6" in input-event-codes.h
    | NumberRow6 = 7

    /// "KEY_7" in input-event-codes.h
    | NumberRow7 = 8

    /// "KEY_8" in input-event-codes.h
    | NumberRow8 = 9

    /// "KEY_9" in input-event-codes.h
    | NumberRow9 = 10

    /// "KEY_0" in input-event-codes.h
    | NumberRow0 = 11

    /// "KEY_MINUS" in input-event-codes.h
    | Minus = 12

    /// "KEY_EQUAL" in input-event-codes.h
    | Equal = 13

    /// "KEY_BACKSPACE" in input-event-codes.h
    | Backspace = 14

    /// "KEY_TAB" in input-event-codes.h
    | Tab = 15

    /// "KEY_Q" in input-event-codes.h
    | Q = 16

    /// "KEY_W" in input-event-codes.h
    | W = 17

    /// "KEY_E" in input-event-codes.h
    | E = 18

    /// "KEY_R" in input-event-codes.h
    | R = 19

    /// "KEY_T" in input-event-codes.h
    | T = 20

    /// "KEY_Y" in input-event-codes.h
    | Y = 21

    /// "KEY_U" in input-event-codes.h
    | U = 22

    /// "KEY_I" in input-event-codes.h
    | I = 23

    /// "KEY_O" in input-event-codes.h
    | O = 24

    /// "KEY_P" in input-event-codes.h
    | P = 25

    /// "KEY_LEFTBRACE" in input-event-codes.h
    | LeftBrace = 26

    /// "KEY_RIGHTBRACE" in input-event-codes.h
    | RightBrace = 27

    /// "KEY_ENTER" in input-event-codes.h
    | Enter = 28

    /// "KEY_LEFTCTRL" in input-event-codes.h
    | LeftCtrl = 29

    /// "KEY_A" in input-event-codes.h
    | A = 30

    /// "KEY_S" in input-event-codes.h
    | S = 31

    /// "KEY_D" in input-event-codes.h
    | D = 32

    /// "KEY_F" in input-event-codes.h
    | F = 33

    /// "KEY_G" in input-event-codes.h
    | G = 34

    /// "KEY_H" in input-event-codes.h
    | H = 35

    /// "KEY_J" in input-event-codes.h
    | J = 36

    /// "KEY_K" in input-event-codes.h
    | K = 37

    /// "KEY_L" in input-event-codes.h
    | L = 38

    /// "KEY_SEMICOLON" in input-event-codes.h
    | Semicolon = 39

    /// "KEY_APOSTROPHE" in input-event-codes.h
    | Apostrophe = 40

    /// "KEY_GRAVE" in input-event-codes.h
    | Grave = 41

    /// "KEY_LEFTSHIFT" in input-event-codes.h
    | LeftShift = 42

    /// "KEY_BACKSLASH" in input-event-codes.h
    | Backslash = 43

    /// "KEY_Z" in input-event-codes.h
    | Z = 44

    /// "KEY_X" in input-event-codes.h
    | X = 45

    /// "KEY_C" in input-event-codes.h
    | C = 46

    /// "KEY_V" in input-event-codes.h
    | V = 47

    /// "KEY_B" in input-event-codes.h
    | B = 48

    /// "KEY_N" in input-event-codes.h
    | N = 49

    /// "KEY_M" in input-event-codes.h
    | M = 50

    /// "KEY_COMMA" in input-event-codes.h
    | Comma = 51

    /// "KEY_DOT" in input-event-codes.h
    | Period = 52

    /// "KEY_SLASH" in input-event-codes.h
    | Slash = 53

    /// "KEY_RIGHTSHIFT" in input-event-codes.h
    | RightShift = 54

    /// "KEY_KPASTERISK" in input-event-codes.h
    | KeypadAsterisk = 55

    /// "KEY_LEFTALT" in input-event-codes.h
    | LeftAlt = 56

    /// "KEY_SPACE" in input-event-codes.h
    | Space = 57

    /// "KEY_CAPSLOCK" in input-event-codes.h
    | CapsLock = 58

    /// "KEY_F1" in input-event-codes.h
    | F1 = 59

    /// "KEY_F2" in input-event-codes.h
    | F2 = 60

    /// "KEY_F3" in input-event-codes.h
    | F3 = 61

    /// "KEY_F4" in input-event-codes.h
    | F4 = 62

    /// "KEY_F5" in input-event-codes.h
    | F5 = 63

    /// "KEY_F6" in input-event-codes.h
    | F6 = 64

    /// "KEY_F7" in input-event-codes.h
    | F7 = 65

    /// "KEY_F8" in input-event-codes.h
    | F8 = 66

    /// "KEY_F9" in input-event-codes.h
    | F9 = 67

    /// "KEY_F10" in input-event-codes.h
    | F10 = 68

    /// "KEY_NUMLOCK" in input-event-codes.h
    | NumberLock = 69

    /// "KEY_SCROLLLOCK" in input-event-codes.h
    | ScrollLock = 70

    /// "KEY_KP7" in input-event-codes.h
    | Keypad7 = 71

    /// "KEY_KP8" in input-event-codes.h
    | Keypad8 = 72

    /// "KEY_KP9" in input-event-codes.h
    | Keypad9 = 73

    /// "KEY_KPMINUS" in input-event-codes.h
    | KeypadMinus = 74

    /// "KEY_KP4" in input-event-codes.h
    | Keypad4 = 75

    /// "KEY_KP5" in input-event-codes.h
    | Keypad5 = 76

    /// "KEY_KP6" in input-event-codes.h
    | Keypad6 = 77

    /// "KEY_KPPLUS" in input-event-codes.h
    | KeypadPlus = 78

    /// "KEY_KP1" in input-event-codes.h
    | Keypad1 = 79

    /// "KEY_KP2" in input-event-codes.h
    | Keypad2 = 80

    /// "KEY_KP3" in input-event-codes.h
    | Keypad3 = 81

    /// "KEY_KP0" in input-event-codes.h
    | Keypad0 = 82

    /// "KEY_KPDOT" in input-event-codes.h
    | KeypadDot = 83

    /// "KEY_ZENKAKUHANKAKU" in input-event-codes.h
    | ZenkakuHankaku = 85

    /// "KEY_102ND" in input-event-codes.h
    | NonUsBackslashAndPipe = 86

    /// "KEY_F11" in input-event-codes.h
    | F11 = 87

    /// "KEY_F12" in input-event-codes.h
    | F12 = 88

    /// "KEY_RO" in input-event-codes.h
    | Ro = 89

    /// "KEY_KATAKANA" in input-event-codes.h
    | Katakana = 90

    /// "KEY_HIRAGANA" in input-event-codes.h
    | Hiragana = 91

    /// "KEY_HENKAN" in input-event-codes.h
    | Henkan = 92

    /// "KEY_KATAKANAHIRAGANA" in input-event-codes.h
    | KatakanaHiragana = 93

    /// "KEY_MUHENKAN" in input-event-codes.h
    | Muhenkan = 94

    /// "KEY_KPJPCOMMA" in input-event-codes.h
    | KeypadJpComma = 95

    /// "KEY_KPENTER" in input-event-codes.h
    | KeypadEnter = 96

    /// "KEY_RIGHTCTRL" in input-event-codes.h
    | RightCtrl = 97

    /// "KEY_KPSLASH" in input-event-codes.h
    | KeypadSlash = 98

    /// "KEY_SYSRQ" in input-event-codes.h
    | SysRq = 99

    /// "KEY_RIGHTALT" in input-event-codes.h
    | RightAlt = 100

    /// "KEY_HOME" in input-event-codes.h
    | Home = 102

    /// "KEY_UP" in input-event-codes.h
    | Up = 103

    /// "KEY_PAGEUP" in input-event-codes.h
    | PageUp = 104

    /// "KEY_LEFT" in input-event-codes.h
    | Left = 105

    /// "KEY_RIGHT" in input-event-codes.h
    | Right = 106

    /// "KEY_END" in input-event-codes.h
    | End = 107

    /// "KEY_DOWN" in input-event-codes.h
    | Down = 108

    /// "KEY_PAGEDOWN" in input-event-codes.h
    | PageDown = 109

    /// "KEY_INSERT" in input-event-codes.h
    | Insert = 110

    /// "KEY_DELETE" in input-event-codes.h
    | Delete = 111

    /// "KEY_MUTE" in input-event-codes.h
    | Mute = 113

    /// "KEY_VOLUMEDOWN" in input-event-codes.h
    | VolumeDown = 114

    /// "KEY_VOLUMEUP" in input-event-codes.h
    | VolumeUp = 115

    /// "KEY_POWER" in input-event-codes.h
    | Power = 116

    /// "KEY_KPEQUAL" in input-event-codes.h
    | KeypadEqual = 117

    /// "KEY_PAUSE" in input-event-codes.h
    | Pause = 119

    /// "KEY_KPCOMMA" in input-event-codes.h
    | KeypadComma = 121

    /// "KEY_HANGUEL" in input-event-codes.h
    | Hanguel = 122

    /// "KEY_HANJA" in input-event-codes.h
    | Hanja = 123

    /// "KEY_YEN" in input-event-codes.h
    | Yen = 124

    /// "KEY_LEFTMETA" in input-event-codes.h
    | LeftMeta = 125

    /// "KEY_RIGHTMETA" in input-event-codes.h
    | RightMeta = 126

    /// "KEY_COMPOSE" in input-event-codes.h
    | Compose = 127

    /// "KEY_STOP" in input-event-codes.h
    | Stop = 128

    /// "KEY_AGAIN" in input-event-codes.h
    | Again = 129

    /// "KEY_PROPS" in input-event-codes.h
    | Props = 130

    /// "KEY_UNDO" in input-event-codes.h
    | Undo = 131

    /// "KEY_FRONT" in input-event-codes.h
    | Front = 132

    /// "KEY_COPY" in input-event-codes.h
    | Copy = 133

    /// "KEY_OPEN" in input-event-codes.h
    | Open = 134

    /// "KEY_PASTE" in input-event-codes.h
    | Paste = 135

    /// "KEY_FIND" in input-event-codes.h
    | Find = 136

    /// "KEY_CUT" in input-event-codes.h
    | Cut = 137

    /// "KEY_HELP" in input-event-codes.h
    | Help = 138

    /// "KEY_CALC" in input-event-codes.h
    | Calc = 140

    /// "KEY_SLEEP" in input-event-codes.h
    | Sleep = 142

    /// "KEY_WWW" in input-event-codes.h
    | Www = 150

    /// "KEY_SCREENLOCK" in input-event-codes.h
    | ScreenLock = 152

    /// "KEY_BACK" in input-event-codes.h
    | Back = 158

    /// "KEY_FORWARD" in input-event-codes.h
    | Forward = 159

    /// "KEY_EJECTCD" in input-event-codes.h
    | EjectCd = 161

    /// "KEY_NEXTSONG" in input-event-codes.h
    | NextSong = 163

    /// "KEY_PLAYPAUSE" in input-event-codes.h
    | PlayPause = 164

    /// "KEY_PREVIOUSSONG" in input-event-codes.h
    | PreviousSong = 165

    /// "KEY_STOPCD" in input-event-codes.h
    | StopCd = 166

    /// "KEY_REFRESH" in input-event-codes.h
    | Refresh = 173

    /// "KEY_EDIT" in input-event-codes.h
    | Edit = 176

    /// "KEY_SCROLLUP" in input-event-codes.h
    | ScrollUp = 177

    /// "KEY_SCROLLDOWN" in input-event-codes.h
    | ScrollDown = 178

    /// "KEY_KPLEFTPAREN" in input-event-codes.h
    | KeypadLeftParen = 179

    /// "KEY_KPRIGHTPAREN" in input-event-codes.h
    | KeypadRightParen = 180

    /// "KEY_F13" in input-event-codes.h
    | F13 = 183

    /// "KEY_F14" in input-event-codes.h
    | F14 = 184

    /// "KEY_F15" in input-event-codes.h
    | F15 = 185

    /// "KEY_F16" in input-event-codes.h
    | F16 = 186

    /// "KEY_F17" in input-event-codes.h
    | F17 = 187

    /// "KEY_F18" in input-event-codes.h
    | F18 = 188

    /// "KEY_F19" in input-event-codes.h
    | F19 = 189

    /// "KEY_F20" in input-event-codes.h
    | F20 = 190

    /// "KEY_F21" in input-event-codes.h
    | F21 = 191

    /// "KEY_F22" in input-event-codes.h
    | F22 = 192

    /// "KEY_F23" in input-event-codes.h
    | F23 = 193

    /// "KEY_F24" in input-event-codes.h
    | F24 = 194

    /// "KEY_UNKNOWN" in input-event-codes.h
    | Unknown = 240
    
/// Defines the possible event codes an attached keyboard can consume through the LED event
type KeyboardLed =
    /// The number lock LE
    | NumLock = 0

    /// The caps lock LED
    | CapsLock = 1

    /// The scroll lock LEDs
    | ScrollLock = 2

/// Defines the possible event types an attached keyboard can raise
type KeyboardEventType =
    | Syn = 0
    | Key = 1
    | Msc = 2
    | Led = 17

/// Contains data related to generic key events raised by keyboard
type KeyEventArgs(key: KeyboardKey) =
    /// he key that raised the event
    member _.Key = key
    
/// Contains data related to repeatable key events raised by keyboards
/// <param name="key">The key that raised the event</param>
/// <param name="repeat">Whether or not the raised event was a repeat event</param>
type KeyPressEventArgs(key: KeyboardKey, repeat: bool) =
    inherit KeyEventArgs(key)

    /// Whether or not the raised event was a software repeat or a hardware state change
    member _.Repeat = repeat
 
// /// Provides methods for monitoring a physical keyboard attached to the device
// public sealed class HardwareKeyboardDriver : UnixInputDriver, IKeyboardDriver
//     /// <inheritdoc />
//     public event EventHandler<KeyPressEventArgs> Pressed;
//
//     /// <inheritdoc />
//     public event EventHandler<KeyEventArgs> Released;
//
//     /// <inheritdoc />
//     public Dictionary<KeyboardKey, ButtonState> KeyStates { get; }
//
//     /// Creates a new <see cref="HardwareKeyboardDriver" />
//     /// <param name="device">The device event stream to poll for new events</param>
//     public HardwareKeyboardDriver(string device) : base(device)
//     {
//         KeyStates = new Dictionary<KeyboardKey, ButtonState>();
//     }
//
//     /// <inheritdoc />
//     protected override void DataAvailable(object sender, DataAvailableEventArgs<EvEvent> e)
//     {
//         var data = e.Data;
//
//         var eventType = (KeyboardEventType)data.Type;
//
//         switch (eventType)
//         {
//             case KeyboardEventType.Syn:
//             case KeyboardEventType.Msc:
//             case KeyboardEventType.Led:
//                 break;
//             case KeyboardEventType.Key:
//             {
//                 var key = (KeyboardKey)data.Code;
//                 var state = (ButtonState)data.Value;
//
//                 switch (state)
//                 {
//                     case ButtonState.Released:
//                         Released?.Invoke(this, new KeyEventArgs(key));
//                         break;
//                     case ButtonState.Pressed:
//                     case ButtonState.Repeat:
//                         Pressed?.Invoke(this, new KeyPressEventArgs(key, state == ButtonState.Repeat));
//                         break;
//                     default:
//                         throw new ArgumentOutOfRangeException(nameof(state), state, state.GetType().Name);
//                 }
//
//                 break;
//             }
//             default:
//                 throw new ArgumentOutOfRangeException(nameof(eventType), eventType, eventType.GetType().Name);
//         }
//     }

/// Provides an interface through which a physical keyboard can be accessed
type IKeyboardDriver = interface end
    // /// Fired when a resting key is pressed
    // event EventHandler<KeyPressEventArgs> Pressed;
    //
    // /// Fired when a pressed key is released
    // event EventHandler<KeyEventArgs> Released;
    //
    // /// Contains a map of all instantaneous key states
    // Dictionary<KeyboardKey, ButtonState> KeyStates { get; }
