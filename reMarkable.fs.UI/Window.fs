namespace reMarkable.fs.UI

open System.Collections.Generic
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.UI.Util

type WindowUpdateEventArgs(buffer: Image<Rgb24>, updatedArea: Rectangle) =
    member _.Buffer: Image<Rgb24> = buffer
    member _.UpdatedArea: Rectangle = updatedArea

type Window(width: int, height: int) =
    let pages = Stack<Page>();
    let buffer = new Image<Rgb24>(width, height)
    
    let update = Event<WindowUpdateEventArgs>()
    
    do
        buffer.Mutate(fun g -> g.Clear(Color.White) |> ignore)

    member _.Buffer = buffer
    member _.Width = width
    member _.Height = height
    
    member _.Update = update.Publish

    member _.GetCurrentPage() =
        if pages.Count > 0 then
            Some <| pages.Peek()
        else
            None
    
    interface IWindow with 
        member this.Refresh(rectangle: RectangleF): unit =
            let currentPage = this.GetCurrentPage()

            match currentPage with
            | None -> ()
            | Some page -> 
                page.Draw(this.Buffer)
                update.Trigger(WindowUpdateEventArgs(this.Buffer, RectangleExtensions.GetContainingIntRect(rectangle)))

    member this.CreatePage<'T when 'T :> IPanel and 'T : (new: unit -> 'T)>(): Page =
         Page(this :> IWindow, this.Width, this.Height, new 'T())
         
    member this.ShowPage(page: Page) =
        pages.Push(page)
        (this :> IWindow).Refresh(RectangleF(0f, 0f, width |> float32, height |> float32))