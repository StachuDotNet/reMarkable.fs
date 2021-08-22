namespace reMarkable.fs.Display

open System.Runtime.InteropServices
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open reMarkable.fs.Display.Framebuffer

// public enum AutoUpdateMode : uint
// {
//     Region = 0,
//     Automatic = 1
// }
    
// public struct UpdateMarkerData
// {
//     public uint UpdateMarker;
//     public uint CollisionTest;
// }
    
type UpdateMode =
    | Partial = 0u
    | Full = 1u
    
type UpdateScheme =
    | Snapshot = 0
    | Queue = 1
    | QueueAndMerge = 2

type WaveformMode = 
    /// Screen goes to white (clears)
    | Init = 0x0u
    
    /// Basically A2 (so partial refresh shouldn't be possible here)
    | Glr16 = 0x4u
    
    /// Official -- and enables Regal D Processing
    | Gld16 = 0x5u
    
    /// [Direct Update] Grey->white/grey->black  -- remarkable uses this for drawing
    | Du = 0x1u

    /// High fidelity (flashing)
    | Gc16 = 0x2u

    /// Medium fidelity  -- remarkable uses this for UI
    | Gc16Fast = 0x3u
    
    /// Medium fidelity from white transition
    | Gl16Fast = 0x6u
    
    /// Medium fidelity 4 level of gray direct update
    | Du4 = 0x7u
    
    /// Ghost compensation waveform
    | Reagl = 0x8u
    
    /// Ghost compensation waveform with dithering
    | Reagld = 0x9u
    
    /// 2-bit from white transition
    | Gl4 = 0xAu
    
    /// High fidelity for black transition
    | Gl16Inv = 0xBu

    /// Official
    | Auto = 257u
    
// [StructLayout(LayoutKind.Sequential)]
// public struct WaveformModes
// {
//     public int ModeInit;
//     public int ModeDu;
//     public int ModeGc4;
//     public int ModeGc8;
//     public int ModeGc16;
//     public int ModeGc32;
// }

type DisplayTemp =
    | Ambient = 0x1000us
    | Papyrus = 0x1001us
    | RemarkableDraw = 0x0018us
    | Max = 0xFFFFus

[<StructLayout(LayoutKind.Sequential)>]
type FrameBufferRectangle =
  struct
    val mutable Y: uint
    val mutable X: uint
    val mutable Width: uint
    val mutable Height: uint
  end
  
[<StructLayout(LayoutKind.Sequential)>]
type FrameBufferAltBufferData =
  struct
    val mutable PhysicalAddress: uint
    val mutable Width: uint
    val mutable Height: uint
    val mutable AltUpdateRegion: FrameBufferRectangle
  end

[<StructLayout(LayoutKind.Sequential)>]
type FrameBufferUpdateData =
  struct
    val mutable UpdateRegion: FrameBufferRectangle
    val mutable WaveformMode: WaveformMode
    val mutable UpdateMode: UpdateMode
    val mutable UpdateMarker: uint
    val mutable DisplayTemp: DisplayTemp
    val mutable Flags: uint
    val mutable DitherMode: int
    val mutable QuantBit: int
    val mutable AltData: FrameBufferAltBufferData
  end

type DrawAndRefreshArgs =
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
    abstract member Height: int

    /// The horizontal resolution of the backing framebuffer
    abstract member Width: int

    /// Draws image data to the screen and refreshes a portion of it
    abstract member DrawAndRefresh: DrawAndRefreshArgs -> unit

    ///  Draws the contents of the internal buffer to the display device
    ///
    /// <param name="rectangle">The region of the buffer to be drawn to the device</param>
    /// <param name="mode">The update waveform used to refresh the region</param>
    /// <param name="displayTemp">The display temperature to use to refresh the region</param>
    /// <param name="updateMode">The update mode to use to refresh</param>
    abstract member Refresh:
        rectangle: Rectangle * mode: WaveformMode * displayTemp: DisplayTemp * updateMode: UpdateMode
         -> unit
