namespace reMarkable.fs.Display

open System
open System.IO
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

[<AbstractClass>]
type Hack () =
    abstract member Hack : byref<Rectangle> -> unit

/// Provides methods for interacting with the device's framebuffer
type HardwareFramebuffer(devicePath: string, visibleWidth: int, visibleHeight: int, virtualWidth: int, virtualHeight: int) = 

    /// The visible bounds of the buffer
    let visibleBounds = new Rectangle(0, 0, visibleWidth, visibleHeight)
    
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
    
        member _.ConstrainRectangle(area: byref<Rectangle>) =
            area.Intersect(visibleBounds)
        
    
    member _.ConstrainRectangle(srcArea: byref<Rectangle> ,destPoint: byref<Point>) =
            let tempRect = Rectangle(destPoint, srcArea.Size)
            tempRect.Intersect(visibleBounds);
            srcArea.Size <- tempRect.Size;
            destPoint <- tempRect.Location;


    /// Finds the stream location corresponding to the pixel coordinates
    /// 
    /// <param name="x">The pixel X coordinate</param>
    /// <param name="y">The pixel Y coordinate</param>
    /// <returns>The stream location</returns>
    member _.PointToOffset(x: int, y: int) =
        (virtualWidth * y + x) * 2

    member this.Write<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>,
                                                               srcArea: Rectangle,
                                                               destPoint: Point,
                                                               makeEncoder: HardwareFramebuffer -> Rectangle ->Point -> IImageEncoder) =
        let point = Point(destPoint.X, destPoint.Y)
        let x, y = point.Y, point.Y
        
        let mutable srcArea = srcArea // is this OK?
        
        this.ConstrainRectangle(ref srcArea, ref destPoint)
        srcArea.Location <- new Point(srcArea.Location.X + destPoint.X - x, srcArea.Location.Y + destPoint.Y - y);

        image
            .Clone(fun srcImage -> srcImage.Crop(srcArea) |> ignore)
            .Save(deviceStream, makeEncoder this srcArea destPoint)

    

    // /// Finds the pixel coordinates corresponding to the stream location
    // /// <param name="offset">The stream location</param>
    // /// <returns>The pixel coordinates</returns>
    // public Point OffsetToPoint(long offset)
    // {
    //     offset /= sizeof(short);
    //     var x = (int)(offset % VirtualWidth);
    //     var y = (int)(offset / VirtualWidth);
    //
    //     return new Point(x, y);
    // }

    // public Image<Rgb24> Read(Rectangle area)
    // {
    //     ConstrainRectangle(ref area);
    //     return Image.Load<Rgb24>(_deviceStream, new Rgb565FramebufferDecoder(this, area));
    // }

    // public void SetPixel(int x, int y, Color color)
    // {
    //     _deviceStream.Seek(PointToOffset(x, y), SeekOrigin.Begin);
    //     var rgb = color.ToPixel<Rgb24>();
    //     _deviceStream.Write(BitConverter.GetBytes(Rgb565.Pack(rgb.R, rgb.G, rgb.B)), 0, 2);
    // }