namespace reMarkable.fs.Display

// /// Provides methods for interacting with the hardware display installed in the device
// public sealed class HardwareDisplayDriver : IDisposable, IDisplayDriver
// {
//     /// The device handle through which IOCTL commands can be issued
//     private readonly StreamStuff.SafeUnixHandle _handle;
//
//     /// The update marker ID returned by the device
//     private uint _updateMarker = 0;
//
//     /// The device handle location
//     public string DevicePath { get; }
//
//     /// <inheritdoc />
//     public IFramebuffer Framebuffer { get; }
//
//     /// <inheritdoc />
//     public int VirtualHeight { get; }
//
//     /// <inheritdoc />
//     public int VirtualWidth { get; }
//
//     /// <inheritdoc />
//     public int VisibleHeight { get; }
//
//     /// <inheritdoc />
//     public int VisibleWidth { get; }
//
//     /// Creates a new <see cref="HardwareDisplayDriver" />
//     /// <param name="devicePath">The device handle location</param>
//     public HardwareDisplayDriver(string devicePath)
//     {
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
//     }
//
//     /// <inheritdoc />
//     public void Dispose()
//     {
//         _handle?.Dispose();
//         ((HardwareFramebuffer)Framebuffer)?.Dispose();
//     }
//
//     /// <inheritdoc />
//     public void Draw(DrawArgs args)
//     {
//         //Rectangle refreshArea = default,
//         //WaveformMode waveformMode = WaveformMode.Auto
//         //DisplayTemp displayTemp = DisplayTemp.Papyrus,
//         //UpdateMode updateMode = UpdateMode.Partial
//         
//         Framebuffer.Write(args.Image, args.SrcArea, args.DestPoint);
//     
//         // todo:
//         // if (FSharpOption<Rectangle>.get_IsNone(args.RefreshArea))
//         // {
//         //     refreshArea.Location = destPoint;
//         //     refreshArea.Size = srcArea.Size;
//         // }
//     
//         var waveformMode = WaveformMode.Auto;// todo: respect incoming.
//         var displayTemp = DisplayTemp.RemarkableDraw; // todo: respect incoming
//         var updateMode = UpdateMode.Full; // todo: respect incoming
//     
//         // todo: deal with empty refreshArea
//         Refresh(args.RefreshArea.Value, waveformMode, displayTemp, updateMode);
//     }
//
//     // /// Reads fixed device information from the display
//     // /// <returns>A populated <see cref="FbFixedScreenInfo" /></returns>
//     // public FbFixedScreenInfo GetFixedScreenInfo()
//     // {
//     //     var screenInfo = new FbFixedScreenInfo();
//     //     
//     //     if (DisplayIoctl.Ioctl(_handle, IoctlDisplayCommand.GetFixedScreenInfo, ref screenInfo) == -1)
//     //         throw new UnixException();
//     //     
//     //     return screenInfo;
//     // }
//
//     /// Reads variable device information from the display
//     /// <returns>A populated <see cref="FbVarScreenInfo" /></returns>
//     public FbVarScreenInfo GetVariableScreenInfo()
//     {
//         var variableInfo = new FbVarScreenInfo();
//         
//         if (DisplayIoctl.Ioctl2(_handle, IoctlDisplayCommand.GetVariableScreenInfo, ref variableInfo) == -1)
//             throw new UnixException();
//         
//         return variableInfo;
//     }
//
//     /// Writes variable device to the display
//     /// <param name="variableInfo">A populated <see cref="FbVarScreenInfo" /></param>
//     public void PutVarScreenInfo(FbVarScreenInfo variableInfo)
//     {
//         if (DisplayIoctl.Ioctl2(_handle, IoctlDisplayCommand.PutVariableScreenInfo, ref variableInfo) == -1)
//             throw new UnixException();
//     }
//
//     /// <inheritdoc />
//     public void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode)
//     {
//         Framebuffer.ConstrainRectangle(ref rectangle);
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
//     }
// }
