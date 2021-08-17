namespace reMarkable.fs.Display

open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats

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
    
    /// The virtual height of the buffer
    abstract member VirtualHeight: int

    /// The virtual width of the buffer
    abstract member VirtualWidth: int

    /// The visible height of the buffer, starting from the top left
    abstract member VisibleHeight: int

    /// The visible width of the buffer, starting from the top left
    abstract member VisibleWidth: int
            
    /// Finds the stream location corresponding to the pixel coordinates
    abstract member PointToOffset: x:int * y:int -> int

    /// Reads a rectangular region of pixels from the buffer
    /// <param name="area">The region of pixels to read</param>
    /// <returns>The pixels as an <see cref="Image{Rgb24}" /></returns>
    abstract member Read: Rectangle -> Image<Rgb24> 
    
    /// Writes a rectangular region of pixels to the buffer
    /// <typeparam name="TPixel">The pixel format</typeparam>
    /// <param name="image">The pixels to write to the buffer</param>
    /// <param name="srcArea">The source area of the image to draw</param>
    /// <param name="destPoint">The point in the buffer where the source area will be drawn</param>
    abstract member Write: Image<'TPixel> * srcArea:Rectangle * destPoint:Point * (IFramebuffer -> Rectangle -> Point -> IImageEncoder) -> unit