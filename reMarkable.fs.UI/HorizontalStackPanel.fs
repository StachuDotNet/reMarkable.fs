namespace reMarkable.fs.UI.Controls

open SixLabors.ImageSharp
open reMarkable.fs.UI

type HorizontalStackPanel() =
    inherit Panel()
    
    override this.Add(item: Control) =
        let totalWidth = this |> Seq.sumBy(fun control -> control.Bounds.Width)
        
        item.Location <- PointF(this.Bounds.X + totalWidth, this.Bounds.Y)

        base.Add(item)
    
    member private this.RecalculateChildPositions() =
        let mutable widthToAdd: float32 = 0f
        
        this
        |> Seq.iter(fun control ->
            control.Location <- PointF(this.Location.X + widthToAdd, this.Location.Y)
            widthToAdd <- widthToAdd + control.Size.Width
        )

    override this.Location
        with get () = base.Location
        and set (value) =
            base.Location <- value
            this.RecalculateChildPositions()