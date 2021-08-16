namespace reMarkable.fs.UI

open System
open reMarkable.fs.UI
open reMarkable.fs.UI.Util
open reMarkable.fs.UI.Util.RectangleExtensions
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing

[<AbstractClass>]
type Control() =
    let mutable bounds: RectangleF = RectangleF.Empty
    let mutable font = Fonts.SegoeUiSemibold.CreateFont(32f)
    let mutable backgroundColor = Color.Transparent
    let mutable foregroundColor = Color.Black
    let mutable text = String.Empty
    let mutable textAlign = { Horizontal = None; Vertical = None }
    
    let mutable parent: Control option = None
    let mutable window: IWindow option = None

    /// Redraws the control with the smallest rectangle that
    /// encompasses the visual change provided by the lambda
    /// including removing artifacts from the previous value
    /// <param name="action">The visual change to update</param>
    member this.RedrawWithChange(action: Action): unit =
        let oldMinimumRect = this.GetMinimumRedrawRect()
        action.Invoke()
        let newMinimumRect = this.GetMinimumRedrawRect()

        let redrawRect = RectangleExtensions.GetSmallestContaining(oldMinimumRect, newMinimumRect)
        
        match window with
        | Some w -> w.Refresh(redrawRect)
        | _ -> ()

    member _.Parent
        with set(value) = parent <- value
        and get () = parent
        
    member _.Window
        with set(value) = window <- value
        and get () = window

    member this.Bounds
        with set(value) = this.RedrawWithChange(fun () -> bounds <- value)
        and get() = bounds

    abstract member Location: PointF with get, set
    default this.Location
        with set(value) = this.RedrawWithChange(fun () -> bounds.Location <- value)
        and get () = bounds.Location

    member this.Size
        with set(value) = this.RedrawWithChange(fun () -> bounds.Size <- value)
        and get () = bounds.Size

    member val Layer: int = 0 with get, set
    
    member this.ForegroundColor
        with get() = foregroundColor
        and set(value) = this.RedrawWithChange(fun () -> foregroundColor <- value)
        
    member this.BackgroundColor
        with get() = backgroundColor
        and set(value) = this.RedrawWithChange(fun () -> backgroundColor <- value)

    member this.Font
        with get() = font
        and set(value) = this.RedrawWithChange(fun () -> font <- value)
        
    member this.Text
        with get() = text
        and set(value) = this.RedrawWithChange(fun () -> text <- value)

    member this.TextAlign
        with get () = textAlign
        and set(value) = this.RedrawWithChange(fun () -> textAlign <- value)

    abstract member Draw: Image<Rgb24> -> unit


    member this.MeasureString(s: string, layoutRectangle: RectangleF, align: RectAlign): RectangleF =
        if s = null then
            RectangleF.Empty
        else
            let a = ToRectangle(TextMeasurer.Measure(s, RendererOptions(this.Font)))
            let b = Align(a, layoutRectangle, align )

            let rectF = GetContainingIntRect(b)
            
            RectangleF(rectF.X |> float32, rectF.Y |> float32, rectF.Width |> float32, rectF.Height |> float32)


    member this.DrawString(buffer: Image<Rgb24>, s: string, layoutRectangle: RectangleF, align: RectAlign): unit =
        if s = null then
            ()
        else
            let strSize = this.MeasureString(s, layoutRectangle, align)

            let drawText = fun (g:IImageProcessingContext) -> g.DrawText(s, this.Font, this.ForegroundColor, strSize.Location) |> ignore
            buffer.Mutate(drawText)


    abstract member GetMinimumRedrawRect: unit -> RectangleF
    default _.GetMinimumRedrawRect(): RectangleF = bounds

    // public event EventHandler<FingerState> FingerPress;
    // public event EventHandler<FingerState> FingerRelease;
    // public event EventHandler<FingerState> FingerMove;

    // public event EventHandler<StylusPressEventArgs> StylusPress;
    // public event EventHandler<StylusPressEventArgs> StylusRelease;
    // public event EventHandler<StylusState> StylusMove;

    // protected void DrawStringWithIcon(Image<Rgb24> buffer, char icon, float iconPadding, string s, RectangleF layoutRectangle)
    //     // TODO. (note, this was commented when you got it, Stachu!)
    //     //var iconSize = icon == 0 ? RectangleF.Empty : TextMeasurer.Measure(icon.ToString()).ToRectangle();
    //     //var strSize = TextMeasurer.Measure(s, rendererOptions).ToRectangle();
    //     //
    //     //if (s != null && !s.Contains('\n'))
    //     //    strSize.Height = Font.Size;
    //     //
    //     //iconSize.Width += iconPadding;
    //     //
    //     //iconSize.Align(layoutRectangle, RectAlign.Middle);
    //     //strSize.Align(layoutRectangle, RectAlign.Middle);
    //     //
    //     //var combinedLeft = layoutRectangle.Left + (layoutRectangle.Width - (iconSize.Width + strSize.Width)) / 2;
    //     //
    //     //iconSize.Location = new PointF(combinedLeft, iconSize.Top);
    //     //strSize.Location = new PointF(combinedLeft + iconSize.Width, strSize.Top);
    //     //
    //     //buffer.Mutate(g =>
    //     //{
    //     //    g.DrawText(textGraphicsOptions, s, Font, ForegroundColor, strSize.GetContainingIntRect().Location);
    //     //
    //     //    if (icon != 0)
    //     //        g.DrawText(textGraphicsOptions, icon.ToString(), Font, ForegroundColor, iconSize.GetContainingIntRect().Location);
    //     //});

    // protected void DrawBounds(IImageProcessingContext g)
    //     g.Draw(ForegroundColor, 1, Bounds);

    // public virtual bool BoundsContains(PointF point)
    //     return Bounds.Contains(point);
    //
    // internal virtual void OnPress(FingerState fingerState)
    //     FingerPress?.Invoke(this, fingerState);
    //
    // internal virtual void OnRelease(FingerState fingerState)
    //     FingerRelease?.Invoke(this, fingerState);
    //
    // internal virtual void OnMove(FingerState fingerState)
    //     FingerMove?.Invoke(this, fingerState);
    //
    // internal virtual void OnPress(StylusState stylus, DigitizerEventKeyCode code)
    //     StylusPress?.Invoke(this, new StylusPressEventArgs(stylus, code));
    //
    // internal virtual void OnRelease(StylusState stylus, DigitizerEventKeyCode code)
    //     StylusRelease?.Invoke(this, new StylusPressEventArgs(stylus, code));
    //
    // internal virtual void OnMove(StylusState stylus)
    //     StylusMove?.Invoke(this, stylus);
