namespace reMarkable.fs.Display

open System
open System.Runtime.InteropServices
open reMarkable.fs.Util.Stream

[<Flags>]
type FbSync = // : uint
    | None = 0u
    | HorizontalHighActive = 1u
    | VerticalHighActive = 2u
    | External = 4u
    | CompositeHighActive = 8u
    | BroadcastTimings = 16u


//public struct FbFixedScreenInfo
//    // /// identification string eg "TT Builtin"
//    // public unsafe fixed char Id[16];
//    //
//    // /// Start of frame buffer mem (physical address)
//    // public ulong SmemStart;
//    //
//    // /// Length of frame buffer mem
//    // public uint SmemLen;
//    //
//    // /// see FB_TYPE_*
//    // public uint Type;
//    //
//    // /// Interleave for interleaved Planes
//    // public uint TypeAux;
//    //
//    // /// see FB_VISUAL_*
//    // public uint Visual;
//    //
//    // /// zero if no hardware panning
//    // public ushort XPanStep;
//    //
//    // /// zero if no hardware panning
//    // public ushort YPanStep;
//    //
//    // /// zero if no hardware ywrap
//    // public ushort YWrapStep;
//    //
//    // /// length of a line in bytes
//    // public uint LineLength;
//    //
//    // /// Start of Memory Mapped I/O (physical address)
//    // public ulong MmioStart;
//    //
//    // /// Length of Memory Mapped I/O
//    // public uint MmioLen;
//    //
//    // /// Indicate to driver which specific chip/card we have
//    // public uint Accel;
//    //
//    // /// see FB_CAP_*
//    // public ushort Capabilities;
//    //
//    // /// Reserved for future compatibility
//    // public ushort Reserved0;
//    //
//    // /// Reserved for future compatibility
//    // public ushort Reserved1;

[<Flags>]
type FbVMode = // : uint
    | NonInterlaced = 0u
    | Interlaced = 1u
    | Double = 2u
    | OddFieldFirst = 4u
    | YWrap = 256u
    | SmoothXPan = 512u
    
type FbBitfield =
    struct // some of the below may need to be made mutable?
        /// beginning of bitfield
        val Offset: uint

        /// length of bitfield
        val Length: uint

        /// != 0 : Most significant bit is right
        val IsMsbRight: uint
    end

type FbVarScreenInfo =
    struct
        val mutable VisibleResolutionX: uint
        val mutable VisibleResolutionY: uint

        val VirtualResolutionX: uint
        val VirtualResolutionY: uint

        val VisibleOffsetX: uint;
        
        val VisibleOffsetY: uint;
        
        val BitsPerPixel: uint;
        
        ///  0 = color, 1 = grayscale
        val IsGrayscale: uint
        
        val Red: FbBitfield;
        
        val Green: FbBitfield;
        
        val Blue: FbBitfield;
        
        val Transparency: FbBitfield;
        
        /// != 0 Non standard pixel format
        val NonStandardPixelFormat: uint
        
        /// see FB_ACTIVATE_*
        val Activate: uint
        
        /// height of picture in mm
        val Height: uint
        
        /// width of picture in mm
        val Width: uint
        
        /// (OBSOLETE) see fb_info.flags
        val AccelFlags: uint
        
        /// pixel clock in ps (picoseconds)
        val PixClock: uint
        
        /// time from sync to picture in pixclocks
        val LeftMargin: uint
        
        /// time from picture to sync in pixclocks
        val RightMargin: uint
        
        /// time from sync to picture in pixclocks
        val UpperMargin: uint
        
        /// time from picture to sync in pixclocks
        val LowerMargin: uint
        
        /// length of horizontal sync in pixclocks
        val HSyncLen: uint
        
        /// length of vertical sync in pixclocks
        val VSyncLen: uint
        
        /// see FB_SYNC_*
        val Sync: FbSync
        
        /// ee FB_VMODE_*
        val VMode: FbVMode
        
        /// angle we rotate counter clockwise
        val Rotate: uint
        
        /// colorspace for FOURCC-based modes
        val Colorspace: uint
        
        /// Reserved for future compatibility
        val Reserved0: uint
        
        /// Reserved for future compatibility
        val Reserved1: uint
        
        /// Reserved for future compatibility
        val Reserved2: uint
        
        /// Reserved for future compatibility
        val Reserved3: uint
    end

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

/// Provides methods for interacting with the device subsystem from userspace
module DisplayIoctl =
    /// The IOCTL handle for <see cref="FbUpdateData" /> payloads
    [<DllImport("libc", EntryPoint = "ioctl", SetLastError = true)>]
    extern int Ioctl1(SafeUnixHandle handle, IoctlDisplayCommand request, FrameBufferUpdateData& data);

    /// The IOCTL handle for <see cref="FbVarScreenInfo" /> payloads
    [<DllImport("libc", EntryPoint = "ioctl", SetLastError = true)>]
    extern int Ioctl2(SafeUnixHandle handle, IoctlDisplayCommand request, FbVarScreenInfo& data);

    // /// The IOCTL handle for <see cref="FbFixedScreenInfo" /> payloads
    // [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
    // public static extern int Ioctl(SafeUnixHandle handle, IoctlDisplayCommand request, ref FbFixedScreenInfo data);


