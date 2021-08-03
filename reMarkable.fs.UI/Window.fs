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


    // public bool CanGoBack => _pages.Count > 0;

    // public void ConsumePress(FingerState fingerState)
    // {
    //     var eligibleControls = GetControlsAtPoint(fingerState.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnPress(fingerState);
    // }
    //
    // public void ConsumeRelease(FingerState fingerState)
    // {
    //     var eligibleControls = GetControlsAtPoint(fingerState.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnRelease(fingerState);
    // }
    //
    // public void ConsumeMove(FingerState fingerState)
    // {
    //     var eligibleControls = GetControlsAtPoint(fingerState.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnMove(fingerState);
    // }
    //
    // public void ConsumePress(StylusState stylus, DigitizerEventKeyCode code)
    // {
    //     var eligibleControls = GetControlsAtPoint(stylus.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnPress(stylus, code);
    // }
    //
    // public void ConsumeRelease(StylusState stylus, DigitizerEventKeyCode code)
    // {
    //     var eligibleControls = GetControlsAtPoint(stylus.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnRelease(stylus, code);
    // }
    //
    // public void ConsumeMove(StylusState stylus)
    // {
    //     var eligibleControls = GetControlsAtPoint(stylus.DevicePosition);
    //     foreach (var control in eligibleControls)
    //         control.OnMove(stylus);
    // }

    // private IEnumerable<Control> GetControlsAtPoint(PointF position)
    // {
    //     var page = GetCurrentPage();
    //
    //     if (page == null)
    //         return new List<Control>();
    //
    //     var children = GetChildControls(page.Content);
    //
    //     if (children.All(control => !control.Bounds.Contains(position)))
    //         return new List<Control>();
    //
    //     return children
    //         .Where(control => control.BoundsContains(position))
    //         .GroupBy(control => control.Layer)
    //         .OrderByDescending(controls => controls.Key)
    //         .First()
    //         .ToList();
    // }

    // private static List<Control> GetChildControls(Panel parent)
    // {
    //     var controls = new List<Control>();
    //
    //     foreach (var control in parent)
    //     {
    //         controls.Add(control);
    //         if (control is Panel p)
    //             controls.AddRange(GetChildControls(p));
    //     }
    //
    //     return controls;
    // }

    // public void ShowPreviousPage()
    // {
    //     _pages.Pop();
    //     Refresh(new Rectangle(0, 0, Width, Height));
    // }