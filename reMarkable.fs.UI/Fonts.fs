module reMarkable.fs.UI.Fonts

open System
open System.IO;
open System.Reflection
open reMarkable.fs.ResourceHelpers
open SixLabors.Fonts;

let startOfProgram = DateTimeOffset.Now
let printDuration label =
    (DateTimeOffset.Now - startOfProgram).TotalMilliseconds
    |> printfn "%s: %f" label
    
        
let installFont(fonts: IFontCollection, font: byte[]) =
    use ms = new MemoryStream(font)
    fonts.Install ms |> ignore

let fonts = FontCollection()

let assembly = Assembly.GetExecutingAssembly()

installFont(fonts, getFileInBytes assembly "segoeui.ttf"); // Segoe UI
printDuration "A"
installFont(fonts, getFileInBytes assembly "segoeuib.ttf"); // Segoe UI - Bold
printDuration "B"
installFont(fonts, getFileInBytes assembly "segoeuil.ttf"); // Segoe UI Light
printDuration "C"
installFont(fonts, getFileInBytes assembly "segoeuisl.ttf"); // Segoe UI Semilight
printDuration "D"
installFont(fonts, getFileInBytes assembly "seguisb.ttf"); // Segoe UI Semibold
printDuration "E"

let SegoeUi: FontFamily = fonts.Find("Segoe UI")
let SegoeUiLight: FontFamily = fonts.Find("Segoe UI Light")
let SegoeUiSemilight: FontFamily = fonts.Find("Segoe UI Semilight")
let SegoeUiSemibold: FontFamily = fonts.Find("Segoe UI Semibold")
