namespace reMarkable.fs.Display

open System
open SixLabors.ImageSharp.Formats

open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open reMarkable.fs.Display

/// Provides methods for encoding an <see cref="Image" /> to a RGB565 framebuffer stream
/// 
/// <param name="framebuffer">The hardware framebuffer to write data to</param>
/// <param name="srcArea">The area of the source image to encode</param>
/// <param name="destPoint">The location to place the top-leftmost corner of the source area on the destination framebuffer</param>
type Rgb565FramebufferEncoder(framebuffer: HardwareFramebuffer, srcArea: Rectangle, destPoint: Point)=
    interface IImageEncoder with
        member _.EncodeAsync<'TPixel when 'TPixel :> IPixel<'TPixel>>(_image: Image<'TPixel>, _stream: Stream)  =
            raise <| NotImplementedException()

        member _.Encode<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>, stream: Stream): unit =
            let buf: byte array = Array.zeroCreate (srcArea.Width * 2)
            
            let rgb565Buf: uint16 array = Array.zeroCreate srcArea.Width
            
            let mutable rgba32 = Rgba32()
            
            for y = 0 to srcArea.Height - 1 do
                let span = image.GetPixelRowSpan(y)

                for x = 0 to srcArea.Width - 1 do
                    span.[x].ToRgba32(&rgba32)
                    rgb565Buf.[x] <- Rgb565.Pack(rgba32.R, rgba32.G, rgba32.B)

                stream.Seek(framebuffer.PointToOffset(destPoint.X, destPoint.Y + y) |> int64, SeekOrigin.Begin)
                    |> ignore

                Buffer.BlockCopy(rgb565Buf, 0, buf, 0, buf.Length)
                stream.Write(buf, 0, buf.Length)
