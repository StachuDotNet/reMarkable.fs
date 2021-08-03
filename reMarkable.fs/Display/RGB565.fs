namespace reMarkable.fs.Graphics

open System
open System.IO
open reMarkable.fs.Display
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats

/// Provides utilities for packing and unpacking <see cref="ushort" /> encoded RGB565 colors 
module Rgb565 =
    /// The blue component bits
    [<Literal>]
    let private MaxB = 0b11111us

    /// The green component bits
    let MaxG = 0b111111us
    
    /// The red component bits
    [<Literal>]
    let MaxR = 0b11111us
    
    /// The blue component bitmask
    [<Literal>]
    let MaskB = MaxB

    /// The green component bitmask
    let MaskG = MaxG <<< 6 //?

    /// The red component bitmask
    let MaskR = MaxR <<< 11 //?

    /// Packs individual components of a 24-bit RGB values into a 16-bit RGB565 <see cref="ushort" />
    /// <param name="r">The red component byte</param>
    /// <param name="g">The green component byte</param>
    /// <param name="b">The blue component byte</param>
    /// <returns>A RGB565-encoded <see cref="ushort" /></returns>
    let Pack(r: byte, g: byte, b: byte): uint16 = 
        let part1 = uint16((int16 r) <<< 8) &&& MaskR
        let part2 = uint16((int16 g) <<< 3) &&& (MaskG)
        let part3 = uint16((int16 b) >>> 3) &&& (MaskB)
        let uncasted = part1 ||| part2 ||| part3
        uint16 uncasted
        
    //
    // /// Unpacks a 16-bit RGB565 value into a 24-bit RGB value
    // /// <param name="components">The RGB565 encoded value</param>
    // /// <returns>A 24-bit <see cref="Rgb24" /></returns>
    // public static Rgb24 Unpack(ushort components)
    // {
    //     return new Rgb24((byte)(((components >> 11) & MaxR) << 3), (byte)(((components >> 5) & MaxG) << 2),
    //         (byte)((components & MaxB) << 3));
    // }




// /// Provides methods for decoding a RGB565 framebuffer stream to an <see cref="Image" />
// public class Rgb565FramebufferDecoder : IImageDecoder
// {
//     /// The rectangular area to read from the framebuffer
//     private readonly Rectangle _area;
//
//     /// The hardware framebuffer to read data from
//     private readonly HardwareFramebuffer _framebuffer;
//
//     /// Creates a new <see cref="Rgb565FramebufferDecoder" />
//     /// <param name="framebuffer">The hardware framebuffer to read data from</param>
//     /// <param name="area">The rectangular area to read from the framebuffer</param>
//     public Rgb565FramebufferDecoder(HardwareFramebuffer framebuffer, Rectangle area)
//     {
//         _framebuffer = framebuffer;
//         _area = area;
//     }
//
//     /// <inheritdoc />
//     public Image<TPixel> Decode<TPixel>(Configuration configuration, Stream stream)
//         where TPixel : unmanaged, IPixel<TPixel>
//     {
//         var image = new Image<TPixel>(configuration, _area.Width, _area.Height);
//         return DecodeIntoImage(stream, image);
//     }
//
//     /// <inheritdoc />
//     public Image Decode(Configuration configuration, Stream stream)
//     {
//         var image = new Image<Rgb24>(configuration, _area.Width, _area.Height);
//         return DecodeIntoImage(stream, image);
//     }
//
//     /// <inheritdoc />
//     public Task<Image<TPixel>> DecodeAsync<TPixel>(Configuration configuration, Stream stream)
//         where TPixel : unmanaged, IPixel<TPixel>
//     {
//         throw new NotImplementedException();
//     }
//
//     /// <inheritdoc />
//     public Task<Image> DecodeAsync(Configuration configuration, Stream stream)
//     {
//         throw new NotImplementedException();
//     }
//
//     /// Decodes the given RGB565 stream into the provided image
//     /// <typeparam name="TPixel">The pixel format</typeparam>
//     /// <param name="stream">The RGB565 stream to read <see cref="ushort" />-encoded image data from</param>
//     /// <param name="image">The image to read data into</param>
//     /// <returns>The image passed as the <paramref name="image" /> argument</returns>
//     private Image<TPixel> DecodeIntoImage<TPixel>(Stream stream, Image<TPixel> image)
//         where TPixel : unmanaged, IPixel<TPixel>
//     {
//         var buf = new byte[_area.Width * sizeof(short)];
//         var rgb565Buf = new ushort[_area.Width];
//
//         for (var y = 0; y < _area.Height; y++)
//         {
//             stream.Seek(_framebuffer.PointToOffset(_area.X, _area.Y + y), SeekOrigin.Begin);
//             stream.Read(buf, 0, buf.Length);
//             Buffer.BlockCopy(buf, 0, rgb565Buf, 0, buf.Length);
//
//             var span = image.GetPixelRowSpan(y);
//
//             for (var x = 0; x < _area.Width; x++) span[x].FromRgb24(Rgb565.Unpack(rgb565Buf[x]));
//         }
//
//         return image;
//     }
// }

/// Provides methods for encoding an <see cref="Image" /> to a RGB565 framebuffer stream
/// 
/// <param name="framebuffer">The hardware framebuffer to write data to</param>
/// <param name="srcArea">The area of the source image to encode</param>
/// <param name="destPoint">The location to place the top-leftmost corner of the source area on the destination framebuffer</param>
type Rgb565FramebufferEncoder(framebuffer: HardwareFramebuffer, srcArea: Rectangle, destPoint: Point)=
    interface IImageEncoder with
        member _.EncodeAsync<'TPixel when 'TPixel :> IPixel<'TPixel>>(_image: Image<'TPixel>, _stream: Stream)  =
            raise <| NotImplementedException()

        member _.Encode<'TPixel when 'TPixel :> IPixel<'TPixel>>(image: Image<'TPixel>, stream: Stream): unit =
            let buf: byte array = Array.zeroCreate (srcArea.Width * 2)
            
            let rgb565Buf: uint16 array = Array.zeroCreate srcArea.Width
            
            let mutable rgba32 = Rgba32()
            
            for y = 0 to srcArea.Height - 1 do
                let span = image.GetPixelRowSpan(y)

                for x = 0 to srcArea.Width - 1 do
                    span.[x].ToRgba32(&rgba32)
                    rgb565Buf.[x] <- Rgb565.Pack(rgba32.R, rgba32.G, rgba32.B)

                stream.Seek(framebuffer.PointToOffset(destPoint.X, destPoint.Y + y) |> int64, SeekOrigin.Begin)
                    |> ignore

                Buffer.BlockCopy(rgb565Buf, 0, buf, 0, buf.Length)
                stream.Write(buf, 0, buf.Length)
