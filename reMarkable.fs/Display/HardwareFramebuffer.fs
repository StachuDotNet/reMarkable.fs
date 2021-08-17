namespace reMarkable.fs.Display

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.Display.Rgb565

/// Provides methods for interacting with the device's framebuffer
type HardwareFramebuffer(devicePath: string, visibleWidth: int, visibleHeight: int, virtualWidth: int, virtualHeight: int) = 
    /// The visible bounds of the buffer
    let visibleBounds = Rectangle(0, 0, visibleWidth, visibleHeight)
    
    /// The framebuffer stream
    let deviceStream = File.Open(devicePath, FileMode.Open);
    
    /// The buffer file location 
    member _.DevicePath = devicePath
    
    interface IFramebuffer with
        member _.VisibleWidth = visibleWidth
        member _.VisibleHeight = visibleHeight
        member _.VirtualWidth = virtualWidth
        member _.VirtualHeight = virtualHeight
        
        member _.ConstrainRectangle(area: Rectangle): Rectangle =
            let result = area // ok?
            result.Intersect(visibleBounds)
            result
    
        member _.ConstrainRectangleToPoint(srcArea: Rectangle, destPoint: Point): Rectangle * Point =
            let tempRect = Rectangle(destPoint, srcArea.Size)
            tempRect.Intersect(visibleBounds)
            tempRect, tempRect.Location
            
        member _.PointToOffset(x: int, y: int) =
            (virtualWidth * y + x) * 2

        member this.SetPixel (data: {| X: int; Y: int; Color: Color |}): unit =
            deviceStream.Seek((this :> IFramebuffer).PointToOffset(data.X, data.Y) |> int64, SeekOrigin.Begin)
                |> ignore
                
            let rgb = data.Color.ToPixel<Rgb24>()
            
            deviceStream.Write(BitConverter.GetBytes(Rgb565.Pack(rgb.R, rgb.G, rgb.B)), 0, 2)
            
        
        member this.Read(area: Rectangle): Image<Rgb24> =
            let area = (this :> IFramebuffer).ConstrainRectangle(area)
            Image.Load<Rgb24>(deviceStream, Rgb565FramebufferDecoder(this, area))

        /// Writes an image to the framebuffer
        member this.Write<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>,
                                                                   srcArea: Rectangle,
                                                                   destPoint: Point,
                                                                   makeEncoder: IFramebuffer -> Rectangle -> Point -> IImageEncoder) =
            let point = Point(destPoint.X, destPoint.Y)
            let x, y = point.Y, point.Y
            
            //todo: something like this
            //let srcArea, destPoint = (this :> IFramebuffer).ConstrainRectangleToPoint(srcArea, destPoint)
            //srcArea.Location <- Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y)

            printfn "Area , Point: %A, %A" srcArea destPoint
            
            let encoder = makeEncoder this srcArea destPoint
            
            image.Clone(fun srcImage -> srcImage.Crop(srcArea) |> ignore)
                .Save(deviceStream, encoder)


    interface IDisposable with
        member _.Dispose() = deviceStream.Dispose()
