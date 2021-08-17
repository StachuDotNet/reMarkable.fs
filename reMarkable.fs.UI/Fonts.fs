module reMarkable.fs.UI.Fonts

open System.IO;
open System.Reflection
open reMarkable.fs.ResourceHelpers
open SixLabors.Fonts;

let installFont(fonts: IFontCollection, font: byte[]) =
    use ms = new MemoryStream(font)
    fonts.Install ms |> ignore

let fonts = FontCollection()

let assembly = Assembly.GetExecutingAssembly()

installFont(fonts, getFileInBytes assembly "segoeui.ttf"); // Segoe UI
installFont(fonts, getFileInBytes assembly "segoeuib.ttf"); // Segoe UI - Bold
installFont(fonts, getFileInBytes assembly "segoeuil.ttf"); // Segoe UI Light
installFont(fonts, getFileInBytes assembly "segoeuisl.ttf"); // Segoe UI Semilight
installFont(fonts, getFileInBytes assembly "seguisb.ttf"); // Segoe UI Semibold

let SegoeUi: FontFamily = fonts.Find("Segoe UI")
let SegoeUiLight: FontFamily = fonts.Find("Segoe UI Light")
let SegoeUiSemilight: FontFamily = fonts.Find("Segoe UI Semilight")
let SegoeUiSemibold: FontFamily = fonts.Find("Segoe UI Semibold")