// /// Provides methods for interacting with the hardware display installed in the device
// public sealed class HardwareDisplayDriver : IDisposable, IDisplayDriver
//     /// The device handle through which IOCTL commands can be issued
//     private readonly StreamStuff.SafeUnixHandle _handle;
//
//     /// The update marker ID returned by the device
//     private uint _updateMarker = 0;
//
//     /// The device handle location
//     public string DevicePath { get; }
//
//     public IFramebuffer Framebuffer { get; }
//
//     public int VirtualHeight { get; }
//
//     public int VirtualWidth { get; }
//
//     public int VisibleHeight { get; }
//
//     public int VisibleWidth { get; }
//
//     /// Creates a new <see cref="HardwareDisplayDriver" />
//     /// <param name="devicePath">The device handle location</param>
//     public HardwareDisplayDriver(string devicePath)
//         DevicePath = devicePath;
//
//         _handle = StreamStuff.Open(DevicePath, 0, UnixFileMode.WriteOnly);
//
//         var vinfo = GetVariableScreenInfo();
//
//         VisibleWidth = (int)vinfo.VisibleResolutionX;
//         VisibleHeight = (int)vinfo.VisibleResolutionY;
//         VirtualWidth = (int)vinfo.VirtualResolutionX;
//         VirtualHeight = (int)vinfo.VirtualResolutionY;
//         Framebuffer = new HardwareFramebuffer(devicePath, VisibleWidth, VisibleHeight, VirtualWidth, VirtualHeight);
//
//         // vinfo.AccelFlags = 0x01;
//         // vinfo.Width = 0xFFFFFFFF;
//         // vinfo.Height = 0xFFFFFFFF;
//         // vinfo.Rotate = 1;
//         // vinfo.PixClock = 160000000;
//         vinfo.VisibleResolutionX = 1872;
//         vinfo.VisibleResolutionY = 1404;
//         // vinfo.LeftMargin = 32;
//         // vinfo.RightMargin = 326;
//         // vinfo.UpperMargin = 4;
//         // vinfo.LowerMargin = 12;
//         // vinfo.HSyncLen = 44;
//         // vinfo.VSyncLen = 1;
//         // vinfo.Sync = FbSync.None;
//         // vinfo.VMode = FbVMode.NonInterlaced;
//         // vinfo.BitsPerPixel = sizeof(short) * 8;
//
//         PutVarScreenInfo(vinfo);
//
//     /// <inheritdoc />
//     public void Dispose()
//         _handle?.Dispose();
//         ((HardwareFramebuffer)Framebuffer)?.Dispose();
//
//     /// <inheritdoc />
//     public void Draw(DrawArgs args)
//         //Rectangle refreshArea = default,
//         //WaveformMode waveformMode = WaveformMode.Auto
//         //DisplayTemp displayTemp = DisplayTemp.Papyrus,
//         //UpdateMode updateMode = UpdateMode.Partial
//         
//         Framebuffer.Write(args.Image, args.SrcArea, args.DestPoint);
//     
//         // todo:
//         // if (FSharpOption<Rectangle>.get_IsNone(args.RefreshArea))
//         //     refreshArea.Location = destPoint;
//         //     refreshArea.Size = srcArea.Size;
//     
//         var waveformMode = WaveformMode.Auto;// todo: respect incoming.
//         var displayTemp = DisplayTemp.RemarkableDraw; // todo: respect incoming
//         var updateMode = UpdateMode.Full; // todo: respect incoming
//     
//         // todo: deal with empty refreshArea
//         Refresh(args.RefreshArea.Value, waveformMode, displayTemp, updateMode);
//
//     // /// Reads fixed device information from the display
//     // /// <returns>A populated <see cref="FbFixedScreenInfo" /></returns>
//     // public FbFixedScreenInfo GetFixedScreenInfo()
//     //     var screenInfo = new FbFixedScreenInfo();
//     //     
//     //     if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetFixedScreenInfo, ref screenInfo) == -1)
//     //         throw new UnixException();
//     //     
//     //     return screenInfo;
//
//     /// Reads variable device information from the display
//     /// <returns>A populated <see cref="FbVarScreenInfo" /></returns>
//     public FbVarScreenInfo GetVariableScreenInfo()
//         var variableInfo = new FbVarScreenInfo();
//         
//         if (DisplayIoctl.Ioctl2(_handle, IoctlDisplayCommand.GetVariableScreenInfo, ref variableInfo) == -1)
//             throw new UnixException();
//         
//         return variableInfo;
//
//     /// Writes variable device to the display
//     /// <param name="variableInfo">A populated <see cref="FbVarScreenInfo" /></param>
//     public void PutVarScreenInfo(FbVarScreenInfo variableInfo)
//         if (DisplayIoctl.Ioctl2(_handle, IoctlDisplayCommand.PutVariableScreenInfo, ref variableInfo) == -1)
//             throw new UnixException();
//
//     public void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode)
//     {
//         let rectangle = Framebuffer.ConstrainRectangle(rectangle)
//         var data = new FbUpdateData
//         {
//             UpdateRegion = new FbRect
//             {
//                 X = (uint)rectangle.X,
//                 Y = (uint)rectangle.Y,
//                 Width = (uint)rectangle.Width,
//                 Height = (uint)rectangle.Height
//             },
//             WaveformMode = mode,
//             DisplayTemp = displayTemp,
//             UpdateMode = updateMode,
//             UpdateMarker = _updateMarker,
//             DitherMode = 0,
//             QuantBit = 0,
//             Flags = 0
//         };
//     
//         var retCode = DisplayIoctl.Ioctl1(_handle, IoctlDisplayCommand.SendUpdate, ref data);
//         if (retCode == -1)
//             throw new UnixException();
//     
//         _updateMarker = (uint) retCode;
