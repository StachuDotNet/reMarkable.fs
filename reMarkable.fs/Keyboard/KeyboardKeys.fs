namespace reMarkable.fs.Keyboard

/// Event codes an attached keyboard can raise through the KEY event
type KeyboardKey =
    /// "KEY_ESC" in input-event-codes.h
    | Esc = 1us

    /// "KEY_1" in input-event-codes.h
    | NumberRow1 = 2us

    /// "KEY_2" in input-event-codes.h
    | NumberRow2 = 3us

    /// "KEY_3" in input-event-codes.h
    | NumberRow3 = 4us

    /// "KEY_4" in input-event-codes.h
    | NumberRow4 = 5us

    /// "KEY_5" in input-event-codes.h
    | NumberRow5 = 6us

    /// "KEY_6" in input-event-codes.h
    | NumberRow6 = 7us

    /// "KEY_7" in input-event-codes.h
    | NumberRow7 = 8us

    /// "KEY_8" in input-event-codes.h
    | NumberRow8 = 9us

    /// "KEY_9" in input-event-codes.h
    | NumberRow9 = 10us

    /// "KEY_0" in input-event-codes.h
    | NumberRow0 = 11us

    /// "KEY_MINUS" in input-event-codes.h
    | Minus = 12us

    /// "KEY_EQUAL" in input-event-codes.h
    | Equal = 13us

    /// "KEY_BACKSPACE" in input-event-codes.h
    | Backspace = 14us

    /// "KEY_TAB" in input-event-codes.h
    | Tab = 15us

    /// "KEY_Q" in input-event-codes.h
    | Q = 16us

    /// "KEY_W" in input-event-codes.h
    | W = 17us

    /// "KEY_E" in input-event-codes.h
    | E = 18us

    /// "KEY_R" in input-event-codes.h
    | R = 19us

    /// "KEY_T" in input-event-codes.h
    | T = 20us

    /// "KEY_Y" in input-event-codes.h
    | Y = 21us

    /// "KEY_U" in input-event-codes.h
    | U = 22us

    /// "KEY_I" in input-event-codes.h
    | I = 23us

    /// "KEY_O" in input-event-codes.h
    | O = 24us

    /// "KEY_P" in input-event-codes.h
    | P = 25us

    /// "KEY_LEFTBRACE" in input-event-codes.h
    | LeftBrace = 26us

    /// "KEY_RIGHTBRACE" in input-event-codes.h
    | RightBrace = 27us

    /// "KEY_ENTER" in input-event-codes.h
    | Enter = 28us

    /// "KEY_LEFTCTRL" in input-event-codes.h
    | LeftCtrl = 29us

    /// "KEY_A" in input-event-codes.h
    | A = 30us

    /// "KEY_S" in input-event-codes.h
    | S = 31us

    /// "KEY_D" in input-event-codes.h
    | D = 32us

    /// "KEY_F" in input-event-codes.h
    | F = 33us

    /// "KEY_G" in input-event-codes.h
    | G = 34us

    /// "KEY_H" in input-event-codes.h
    | H = 35us

    /// "KEY_J" in input-event-codes.h
    | J = 36us

    /// "KEY_K" in input-event-codes.h
    | K = 37us

    /// "KEY_L" in input-event-codes.h
    | L = 38us

    /// "KEY_SEMICOLON" in input-event-codes.h
    | Semicolon = 39us

    /// "KEY_APOSTROPHE" in input-event-codes.h
    | Apostrophe = 40us

    /// "KEY_GRAVE" in input-event-codes.h
    | Grave = 41us

    /// "KEY_LEFTSHIFT" in input-event-codes.h
    | LeftShift = 42us

    /// "KEY_BACKSLASH" in input-event-codes.h
    | Backslash = 43us

    /// "KEY_Z" in input-event-codes.h
    | Z = 44us

    /// "KEY_X" in input-event-codes.h
    | X = 45us

    /// "KEY_C" in input-event-codes.h
    | C = 46us

    /// "KEY_V" in input-event-codes.h
    | V = 47us

    /// "KEY_B" in input-event-codes.h
    | B = 48us

    /// "KEY_N" in input-event-codes.h
    | N = 49us

    /// "KEY_M" in input-event-codes.h
    | M = 50us

    /// "KEY_COMMA" in input-event-codes.h
    | Comma = 51us

    /// "KEY_DOT" in input-event-codes.h
    | Period = 52us

    /// "KEY_SLASH" in input-event-codes.h
    | Slash = 53us

    /// "KEY_RIGHTSHIFT" in input-event-codes.h
    | RightShift = 54us

    /// "KEY_KPASTERISK" in input-event-codes.h
    | KeypadAsterisk = 55us

    /// "KEY_LEFTALT" in input-event-codes.h
    | LeftAlt = 56us

    /// "KEY_SPACE" in input-event-codes.h
    | Space = 57us

    /// "KEY_CAPSLOCK" in input-event-codes.h
    | CapsLock = 58us

    /// "KEY_F1" in input-event-codes.h
    | F1 = 59us

    /// "KEY_F2" in input-event-codes.h
    | F2 = 60us

    /// "KEY_F3" in input-event-codes.h
    | F3 = 61us

    /// "KEY_F4" in input-event-codes.h
    | F4 = 62us

    /// "KEY_F5" in input-event-codes.h
    | F5 = 63us

    /// "KEY_F6" in input-event-codes.h
    | F6 = 64us

    /// "KEY_F7" in input-event-codes.h
    | F7 = 65us

    /// "KEY_F8" in input-event-codes.h
    | F8 = 66us

    /// "KEY_F9" in input-event-codes.h
    | F9 = 67us

    /// "KEY_F10" in input-event-codes.h
    | F10 = 68us

    /// "KEY_NUMLOCK" in input-event-codes.h
    | NumberLock = 69us

    /// "KEY_SCROLLLOCK" in input-event-codes.h
    | ScrollLock = 70us

    /// "KEY_KP7" in input-event-codes.h
    | Keypad7 = 71us

    /// "KEY_KP8" in input-event-codes.h
    | Keypad8 = 72us

    /// "KEY_KP9" in input-event-codes.h
    | Keypad9 = 73us

    /// "KEY_KPMINUS" in input-event-codes.h
    | KeypadMinus = 74us

    /// "KEY_KP4" in input-event-codes.h
    | Keypad4 = 75us

    /// "KEY_KP5" in input-event-codes.h
    | Keypad5 = 76us

    /// "KEY_KP6" in input-event-codes.h
    | Keypad6 = 77us

    /// "KEY_KPPLUS" in input-event-codes.h
    | KeypadPlus = 78us

    /// "KEY_KP1" in input-event-codes.h
    | Keypad1 = 79us

    /// "KEY_KP2" in input-event-codes.h
    | Keypad2 = 80us

    /// "KEY_KP3" in input-event-codes.h
    | Keypad3 = 81us

    /// "KEY_KP0" in input-event-codes.h
    | Keypad0 = 82us

    /// "KEY_KPDOT" in input-event-codes.h
    | KeypadDot = 83us

    /// "KEY_ZENKAKUHANKAKU" in input-event-codes.h
    | ZenkakuHankaku = 85us

    /// "KEY_102ND" in input-event-codes.h
    | NonUsBackslashAndPipe = 86us

    /// "KEY_F11" in input-event-codes.h
    | F11 = 87us

    /// "KEY_F12" in input-event-codes.h
    | F12 = 88us

    /// "KEY_RO" in input-event-codes.h
    | Ro = 89us

    /// "KEY_KATAKANA" in input-event-codes.h
    | Katakana = 90us

    /// "KEY_HIRAGANA" in input-event-codes.h
    | Hiragana = 91us

    /// "KEY_HENKAN" in input-event-codes.h
    | Henkan = 92us

    /// "KEY_KATAKANAHIRAGANA" in input-event-codes.h
    | KatakanaHiragana = 93us

    /// "KEY_MUHENKAN" in input-event-codes.h
    | Muhenkan = 94us

    /// "KEY_KPJPCOMMA" in input-event-codes.h
    | KeypadJpComma = 95us

    /// "KEY_KPENTER" in input-event-codes.h
    | KeypadEnter = 96us

    /// "KEY_RIGHTCTRL" in input-event-codes.h
    | RightCtrl = 97us

    /// "KEY_KPSLASH" in input-event-codes.h
    | KeypadSlash = 98us

    /// "KEY_SYSRQ" in input-event-codes.h
    | SysRq = 99us

    /// "KEY_RIGHTALT" in input-event-codes.h
    | RightAlt = 100us

    /// "KEY_HOME" in input-event-codes.h
    | Home = 102us

    /// "KEY_UP" in input-event-codes.h
    | Up = 103us

    /// "KEY_PAGEUP" in input-event-codes.h
    | PageUp = 104us

    /// "KEY_LEFT" in input-event-codes.h
    | Left = 105us

    /// "KEY_RIGHT" in input-event-codes.h
    | Right = 106us

    /// "KEY_END" in input-event-codes.h
    | End = 107us

    /// "KEY_DOWN" in input-event-codes.h
    | Down = 108us

    /// "KEY_PAGEDOWN" in input-event-codes.h
    | PageDown = 109us

    /// "KEY_INSERT" in input-event-codes.h
    | Insert = 110us

    /// "KEY_DELETE" in input-event-codes.h
    | Delete = 111us

    /// "KEY_MUTE" in input-event-codes.h
    | Mute = 113us

    /// "KEY_VOLUMEDOWN" in input-event-codes.h
    | VolumeDown = 114us

    /// "KEY_VOLUMEUP" in input-event-codes.h
    | VolumeUp = 115us

    /// "KEY_POWER" in input-event-codes.h
    | Power = 116us

    /// "KEY_KPEQUAL" in input-event-codes.h
    | KeypadEqual = 117us

    /// "KEY_PAUSE" in input-event-codes.h
    | Pause = 119us

    /// "KEY_KPCOMMA" in input-event-codes.h
    | KeypadComma = 121us

    /// "KEY_HANGUEL" in input-event-codes.h
    | Hanguel = 122us

    /// "KEY_HANJA" in input-event-codes.h
    | Hanja = 123us

    /// "KEY_YEN" in input-event-codes.h
    | Yen = 124us

    /// "KEY_LEFTMETA" in input-event-codes.h
    | LeftMeta = 125us

    /// "KEY_RIGHTMETA" in input-event-codes.h
    | RightMeta = 126us

    /// "KEY_COMPOSE" in input-event-codes.h
    | Compose = 127us

    /// "KEY_STOP" in input-event-codes.h
    | Stop = 128us

    /// "KEY_AGAIN" in input-event-codes.h
    | Again = 129us

    /// "KEY_PROPS" in input-event-codes.h
    | Props = 130us

    /// "KEY_UNDO" in input-event-codes.h
    | Undo = 131us

    /// "KEY_FRONT" in input-event-codes.h
    | Front = 132us

    /// "KEY_COPY" in input-event-codes.h
    | Copy = 133us

    /// "KEY_OPEN" in input-event-codes.h
    | Open = 134us

    /// "KEY_PASTE" in input-event-codes.h
    | Paste = 135us

    /// "KEY_FIND" in input-event-codes.h
    | Find = 136us

    /// "KEY_CUT" in input-event-codes.h
    | Cut = 137us

    /// "KEY_HELP" in input-event-codes.h
    | Help = 138us

    /// "KEY_CALC" in input-event-codes.h
    | Calc = 140us

    /// "KEY_SLEEP" in input-event-codes.h
    | Sleep = 142us

    /// "KEY_WWW" in input-event-codes.h
    | Www = 150us

    /// "KEY_SCREENLOCK" in input-event-codes.h
    | ScreenLock = 152us

    /// "KEY_BACK" in input-event-codes.h
    | Back = 158us

    /// "KEY_FORWARD" in input-event-codes.h
    | Forward = 159us

    /// "KEY_EJECTCD" in input-event-codes.h
    | EjectCd = 161us

    /// "KEY_NEXTSONG" in input-event-codes.h
    | NextSong = 163us

    /// "KEY_PLAYPAUSE" in input-event-codes.h
    | PlayPause = 164us

    /// "KEY_PREVIOUSSONG" in input-event-codes.h
    | PreviousSong = 165us

    /// "KEY_STOPCD" in input-event-codes.h
    | StopCd = 166us

    /// "KEY_REFRESH" in input-event-codes.h
    | Refresh = 173us

    /// "KEY_EDIT" in input-event-codes.h
    | Edit = 176us

    /// "KEY_SCROLLUP" in input-event-codes.h
    | ScrollUp = 177us

    /// "KEY_SCROLLDOWN" in input-event-codes.h
    | ScrollDown = 178us

    /// "KEY_KPLEFTPAREN" in input-event-codes.h
    | KeypadLeftParen = 179us

    /// "KEY_KPRIGHTPAREN" in input-event-codes.h
    | KeypadRightParen = 180us

    /// "KEY_F13" in input-event-codes.h
    | F13 = 183us

    /// "KEY_F14" in input-event-codes.h
    | F14 = 184us

    /// "KEY_F15" in input-event-codes.h
    | F15 = 185us

    /// "KEY_F16" in input-event-codes.h
    | F16 = 186us

    /// "KEY_F17" in input-event-codes.h
    | F17 = 187us

    /// "KEY_F18" in input-event-codes.h
    | F18 = 188us

    /// "KEY_F19" in input-event-codes.h
    | F19 = 189us

    /// "KEY_F20" in input-event-codes.h
    | F20 = 190us

    /// "KEY_F21" in input-event-codes.h
    | F21 = 191us

    /// "KEY_F22" in input-event-codes.h
    | F22 = 192us

    /// "KEY_F23" in input-event-codes.h
    | F23 = 193us

    /// "KEY_F24" in input-event-codes.h
    | F24 = 194us

    /// "KEY_UNKNOWN" in input-event-codes.h
    | Unknown = 240us
