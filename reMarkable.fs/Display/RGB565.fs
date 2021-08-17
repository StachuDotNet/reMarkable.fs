/// Provides utilities for packing and unpacking <see cref="ushort" /> encoded RGB565 colors 
module reMarkable.fs.Display.Rgb565

open System
open System.IO
open System.Threading.Tasks
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats

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
let Pack(redComponent: byte, g: byte, b: byte): uint16 = 
    let redPart = uint16((int16 redComponent) <<< 8) &&& MaskR
    let greenPart = uint16((int16 g) <<< 3) &&& (MaskG)
    let bluePart = uint16((int16 b) >>> 3) &&& (MaskB)
    
    redPart ||| greenPart ||| bluePart
    |> uint16
    
/// Unpacks a 16-bit RGB565 value into a 24-bit RGB value
let Unpack(rgbEncodedValue: uint16): Rgb24 =
    let r = (byte)(((rgbEncodedValue >>> 11) &&& MaxR) <<< 3)
    let g = (byte)(((rgbEncodedValue >>> 5) &&& MaxG) <<< 2)
    let b = (byte)((rgbEncodedValue &&& MaxB) <<< 3)

    Rgb24(r, g, b)
    

/// Provides methods for encoding an <see cref="Image" /> to a RGB565 framebuffer stream
/// 
/// <param name="framebuffer">The hardware framebuffer to write data to</param>
/// <param name="srcArea">The area of the source image to encode</param>
/// <param name="destPoint">The location to place the top-leftmost corner of the source area on the destination framebuffer</param>
type Rgb565FramebufferEncoder(framebuffer: IFramebuffer, srcArea: Rectangle, destPoint: Point)=
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
                    rgb565Buf.[x] <- Pack(rgba32.R, rgba32.G, rgba32.B)

                stream.Seek(framebuffer.PointToOffset(destPoint.X, destPoint.Y + y) |> int64, SeekOrigin.Begin)
                    |> ignore

                Buffer.BlockCopy(rgb565Buf, 0, buf, 0, buf.Length)
                stream.Write(buf, 0, buf.Length)


/// Provides methods for decoding a RGB565 framebuffer stream to an <see cref="Image" />
type Rgb565FramebufferDecoder(framebuffer: IFramebuffer , areaToReadFrom: Rectangle) =
     /// Decodes the given RGB565 stream into the provided image
     /// 
     /// <typeparam name="TPixel">The pixel format</typeparam>
     /// <param name="stream">The RGB565 stream to read <see cref="ushort" />-encoded image data from</param>
     /// <param name="image">The image to read data into</param>
     /// <returns>The image passed as the <paramref name="image" /> argument</returns>
     let decodeIntoImage(stream: Stream, image: Image<'TPixel>): Image<'TPixel> =        
         let buf: byte array = Array.zeroCreate (areaToReadFrom.Width * 2) // sizeof(short) -> 2
         let rgb565Buf: uint16 array  = Array.zeroCreate areaToReadFrom.Width

         for y in 0 .. areaToReadFrom.Height - 1 do
             stream.Seek(framebuffer.PointToOffset(areaToReadFrom.X, areaToReadFrom.Y + y) |> int64, SeekOrigin.Begin)
                |> ignore
             stream.Read(buf, 0, buf.Length)
                |> ignore
                
             Buffer.BlockCopy(buf, 0, rgb565Buf, 0, buf.Length)

             let span = image.GetPixelRowSpan(y)

             for x in 0 .. areaToReadFrom.Width - 1 do
                 span.[x].FromRgb24(Unpack(rgb565Buf.[x]))

         image

     interface IImageDecoder with
         member _.Decode<'TPixel when 'TPixel :> IPixel<'TPixel>>(configuration: Configuration, stream: Stream): Image<'TPixel> =
             let image = new Image<'TPixel>(configuration, areaToReadFrom.Width, areaToReadFrom.Height)
             decodeIntoImage(stream, image)

         member this.Decode(configuration: Configuration, stream: Stream): Image =
             let image = new Image<Rgb24>(configuration, areaToReadFrom.Width, areaToReadFrom.Height)
             decodeIntoImage(stream, image) :> Image

         member _.DecodeAsync<'TPixel when 'TPixel :> IPixel<'TPixel>>(_configuration: Configuration, _stream: Stream): Task<Image<'TPixel>> =
             raise <| NotImplementedException()

         member this.DecodeAsync(_configuration: Configuration, _stream: Stream): Task<Image> =
             raise <| NotImplementedException()