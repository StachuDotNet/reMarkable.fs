namespace reMarkable.fs.Display

open SixLabors.ImageSharp

/// Provides an interface through which to access the device's framebuffer
type IFramebuffer =

    /// Constrains a rectangle to not overlap the visible bounds of the display
    /// <param name="area">The source location and area to constrain</param>
    abstract member ConstrainRectangle: byref<Rectangle> -> unit

//    /// Writes a rectangular region of pixels to the buffer
//    /// <typeparam name="TPixel">The pixel format</typeparam>
//    /// <param name="image">The pixels to write to the buffer</param>
//    /// <param name="srcArea">The source area of the image to draw</param>
//    /// <param name="destPoint">The point in the buffer where the source area will be drawn</param>
//    void Write<TPixel>(Image<TPixel> image, Rectangle srcArea, Point destPoint)
//        where TPixel : unmanaged, IPixel<TPixel>;

    
    
    
    // /// The virtual height of the buffer
    // int VirtualHeight { get; }

    // /// The virtual width of the buffer
    // int VirtualWidth { get; }

    // /// The visible height of the buffer, starting from the top left
    // int VisibleHeight { get; }

    // /// The visible width of the buffer, starting from the top left
    // int VisibleWidth { get; }
    //
    // /// Constrains a rectangle to not overlap the visible bounds of the display
    // /// <param name="srcArea">The source area to constrain</param>
    // /// <param name="destPoint">The point in the buffer where the source area should be placed</param>
    // void ConstrainRectangle(ref Rectangle srcArea, ref Point destPoint);

    // /// Reads a rectangular region of pixels from the buffer
    // /// <param name="area">The region of pixels to read</param>
    // /// <returns>The pixels as an <see cref="Image{Rgb24}" /></returns>
    // Image<Rgb24> Read(Rectangle area);

    // /// Sets exactly one pixel in the framebuffer to the specified color
    // /// <param name="x">The X coordinate of the pixel</param>
    // /// <param name="y">The Y coordinate of the pixel</param>
    // /// <param name="color">The color to set the pixel to</param>
    // void SetPixel(int x, int y, Color color);
