module reMarkable.fs.UI.Util.PointExtensions

open SixLabors.ImageSharp

let toInteger (point: PointF): Point =
    Point((int)point.X, (int)point.Y)

let toFloat (point: Point): PointF =
    PointF(point.X |> float32,  point.Y |> float32)