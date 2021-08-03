module reMarkable.fs.UI.Util.RectangleExtensions

open System
open SixLabors.Fonts
open SixLabors.ImageSharp

type HorizontalAlign = Left | Center | Right
type VerticalAlign = Top | Center | Bottom

type RectAlign =
    { Horizontal: HorizontalAlign option
      Vertical: VerticalAlign option }


let GetSmallestContaining(a: RectangleF, b: RectangleF): RectangleF =
    let x = Math.Min(a.Left, b.Left)
    let y = Math.Min(a.Top, b.Top)

    let width = Math.Max(a.Right, b.Right) - x
    let height = Math.Max(a.Bottom, b.Bottom) - y

    RectangleF(x, y, width, height)

let ToRectangle(fr: FontRectangle): RectangleF =
    RectangleF(
        PointF(fr.Left, fr.Top),
        SizeF(fr.Width, fr.Height)
    );

let GetContainingIntRect(rect: RectangleF): Rectangle =
    let x = (int)rect.X
    let y = (int)rect.Y
    let width: int = rect.Width |> float |> Math.Round |> int
    let height: int = rect.Height |> float |> Math.Round |> int
    Rectangle(x, y , width, height)

// public static RectangleF Inflated(this RectangleF rect, float width, float height)
// {
//     var rect2 = new RectangleF(rect.Location, rect.Size);
//     rect2.Inflate(width, height);
//     return rect2;
// }

let Align(src: RectangleF, dest: RectangleF, align: RectAlign): RectangleF =
    let mutable res = src
    
    let x =
        match align.Horizontal with
        | None -> res.Left
        | Some HorizontalAlign.Left -> dest.Left
        | Some HorizontalAlign.Center -> dest.Left + (dest.Width - res.Width) / (float32 2)
        | Some HorizontalAlign.Right -> dest.Right - res.Width

    let y =
        match align.Vertical with
        | None -> res.Top
        | Some VerticalAlign.Top -> dest.Top
        | Some VerticalAlign.Center -> dest.Top + (dest.Height - res.Height) / (float32 2)
        | Some VerticalAlign.Bottom -> dest.Bottom - res.Height

    res.Location <- PointF(x, y)
    res
