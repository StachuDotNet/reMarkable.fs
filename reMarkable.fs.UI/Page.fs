namespace reMarkable.fs.UI

open Microsoft.FSharp.Core
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

type Page(window: IWindow, width: int, height: int, content: IPanel)  =
    do
        content.Window <- Some window
        content.Bounds <- RectangleF(0f, 0f, width |> float32, height |> float32)
        content.LayoutControls()
    
    member _.Width = width
    member _.Height = height
    member _.Content = content

    member _.Window = window

    member this.Draw(buffer: Image<Rgb24>): unit =
        buffer.Mutate(fun g -> g.Clear(Color.White) |> ignore)
        this.Content.Draw(buffer)
