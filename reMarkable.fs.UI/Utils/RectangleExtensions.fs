module reMarkable.fs.UI.Util.RectangleExtensions

open System
open SixLabors.Fonts
open SixLabors.ImageSharp

let ToRectangle(fr: FontRectangle): RectangleF =
    RectangleF(
        PointF(fr.Left, fr.Top),
        SizeF(fr.Width, fr.Height)
    )
    
let ToIntRect (rect: RectangleF): Rectangle =
    let x = (int)rect.X
    let y = (int)rect.Y
    let width: int = rect.Width |> float |> Math.Round |> int
    let height: int = rect.Height |> float |> Math.Round |> int
    Rectangle(x, y , width, height)
