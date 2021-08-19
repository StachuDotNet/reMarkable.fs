module reMarkable.fs.UI.IconLabel

open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.UI.SegoeMdl2Glyphs
        
let drawIcon<'T when 'T : enum<int>> (atlas: Lazy<SymbolAtlas<'T>>) (location: Point) (buffer: Image<Rgb24>) (icon: 'T) iconSize: unit =
    let icon = atlas.Force().GetIcon(iconSize, icon)
    
    let draw (g: IImageProcessingContext) =
        let opacity = 1f
        
        g.DrawImage(icon, location, opacity)
        |> ignore
        
    buffer.Mutate(draw)
