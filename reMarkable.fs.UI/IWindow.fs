namespace reMarkable.fs.UI

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats

type IWindow =
    abstract member Refresh: RectangleF -> unit

type IPanel =
    abstract member Window: IWindow option with get, set
    abstract member Bounds: RectangleF with get, set
    abstract member Draw: Image<Rgb24> -> unit
    abstract member LayoutControls: unit -> unit