namespace reMarkable.fs.Unix.Driver.Display.Framebuffer

open System

[<Flags>]
type FbSync = // : uint
    | None = 0u
    | HorizontalHighActive = 1u
    | VerticalHighActive = 2u
    | External = 4u
    | CompositeHighActive = 8u
    | BroadcastTimings = 16u


//public struct FbFixedScreenInfo
//    // /// identification string eg "TT Builtin"
//    // public unsafe fixed char Id[16];
//    //
//    // /// Start of frame buffer mem (physical address)
//    // public ulong SmemStart;
//    //
//    // /// Length of frame buffer mem
//    // public uint SmemLen;
//    //
//    // /// see FB_TYPE_*
//    // public uint Type;
//    //
//    // /// Interleave for interleaved Planes
//    // public uint TypeAux;
//    //
//    // /// see FB_VISUAL_*
//    // public uint Visual;
//    //
//    // /// zero if no hardware panning
//    // public ushort XPanStep;
//    //
//    // /// zero if no hardware panning
//    // public ushort YPanStep;
//    //
//    // /// zero if no hardware ywrap
//    // public ushort YWrapStep;
//    //
//    // /// length of a line in bytes
//    // public uint LineLength;
//    //
//    // /// Start of Memory Mapped I/O (physical address)
//    // public ulong MmioStart;
//    //
//    // /// Length of Memory Mapped I/O
//    // public uint MmioLen;
//    //
//    // /// Indicate to driver which specific chip/card we have
//    // public uint Accel;
//    //
//    // /// see FB_CAP_*
//    // public ushort Capabilities;
//    //
//    // /// Reserved for future compatibility
//    // public ushort Reserved0;
//    //
//    // /// Reserved for future compatibility
//    // public ushort Reserved1;

[<Flags>]
type FbVMode = // : uint
    | NonInterlaced = 0u
    | Interlaced = 1u
    | Double = 2u
    | OddFieldFirst = 4u
    | YWrap = 256u
    | SmoothXPan = 512u
    
type FbBitfield =
    struct // some of the below may need to be made mutable?
        /// beginning of bitfield
        val Offset: uint

        /// length of bitfield
        val Length: uint

        /// != 0 : Most significant bit is right
        val IsMsbRight: uint
    end

type FbVarScreenInfo =
    struct
        val mutable VisibleResolutionX: uint
        val mutable VisibleResolutionY: uint

        val VirtualResolutionX: uint
        val VirtualResolutionY: uint

        val VisibleOffsetX: uint;
        
        val VisibleOffsetY: uint;
        
        val BitsPerPixel: uint;
        
        ///  0 = color, 1 = grayscale
        val IsGrayscale: uint
        
        val Red: FbBitfield;
        
        val Green: FbBitfield;
        
        val Blue: FbBitfield;
        
        val Transparency: FbBitfield;
        
        /// != 0 Non standard pixel format
        val NonStandardPixelFormat: uint
        
        /// see FB_ACTIVATE_*
        val Activate: uint
        
        /// height of picture in mm
        val Height: uint
        
        /// width of picture in mm
        val Width: uint
        
        /// (OBSOLETE) see fb_info.flags
        val AccelFlags: uint
        
        /// pixel clock in ps (picoseconds)
        val PixClock: uint
        
        /// time from sync to picture in pixclocks
        val LeftMargin: uint
        
        /// time from picture to sync in pixclocks
        val RightMargin: uint
        
        /// time from sync to picture in pixclocks
        val UpperMargin: uint
        
        /// time from picture to sync in pixclocks
        val LowerMargin: uint
        
        /// length of horizontal sync in pixclocks
        val HSyncLen: uint
        
        /// length of vertical sync in pixclocks
        val VSyncLen: uint
        
        /// see FB_SYNC_*
        val Sync: FbSync
        
        /// ee FB_VMODE_*
        val VMode: FbVMode
        
        /// angle we rotate counter clockwise
        val Rotate: uint
        
        /// colorspace for FOURCC-based modes
        val Colorspace: uint
        
        /// Reserved for future compatibility
        val Reserved0: uint
        
        /// Reserved for future compatibility
        val Reserved1: uint
        
        /// Reserved for future compatibility
        val Reserved2: uint
        
        /// Reserved for future compatibility
        val Reserved3: uint
    end
    
   