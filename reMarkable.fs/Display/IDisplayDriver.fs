namespace reMarkable.fs.Display

open SixLabors.ImageSharp;
open SixLabors.ImageSharp.PixelFormats;

//    /// <param name="image">The image to write to the screen</param>
//    /// <param name="srcArea">The source area of the image to draw</param>
//    /// <param name="destPoint">The point in the destination framebuffer where the source area will be drawn</param>
//    /// <param name="refreshArea">The area of the screen to refresh. If omitted, will refresh the entire affected area.</param>
//    /// <param name="waveformMode">
//    ///  The waveform mode to use when refreshing the screen.
//    ///  If omitted, will use <see cref="WaveformMode.Auto" />
//    /// </param>
//    /// <param name="displayTemp">The display temperature to use to refresh the region</param>
//    /// <param name="updateMode">The update mode to use to refresh</param>
type DrawArgs =
    { Image: Image<Rgb24>
      SrcArea: Rectangle
      DestPoint: Point
      RefreshArea: Rectangle option
      WaveformMode: WaveformMode option
      DisplayTemp: DisplayTemp option
      UpdateMode: UpdateMode option }

/// Provides an interface through which to access the device's display
type IDisplayDriver =
    // /// The image data backing the display
    // IFramebuffer Framebuffer { get; }

    /// The vertical resolution of the backing framebuffer
    abstract member VirtualHeight: int

    /// The horizontal resolution of the backing framebuffer
    abstract member VirtualWidth: int

    /// The vertical resolution of the physically visible portion of the screen
    abstract member VisibleHeight: int

    /// The horizontal resolution of the physically visible portion of the screen
    abstract member VisibleWidth: int

    /// Draws image data to the screen and refreshes a portion of it
    abstract member Draw: DrawArgs -> unit

    // /// <summary>
    // ///     Draws the contents of the internal buffer to the display device
    // ///
    // /// <param name="rectangle">The region of the buffer to be drawn to the device</param>
    // /// <param name="mode">The update waveform used to refresh the region</param>
    // /// <param name="displayTemp">The display temperature to use to refresh the region</param>
    // /// <param name="updateMode">The update mode to use to refresh</param>
    // void Refresh(Rectangle rectangle, WaveformMode mode, DisplayTemp displayTemp, UpdateMode updateMode);
