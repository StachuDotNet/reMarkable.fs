namespace reMarkable.fs.UI.Controls

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.UI.SegoeMdl2Glyphs
open reMarkable.fs.UI
open reMarkable.fs.UI.Util

// todo: ensure only 16, 32, or 64 (or learn how to _scale_ glyphs)
type IconLabel<'T when 'T : enum<int>>(atlas: SymbolAtlas<'T>, iconSize: int option, defaultIcon: 'T) as this =
    inherit Control()
    
    let mutable icon: 'T = defaultIcon
    let mutable iconSize: int = iconSize |> Option.defaultValue 32
    
    do
        this.Size <- SizeF(float32 iconSize, float32 iconSize)
    
    member this.Icon
        with get () = icon
        and set(value) = this.RedrawWithChange(fun () -> icon <- value)

    member this.IconSize
        with get() = iconSize
        and set(value) = this.RedrawWithChange(fun () -> iconSize <- value)

    override this.GetMinimumRedrawRect(): RectangleF =
        RectangleExtensions.Align(
            RectangleF(0f, 0f, float32 iconSize, float32 iconSize),
            this.Bounds,
            this.TextAlign
        )
        
    override this.Draw(buffer: Image<Rgb24>): unit =
        let rect = this.GetMinimumRedrawRect()
        let icon = atlas.GetIcon(this.IconSize, this.Icon)
        
        let draw (g: IImageProcessingContext) =
            let location = PointExtensions.toInteger(rect.Location)
            let opacity = 1f
            
            g.DrawImage(icon, location, opacity)
            |> ignore
            
        buffer.Mutate(draw)
