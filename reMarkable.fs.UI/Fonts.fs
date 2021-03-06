module reMarkable.fs.UI.Fonts

open System.IO
open System.Reflection
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing
open reMarkable.fs.ResourceHelpers
open reMarkable.fs.UI.Util
open reMarkable.fs.UI.Util.RectangleExtensions

let installFont(fonts: IFontCollection, font: byte[]) =
    use ms = new MemoryStream(font)
    fonts.Install ms |> ignore

let fonts = FontCollection()

let assembly = Assembly.GetExecutingAssembly()

installFont(fonts, getFileInBytes assembly "segoeui.ttf") // Segoe UI
//installFont(fonts, getFileInBytes assembly "segoeuib.ttf") // Segoe UI - Bold
//installFont(fonts, getFileInBytes assembly "segoeuil.ttf") // Segoe UI Light
//installFont(fonts, getFileInBytes assembly "segoeuisl.ttf") // Segoe UI Semilight
//installFont(fonts, getFileInBytes assembly "seguisb.ttf") // Segoe UI Semibold

let SegoeUi: FontFamily = fonts.Find("Segoe UI")
//let SegoeUiLight: FontFamily = fonts.Find("Segoe UI Light")
//let SegoeUiSemilight: FontFamily = fonts.Find("Segoe UI Semilight")
//let SegoeUiSemibold: FontFamily = fonts.Find("Segoe UI Semibold")

open SixLabors.ImageSharp.Drawing.Processing

let measureString(font, s: string): Rectangle =
    if s = null then
        Rectangle.Empty
    else
        TextMeasurer.Measure(s, RendererOptions(font))
        |> ToRectangle
        |> ToIntRect

let drawString(font, color: Color, buffer: Image<Rgb24>, s: string): unit =
    let strSize = measureString(font, s)

    let drawText = fun (g: IImageProcessingContext) -> g.DrawText(s, font, color, strSize.Location |> PointExtensions.toFloat) |> ignore
    buffer.Mutate(drawText)