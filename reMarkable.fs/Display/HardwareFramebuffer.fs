namespace reMarkable.fs.Display

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

[<AbstractClass>]
type Hack () =
    // this hack exists due to some awkward F# constraint around byref... something to follow up on.
    abstract member Hack : byref<Rectangle> -> unit

/// Provides methods for interacting with the device's framebuffer
type HardwareFramebuffer(devicePath: string, visibleWidth: int, visibleHeight: int, virtualWidth: int, virtualHeight: int) = 
    /// The visible bounds of the buffer
    let visibleBounds = Rectangle(0, 0, visibleWidth, visibleHeight)
    
    /// The framebuffer stream
    let deviceStream = File.Open(devicePath, FileMode.Open);
    
    /// The buffer file location 
    member _.DevicePath = devicePath
    
    member _.VisibleWidth = visibleWidth
    member _.VisibleHeight = visibleHeight
    member _.VirtualWidth = virtualWidth
    member _.VirtualHeight = virtualHeight
        

    interface IDisposable with
        member _.Dispose() = deviceStream.Dispose()
    
    interface IFramebuffer with
        member _.ConstrainRectangle(area: Rectangle): Rectangle =
            let result = area // ok?
            result.Intersect(visibleBounds)
            result
    
        member _.ConstrainRectangleToPoint(srcArea: Rectangle, destPoint: Point): Rectangle * Point =
            let tempRect = Rectangle(destPoint, srcArea.Size)
            tempRect.Intersect(visibleBounds)
            tempRect, tempRect.Location

        member this.SetPixel (data: {| X: int; Y: int; Color: Color |}): unit =
            deviceStream.Seek(this.PointToOffset(data.X, data.Y) |> int64, SeekOrigin.Begin)
                |> ignore
                
            let rgb = data.Color.ToPixel<Rgb24>()
            
            deviceStream.Write(BitConverter.GetBytes(Rgb565.Pack(rgb.R, rgb.G, rgb.B)), 0, 2)

    /// Finds the stream location corresponding to the pixel coordinates
    /// 
    /// <param name="x">The pixel X coordinate</param>
    /// <param name="y">The pixel Y coordinate</param>
    /// <returns>The stream location</returns>
    member _.PointToOffset(x: int, y: int) = (virtualWidth * y + x) * 2

    /// Writes an image to the framebuffer
    member this.Write<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>,
                                                               srcArea: Rectangle,
                                                               destPoint: Point,
                                                               makeEncoder: HardwareFramebuffer -> Rectangle ->Point -> IImageEncoder) =
        let point = Point(destPoint.X, destPoint.Y)
        let x, y = point.Y, point.Y
        
        printfn $"X: {x}, Y: {y}"
        
        //todo: something like this
        //let srcArea, destPoint = (this :> IFramebuffer).ConstrainRectangleToPoint(srcArea, destPoint)
        //srcArea.Location <- Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y)

        printfn "Area , Point: %A, %A" srcArea destPoint
        
        let encoder = makeEncoder this srcArea destPoint
        
        image
            .Clone(fun srcImage -> srcImage.Crop(srcArea) |> ignore)
            .Save(deviceStream, encoder)

    
//    member this.Read(area: Rectangle): Image<Rgb24> =
//        let area = this.ConstrainRectangle(area)
//        Image.Load<Rgb24>(deviceStream, new Rgb565FramebufferDecoder(this, area))
