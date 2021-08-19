open System
open System.Drawing
open System.IO
open System.Reflection
open System.Threading
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open reMarkable.fs.UI
open reMarkable.fs.Digitizer
open reMarkable.fs
open reMarkable.fs.Display

let logger = Logger.CreateLogger("reMarkable.fs.Demo")

let displayDriver = ReMarkable.Display


open System
open reMarkable.fs.UI
open reMarkable.fs.UI.Util
open reMarkable.fs.UI.Util.RectangleExtensions
open SixLabors.Fonts
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Drawing.Processing
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing


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
    //buffer.Mutate(fun z -> z.DrawImage)
    
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
        //g.Resize (200, 200) |> ignore
        
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
    

[<EntryPoint>]
let main _argv =
    //Devices.PhysicalButtons.Pressed.Add(printfn "Button pressed: %A")

    logger.Info("hello from F#")
    
//    
//    printfn "NumberOfCores: %A" ReMarkable.Performance.NumberOfCores
//    printfn "NumberOfProcessors: %A" ReMarkable.Performance.NumberOfProcessors
//    printfn "TotalMemory: %A" ReMarkable.Performance.TotalMemory
//    printfn "TotalSwap: %A" ReMarkable.Performance.TotalSwap
//    printfn "GetFreeMemory: %A" (ReMarkable.Performance.GetFreeMemory())
//    printfn "GetFreeSwap: %A" (ReMarkable.Performance.GetFreeSwap())
//    printfn "GetNetworkAdapters: %A" (ReMarkable.Performance.GetNetworkAdapters())
//    printfn "GetProcessorTime: %A" (ReMarkable.Performance.GetProcessorTime())

    
//    ReMarkable.Digitizer.Released.Add (fun _code -> logger.Info("digitizer pressed"))
//
//    ReMarkable.PhysicalButtons.Pressed.Add (fun test -> logger.Info(test.ToString()))
//    
//    ReMarkable.Touchscreen.Pressed.Add (fun _state -> logger.Info("touchscreen pressed"))
//    ReMarkable.Touchscreen.Released.Add(fun _state -> logger.Info("touchscreen released"))
//    logger.Info($"Width: {displayDriver.Width}")
//    logger.Info($"Height: {displayDriver.Height}")
    
    let threadLock = new ManualResetEventSlim()
    
    
//    let buffer = new Image<Rgb24>(displayDriver.Width, displayDriver.Height)
//    
//    let font = Fonts.SegoeUi.CreateFont(32f)
//    let glyphs = SegoeMdl2Glyphs.SegoeMdl2
//    
//    IconLabel.drawIcon glyphs Point.Empty buffer SegoeMdl2Glyphs.Glyphs.FavoriteStar 32
//    
//    let args: DrawArgs =
//         { Image = buffer
//           SrcArea = Rectangle()
//           DestPoint = e.UpdatedArea.Location
//           RefreshArea = None
//           WaveformMode = None
//           DisplayTemp = Some DisplayTemp.RemarkableDraw
//           UpdateMode = None }
//     
//    displayDriver.Draw(args)
    
//         let wifiIndicator = WiFiIndicator(Some 64)
//        wifiIndicator.Size <- SizeF(toolbarHeight, toolbarHeight)
//        
//        let batteryIndicator = BatteryIndicator(Some 64)
//        batteryIndicator.Size <- SizeF(toolbarHeight, toolbarHeight)
//        
//        let label = Label("Hi, Stachu!", Fonts.SegoeUi.CreateFont(60f))
//        label.Size <- SizeF(toolbarHeight * 5f, toolbarHeight)
//        
//    Fonts.drawString(font, foregroundColor, buffer, text)
//
//    Fonts.measureString(font, text, this.Bounds, this.TextAlign)
//
//    
//    let w = Window(displayDriver.Width, displayDriver.Height)
//    let mainPage = Page(w, displayDriver.Width, displayDriver.Height, MainPage())
//    w.Update.Add windowUpdate
//    logger.Info("Showing main page")
//    w.ShowPage(mainPage)

    
//    let mutable prevState: StylusState option = None
//    let mutable lastUpdated = DateTime.Now
//    ReMarkable.Digitizer.StylusUpdate.Add(fun state ->
//        match prevState with
//        | Some prev ->
//            let wasNotPressingBefore = prev.Pressure < 10
//            let isPressingNow = state.Pressure >= 10
//            
//            if wasNotPressingBefore && isPressingNow && lastUpdated <= DateTime.Now then
//                printfn "Stylus updated: %A" state
//                lastUpdated <- DateTime.Now + TimeSpan.FromMilliseconds(50.0)
//        | None -> ()
//            
//        prevState <- Some state
//    )
        
    //    for x in 100 .. 150 do
    //        for y in 100 .. 150 do
    //            ReMarkable.Display.Framebuffer.SetPixel {| Color= Color.Gray; X = x; Y = y |}
        
    
    
    drawBeans()
    drawGiantRectangle()
    drawIcon()
            
    threadLock.Wait()
    
    0
    