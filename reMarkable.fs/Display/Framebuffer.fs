namespace reMarkable.fs.Unix.Driver.Display.Framebuffer

// [Flags]
// public enum FbSync : uint
// {
//     None = 0,
//     HorizontalHighActive = 1,
//     VerticalHighActive = 2,
//     External = 4,
//     CompositeHighActive = 8,
//     BroadcastTimings = 16
// }


//public struct FbFixedScreenInfo
//{
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
//}

// [Flags]
// public enum FbVMode : uint
// {
//     NonInterlaced = 0,
//     Interlaced = 1,
//     Double = 2,
//     OddFieldFirst = 4,
//     YWrap = 256,
//     SmoothXPan = 512
    // }
    
//        public struct FbBitfield
//    {
//        /// beginning of bitfield
//        public uint Offset;
//
//        /// length of bitfield
//        public uint Length;
//
//        /// != 0 : Most significant bit is right
//        public uint IsMsbRight;
//    }

type FbVarScreenInfo =
    struct
        val mutable VisibleResolutionX: uint32
        val mutable VisibleResolutionY: uint32

        val VirtualResolutionX: uint32
        val VirtualResolutionY: uint32

        // public uint VisibleOffsetX;
        //
        // public uint VisibleOffsetY;
        //
        // public uint BitsPerPixel;
        //
        // ///  0 = color, 1 = grayscale
        // public uint IsGrayscale;
        //
        // public FbBitfield Red;
        //
        // public FbBitfield Green;
        //
        // public FbBitfield Blue;
        //
        // public FbBitfield Transparency;
        //
        // /// != 0 Non standard pixel format
        // public uint NonStandardPixelFormat;
        //
        // /// see FB_ACTIVATE_*
        // public uint Activate;
        //
        // /// height of picture in mm
        // public uint Height;
        //
        // /// width of picture in mm
        // public uint Width;
        //
        // /// (OBSOLETE) see fb_info.flags
        // public uint AccelFlags;
        //
        // /// pixel clock in ps (picoseconds)
        // public uint PixClock;
        //
        // /// time from sync to picture in pixclocks
        // public uint LeftMargin;
        //
        // /// time from picture to sync in pixclocks
        // public uint RightMargin;
        //
        // /// time from sync to picture in pixclocks
        // public uint UpperMargin;
        //
        // /// time from picture to sync in pixclocks
        // public uint LowerMargin;
        //
        // /// length of horizontal sync in pixclocks
        // public uint HSyncLen;
        //
        // /// length of vertical sync in pixclocks
        // public uint VSyncLen;
        //
        // /// see FB_SYNC_*
        // public FbSync Sync;
        //
        // /// ee FB_VMODE_*
        // public FbVMode VMode;
        //
        // /// angle we rotate counter clockwise
        // public uint Rotate;
        //
        // /// colorspace for FOURCC-based modes
        // public uint Colorspace;
        //
        // /// Reserved for future compatibility
        // public uint Reserved0;
        //
        // /// Reserved for future compatibility
        // public uint Reserved1;
        //
        // /// Reserved for future compatibility
        // public uint Reserved2;
        //
        // /// Reserved for future compatibility
        // public uint Reserved3;
    end
    
   