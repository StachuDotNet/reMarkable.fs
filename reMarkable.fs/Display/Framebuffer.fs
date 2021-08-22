module reMarkable.fs.Display.Framebuffer

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

/// Provides an interface through which to access the device's framebuffer
type IFramebuffer =
    /// Constrains a rectangle to not overlap the visible bounds of the display
    /// <param name="area">The source location and area to constrain</param>
    abstract member ConstrainRectangle: Rectangle -> Rectangle

    // /// Constrains a rectangle to not overlap the visible bounds of the display
    // /// <param name="srcArea">The source area to constrain</param>
    // /// <param name="destPoint">The point in the buffer where the source area should be placed</param>
    abstract member ConstrainRectangleToPoint: Rectangle * Point -> Rectangle * Point

    /// Sets exactly one pixel in the framebuffer to the specified color
    /// <param name="x">The X coordinate of the pixel</param>
    /// <param name="y">The Y coordinate of the pixel</param>
    /// <param name="color">The color to set the pixel to</param>
    abstract member SetPixel: {| X: int; Y: int; Color: Color |} -> unit
    
    /// The height of the buffer
    abstract member Height: int

    /// The width of the buffer
    abstract member Width: int

    /// Reads a rectangular region of pixels from the buffer
    /// <param name="area">The region of pixels to read</param>
    /// <returns>The pixels as an <see cref="Image{Rgb24}" /></returns>
    abstract member Read: Rectangle -> Image<Rgb24> 
    
    /// Writes a rectangular region of pixels to the buffer
    /// <typeparam name="TPixel">The pixel format</typeparam>
    /// <param name="image">The pixels to write to the buffer</param>
    /// <param name="srcArea">The source area of the image to draw</param>
    /// <param name="destPoint">The point in the buffer where the source area will be drawn</param>
    abstract member Write: Image<'TPixel> * srcArea:Rectangle * destPoint:Point * (int -> Rectangle -> Point -> IImageEncoder) -> unit


/// Provides methods for interacting with the device's framebuffer
type HardwareFramebuffer(devicePath: string, width: int, height: int) = 
    /// The visible bounds of the buffer
    let visibleBounds = Rectangle(0, 0, width, height)
    
    /// The framebuffer stream
    let deviceStream = File.Open(devicePath, FileMode.Open);
    
    /// The buffer file location 
    member _.DevicePath = devicePath
    
    interface IFramebuffer with
        member _.Width = width
        member _.Height = height
        
        member _.ConstrainRectangle(area: Rectangle): Rectangle =
            let result = area // ok?
            result.Intersect(visibleBounds)
            result
    
        member _.ConstrainRectangleToPoint(srcArea: Rectangle, destPoint: Point): Rectangle * Point =
            let tempRect = Rectangle(destPoint, srcArea.Size)
            tempRect.Intersect(visibleBounds)
            tempRect, tempRect.Location
            
        
        member this.Read(area: Rectangle): Image<Rgb24> =
            let area = (this :> IFramebuffer).ConstrainRectangle(area)
            Image.Load<Rgb24>(deviceStream, Rgb565.Rgb565FramebufferDecoder(width, area))
            
        member this.SetPixel (data: {| X: int; Y: int; Color: Color |}): unit =
            deviceStream.Seek(Rgb565.PointToOffset(width, data.X, data.Y) |> int64, SeekOrigin.Begin)
                |> ignore
                
            let rgb = data.Color.ToPixel<Rgb24>()
            
            deviceStream.Write(BitConverter.GetBytes(Rgb565.Pack(rgb.R, rgb.G, rgb.B)), 0, 2)

        /// Writes an image to the framebuffer
        member this.Write<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>,
                                                                   srcArea: Rectangle,
                                                                   destPoint: Point,
                                                                   makeEncoder: int -> Rectangle -> Point -> IImageEncoder) =
            let point = Point(destPoint.X, destPoint.Y)
            let x, y = point.Y, point.Y
            
            //todo: something like this
            //let srcArea, destPoint = (this :> IFramebuffer).ConstrainRectangleToPoint(srcArea, destPoint)
            //srcArea.Location <- Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y)

            //printfn "Area , Point: %A, %A" srcArea destPoint
            
            let encoder = makeEncoder width srcArea destPoint
            
            image.Clone(fun srcImage -> srcImage.Crop(srcArea) |> ignore)
                .Save(deviceStream, encoder)


    interface IDisposable with
        member _.Dispose() = deviceStream.Dispose()
