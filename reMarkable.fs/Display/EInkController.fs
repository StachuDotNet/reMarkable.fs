namespace reMarkable.fs.Display

open System.Runtime.InteropServices
open reMarkable.fs.Util.Stream
open reMarkable.fs.Unix.Driver.Display.Framebuffer

// public enum AutoUpdateMode : uint
// {
//     Region = 0,
//     Automatic = 1
// }
    
// public struct UpdateMarkerData
// {
//     public uint UpdateMarker;
//     public uint CollisionTest;
// }
    
type UpdateMode =
    | Partial = 0u
    | Full = 1u
    
type UpdateScheme =
    | Snapshot = 0
    | Queue = 1
    | QueueAndMerge = 2

type WaveformMode = 
    // /// Screen goes to white (clears)
    // Init = 0x0,
    //
    // /// Basically A2 (so partial refresh shouldn't be possible here)
    // Glr16 = 0x4,
    //
    // /// Official -- and enables Regal D Processing
    // Gld16 = 0x5,
    //
    // /// [Direct Update] Grey->white/grey->black  -- remarkable uses this for drawing
    // Du = 0x1,
    //
    // /// High fidelity (flashing)
    // Gc16 = 0x2,
    //
    // /// Medium fidelity  -- remarkable uses this for UI
    // Gc16Fast = 0x3,
    //
    // /// Medium fidelity from white transition
    // Gl16Fast = 0x6,
    //
    // /// Medium fidelity 4 level of gray direct update
    // Du4 = 0x7,
    //
    // /// Ghost compensation waveform
    // Reagl = 0x8,
    //
    // /// Ghost compensation waveform with dithering
    // Reagld = 0x9,
    //
    // /// 2-bit from white transition
    // Gl4 = 0xA,
    //
    // /// High fidelity for black transition
    // Gl16Inv = 0xB,

    /// Official
    | Auto = 257u
    
// [StructLayout(LayoutKind.Sequential)]
// public struct WaveformModes
// {
//     public int ModeInit;
//     public int ModeDu;
//     public int ModeGc4;
//     public int ModeGc8;
//     public int ModeGc16;
//     public int ModeGc32;
// }

type DisplayTemp =
    | Ambient = 0x1000us
    | Papyrus = 0x1001us
    | RemarkableDraw = 0x0018us
    | Max = 0xFFFFus

/// Display driver commands, e-paper commands OR'd with 0x40484600
type IoctlDisplayCommand =
    // /// Takes <see cref="WaveformModes" />
    // SetWaveformModes = 0x4048462B,
    //
    // /// Takes <seealso cref="DisplayTemp" />
    // SetTemperature = 0x4048462C,
    //
    // /// Takes <seealso cref="AutoUpdateMode" />
    // SetAutoUpdateMode = 0x4048462D,

    /// Takes <see cref="FbUpdateData" />
    | SendUpdate = 0x4048462Eu

    // /// Takes <see cref="UpdateMarkerData" />
    // WaitForUpdateComplete = 0x4048462F,
    //
    // /// Takes uint
    // SetPowerDownDelay = 0x40484630,
    //
    // /// Takes uint
    // GetPowerDownDelay = 0x40484631,
    //
    // /// Takes <seealso cref="UpdateScheme" />
    // SetUpdateScheme = 0x40484632,
    //
    // /// Takes ulong
    // GetWorkBuffer = 0x40484634,
    //
    // /// Takes uint
    // SetTempAutoUpdatePeriod = 0x40484636,
    //
    // /// No params
    // DisableEpdcAccess = 0x40484635,
    //
    // /// No params
    // EnableEpdcAccess = 0x40484636,
    | GetVariableScreenInfo = 0x4600u
    | PutVariableScreenInfo = 0x4601u
    // GetFixedScreenInfo = 0x4602,
    // GetCmap = 0x4604,
    // PutCmap = 0x4605,
    // PanDisplay = 0x4606,
    // Cursor = 0x4608


[<StructLayout(LayoutKind.Sequential)>]
type FbRect =
  struct
    val mutable Y: uint
    val mutable X: uint
    val mutable Width: uint
    val mutable Height: uint
  end
  
[<StructLayout(LayoutKind.Sequential)>]
type FbAltBufferData =
  struct
    val mutable PhysicalAddress: uint
    val mutable Width: uint
    val mutable Height: uint
    val mutable AltUpdateRegion: FbRect
  end

[<StructLayout(LayoutKind.Sequential)>]
type FbUpdateData =
  struct
    val mutable UpdateRegion: FbRect
    val mutable WaveformMode: WaveformMode
    val mutable UpdateMode: UpdateMode
    val mutable UpdateMarker: uint
    val mutable DisplayTemp: DisplayTemp
    val mutable Flags: uint
    val mutable DitherMode: int
    val mutable QuantBit: int
    val mutable AltData: FbAltBufferData
  end

/// Provides methods for interacting with the device subsystem from userspace
module DisplayIoctl =
    /// The IOCTL handle for <see cref="FbUpdateData" /> payloads
    [<DllImport("libc", EntryPoint = "ioctl", SetLastError = true)>]
    extern int Ioctl1(SafeUnixHandle handle, IoctlDisplayCommand request, FbUpdateData& data);

    /// The IOCTL handle for <see cref="FbVarScreenInfo" /> payloads
    [<DllImport("libc", EntryPoint = "ioctl", SetLastError = true)>]
    extern int Ioctl2(SafeUnixHandle handle, IoctlDisplayCommand request, FbVarScreenInfo& data);

    // /// The IOCTL handle for <see cref="FbFixedScreenInfo" /> payloads
    // [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
    // public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbFixedScreenInfo data);
