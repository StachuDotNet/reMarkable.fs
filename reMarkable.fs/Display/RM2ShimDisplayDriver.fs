namespace reMarkable.fs.Display

open System
open System.Diagnostics.CodeAnalysis
open System.Runtime.ConstrainedExecution
open System.Runtime.InteropServices
open Microsoft.FSharp.Core
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open reMarkable.fs
open reMarkable.fs.Display.Framebuffer
open reMarkable.fs.UnixExceptions

module Driver = 
    [<ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>]
    [<DllImport("librm2fb_client.so.1.0.1", EntryPoint = "close", SetLastError = true)>]
    extern int Close(IntPtr handle)

    [<DllImport("librm2fb_client.so.1.0.1", EntryPoint = "open", SetLastError = false)>]
    [<SuppressMessage(
        category = "Globalization",
        checkId = "CA2101:Specify marshaling for P/Invoke string arguments",
        Justification = "Specifying a marshaling breaks rM compatibility"
    )>]
    extern reMarkable.fs.Util.Stream.SafeUnixHandle Open(string path, uint flags, UnixFileMode mode)

    [<DllImport("librm2fb_client.so.1.0.1", EntryPoint = "ioctl", SetLastError = false)>]
    extern int Ioctl(SafeHandle handle, IoctlDisplayCommand code, FrameBufferUpdateData& data)
    
    
/// Provides methods for interacting with the rm2fb client used on a rm2 device
type RM2ShimDisplayDriver() =
    let devicePath = "/dev/fb0";

    /// The device handle through which IOCTL commands can be issued
    let handle = Driver.Open(devicePath, 0u, UnixFileMode.WriteOnly)
    
    let width, height = 1404, 1872
    
    let framebuffer = new HardwareFramebuffer(devicePath, width, height)
    
    interface IDisposable with
        member _.Dispose() =
            handle.Dispose()
            (framebuffer :> IDisposable).Dispose()
            
    interface IDisplayDriver with
        member _.Width = width
        member _.Height = height
        member _.Framebuffer = framebuffer :> IFramebuffer

        member this.DrawAndRefresh(args: DrawAndRefreshArgs): unit =
            // first write to framebuffer
            let func =
                (fun width rect point ->
                    Rgb565.Rgb565FramebufferEncoder(width, rect, point)
                    :> IImageEncoder
                )
            (framebuffer :> IFramebuffer).Write(args.Image, args.SrcArea, args.DestPoint, func)
        
            // then refresh the target area
            let refreshArea =
                match args.RefreshArea with
                | None -> Rectangle(args.DestPoint, args.SrcArea.Size)
                | Some rect -> rect
            let waveformMode = args.WaveformMode |> Option.defaultValue WaveformMode.Auto
            let displayTemp = args.DisplayTemp |> Option.defaultValue DisplayTemp.Papyrus
            let updateMode = args.UpdateMode |> Option.defaultValue UpdateMode.Partial

            (this :> IDisplayDriver).Refresh(refreshArea, waveformMode, displayTemp, updateMode)

        member _.Refresh(rectangle: Rectangle, mode: WaveformMode, displayTemp: DisplayTemp, updateMode: UpdateMode): unit =
            let rectangle = (framebuffer :> IFramebuffer).ConstrainRectangle(rectangle)
            
            let mutable data = FrameBufferUpdateData()
            
            data.UpdateRegion <-
                let mutable rect = FrameBufferRectangle()
                rect.X <- (uint)rectangle.X
                rect.Y <- (uint)rectangle.Y
                rect.Width <- (uint)rectangle.Width
                rect.Height <- (uint)rectangle.Height
                rect
            data.WaveformMode <- mode
            data.UpdateMode <- updateMode
            data.UpdateMarker <- 0u
            data.DisplayTemp <- displayTemp
            data.Flags <- 0u
            data.DitherMode <- 0 
            data.QuantBit <- 0
            //data.AltData <- null
       
            let retCode = Driver.Ioctl(handle :> SafeHandle, IoctlDisplayCommand.SendUpdate, &data)
            
            if retCode = -1 then
                raise <| UnixException(None, None)