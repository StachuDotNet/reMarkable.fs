namespace reMarkable.fs.Display

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

//type RefreshArgs =
//    { /// What should we follow up and refresh on the screen?
//      RefreshArea: Rectangle option
//      
//      /// The waveform mode to use when refreshing the screen.
//      /// If omitted, will use <see cref="WaveformMode.Auto" />
//      WaveformMode: WaveformMode option
//      
//      /// The display temperature to use to refresh the region
//      DisplayTemp: DisplayTemp option
//      
//      /// The update mode to use to refresh
//      UpdateMode: UpdateMode option }
    
type DrawArgs =
    { /// The image to write to the screen
      Image: Image<Rgb24>
      
      /// The source area of the image to draw
      SrcArea: Rectangle
      
      /// The point in the destination framebuffer where the source area will be drawn
      DestPoint: Point
      
      /// The area of the screen to refresh. If omitted, will refresh the entire affected area.
      RefreshArea: Rectangle option
      
      /// The waveform mode to use when refreshing the screen.
      /// If omitted, will use <see cref="WaveformMode.Auto" />
      WaveformMode: WaveformMode option
      
      /// The display temperature to use to refresh the region
      DisplayTemp: DisplayTemp option
      
      /// The update mode to use to refresh
      UpdateMode: UpdateMode option }

/// Provides an interface through which to access the device's display
type IDisplayDriver =
    /// The image data backing the display
    abstract member Framebuffer: IFramebuffer

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

    ///  Draws the contents of the internal buffer to the display device
    ///
    /// <param name="rectangle">The region of the buffer to be drawn to the device</param>
    /// <param name="mode">The update waveform used to refresh the region</param>
    /// <param name="displayTemp">The display temperature to use to refresh the region</param>
    /// <param name="updateMode">The update mode to use to refresh</param>
    abstract member Refresh:
        rectangle: Rectangle * mode: WaveformMode * displayTemp: DisplayTemp * updateMode: UpdateMode
         -> unit
