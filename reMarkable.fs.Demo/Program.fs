open System
open System.IO
open System.Reflection
open System.Threading
open SixLabors.ImageSharp
open SixLabors.ImageSharp.PixelFormats
open reMarkable.fs.UI
open reMarkable.fs.Digitizer
open reMarkable.fs
open reMarkable.fs.Display
open Sandbox.Pages
open reMarkable.fs.Util

let logger = Logger.CreateLogger("reMarkable.fs.Demo")

let displayDriver = ReMarkable.Display

let windowUpdate (e: WindowUpdateEventArgs) =
     logger.Info("WindowUpdate")
     
     let args: DrawArgs =
         { Image = e.Buffer
           SrcArea = e.UpdatedArea
           DestPoint = e.UpdatedArea.Location
           RefreshArea = None
           WaveformMode = None
           DisplayTemp = Some DisplayTemp.RemarkableDraw
           UpdateMode = None }
     
     displayDriver.Draw(args)

[<EntryPoint>]
let main _argv =
    //Devices.PhysicalButtons.Pressed.Add(printfn "Button pressed: %A")

    logger.Info("hello from F#")

    let threadLock = new ManualResetEventSlim()

    let w = Window(displayDriver.VisibleWidth, displayDriver.VisibleHeight)
    
    logger.Info($"Width: (virtual: {displayDriver.VirtualWidth} visible: {displayDriver.VisibleWidth})")
    logger.Info($"Height: (virtual: {displayDriver.VirtualHeight} visible: {displayDriver.VisibleHeight})")
    
    w.Update.Add windowUpdate

    ReMarkable.Digitizer.Released.Add (fun _code -> logger.Info("digitizer pressed"))

    ReMarkable.PhysicalButtons.Pressed.Add (fun test -> logger.Info(test.ToString()))
    
    ReMarkable.Touchscreen.Pressed.Add (fun _state -> logger.Info("touchscreen pressed"))
    ReMarkable.Touchscreen.Released.Add(fun _state -> logger.Info("touchscreen released"))
    
    let mainPage = w.CreatePage<MainPage>()
    
    logger.Info("Showing main page")
    w.ShowPage(mainPage)

    //logger.Info("Memory: {0}", PassiveDevices.Performance.TotalMemory);
    
    
    
    
    
    let mutable prevState: StylusState option = None
    let mutable lastUpdated = DateTime.Now
    ReMarkable.Digitizer.StylusUpdate.Add(fun state ->
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
        
    //    for x in 100 .. 150 do
    //        for y in 100 .. 150 do
    //            ReMarkable.Display.Framebuffer.SetPixel {| Color= Color.Gray; X = x; Y = y |}
        
    let img: Image<Rgb24> = Image.Load(Path.Combine(Path.GetDirectoryName (Assembly.GetExecutingAssembly().Location), "blackPepper.jpeg"))
    
    for x in 0 .. 2 .. 13 do
        for y in 0 .. 2 .. 17 do
            let destX = x * 100
            let destY = y * 100
            
            printfn "%i, %i" destX destY
            
            ReMarkable.Display.Draw
                { Image = img
                  SrcArea = Rectangle(0, 0, 144, 144)
                  DestPoint = Point(destX, destY)
                  
                  RefreshArea = Rectangle(destX, destY, 144, 144) |> Some
                  WaveformMode = None
                  DisplayTemp = None
                  UpdateMode = Some UpdateMode.Partial }    

    threadLock.Wait()
    
    0
    