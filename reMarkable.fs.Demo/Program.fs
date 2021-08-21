open System
open System.IO
open System.Reflection
open System.Threading
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open SixLabors.Fonts
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.Processing
open reMarkable.fs.UI
open reMarkable.fs.Digitizer
open reMarkable.fs
open reMarkable.fs.Display
open reMarkable.fs.UI.Util

let displayDriver = ReMarkable.Display

let reportOnPerformance() =
    printfn "NumberOfCores: %A" ReMarkable.Performance.NumberOfCores
    printfn "NumberOfProcessors: %A" ReMarkable.Performance.NumberOfProcessors
    printfn "TotalMemory: %A" ReMarkable.Performance.TotalMemory
    printfn "TotalSwap: %A" ReMarkable.Performance.TotalSwap
    printfn "GetFreeMemory: %A" (ReMarkable.Performance.GetFreeMemory())
    printfn "GetFreeSwap: %A" (ReMarkable.Performance.GetFreeSwap())
    printfn "GetNetworkAdapters: %A" (ReMarkable.Performance.GetNetworkAdapters())
    printfn "GetProcessorTime: %A" (ReMarkable.Performance.GetProcessorTime())
    
let drawBeans() =
    let img: Image<Rgb24> = Image.Load(Path.Combine(Path.GetDirectoryName (Assembly.GetExecutingAssembly().Location), "blackPepper.jpeg"))
    
    for x in 0 .. 2 .. 13 do
        for y in 0 .. 2 .. 17 do
            let destX = x * 100
            let destY = y * 100
            
            ReMarkable.Display.DrawAndRefresh
                { Image = img
                  SrcArea = Rectangle(0, 0, 144, 144)
                  DestPoint = Point(destX, destY)
                  
                  RefreshArea = Rectangle(destX, destY, 144, 144) |> Some
                  WaveformMode = None
                  DisplayTemp = None
                  UpdateMode = Some UpdateMode.Partial }

let drawGiantRectangle() =
    let x = 10
    let y = x
    let width = ReMarkable.Display.Width - (x * 2)
    let height = ReMarkable.Display.Height - (y * 2)
    
    let buffer = new Image<Rgb24>(width, height)
    
    ReMarkable.Display.DrawAndRefresh
        { Image = buffer
          SrcArea = Rectangle(0, 0, width, height)
          DestPoint = Point(x, y)
          
          RefreshArea = None
          WaveformMode = None
          DisplayTemp = None
          UpdateMode = Some UpdateMode.Full }

let drawIcon() =
    
    let glyphs = SegoeMdl2Glyphs.SegoeMdl2.Force()
    let icon = glyphs.GetIcon(64, SegoeMdl2Glyphs.Glyphs.Warning)
    
    let draw (g: IImageProcessingContext) =
        let opacity = 1f
        
        g.DrawImage(icon, Point(0, 0), opacity)
        |> ignore
    
    let buffer = new Image<Rgb24>(ReMarkable.Display.Width, ReMarkable.Display.Height)
    buffer.Mutate(fun z ->
                               z.Clear(Color.White) |> ignore)
    buffer.Mutate draw
    ReMarkable.Display.DrawAndRefresh
        { Image = buffer
          SrcArea = Rectangle(0, 0, ReMarkable.Display.Width, ReMarkable.Display.Height)
          DestPoint = Point(0, 0)
          
          RefreshArea = None
          WaveformMode = None
          DisplayTemp = None
          UpdateMode = None }
    
let startDigitizerTracker() =
    let mutable prevState: StylusState option = None
    let mutable lastUpdated = DateTime.Now

    ReMarkable.Digitizer_PleaseReadCommentAtDefinition.StylusUpdate.Add(fun state ->
        match prevState with
        | Some prev ->
            let wasNotPressingBefore = prev.Pressure < 10
            let isPressingNow = state.Pressure >= 10
            
            if wasNotPressingBefore && isPressingNow && lastUpdated <= DateTime.Now then
                printfn "Stylus updated: %A" state
                lastUpdated <- DateTime.Now + TimeSpan.FromMilliseconds(50.0)
        | None -> ()
            
        prevState <- Some state
    )

[<EntryPoint>]
let main _argv =
    printfn "hello from F#"
    let threadLock = new ManualResetEventSlim()
    
    ReMarkable.Touchscreen.Pressed.Add (printfn "Pressed: %A")
    ReMarkable.Touchscreen.Pressed.Add (printfn "Released: %A")
    
    
    
//    let font = Fonts.SegoeUi.CreateFont(100f)
//    let text = sprintf "Now: %A" DateTime.Now
    
//    let containingRectangle, buffer =
//        let textRectangle = Fonts.measureString(font, text) //|> RectangleExtensions.GetContainingIntRect
//        printfn "Text rectangle: %A" textRectangle
//        
//        let buffer = new Image<Rgb24>(textRectangle.Width, textRectangle.Height)
//        Fonts.drawString(font, Color.LightGray, buffer, text)
//        textRectangle, buffer
//    
//    ReMarkable.Display.DrawAndRefresh 
//        { Image = buffer
//          SrcArea = containingRectangle
//          DestPoint = Point(0, 0)
//          
//          RefreshArea = Some containingRectangle
//          WaveformMode = None
//          DisplayTemp = None
//          UpdateMode = None }
  
    threadLock.Wait()
    0
    