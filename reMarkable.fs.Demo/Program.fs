open System
open System.Threading
open reMarkable.fs.UI
open reMarkable.fs.Digitizer
open reMarkable.fs
open reMarkable.fs.Display
open Sandbox.Pages

let logger = Lumberjack.CreateLogger("reMarkable.fs.Demo")

let displayDriver = Devices.Display

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

    Devices.Digitizer.Released.Add (fun _code -> logger.Info("digitizer pressed"))

    Devices.PhysicalButtons.Pressed.Add (fun test -> logger.Info(test.ToString()))
    
    Devices.Touchscreen.Pressed.Add (fun _state -> logger.Info("touchscreen pressed"))
    Devices.Touchscreen.Released.Add(fun _state -> logger.Info("touchscreen released"))
    
    let mainPage = w.CreatePage<MainPage>()
    
    logger.Info("Showing main page");
    w.ShowPage(mainPage);

    //logger.Info("Memory: {0}", PassiveDevices.Performance.TotalMemory);
    
    let mutable prevState: StylusState option = None
    let mutable time = DateTime.Now
    
    Devices.Digitizer.StylusUpdate.Add(fun state ->
        match prevState with
        | Some prev ->
            if prev.Pressure < 10 && state.Pressure >= 10 && time <= DateTime.Now then
                logger.Info("stylus updated")
                time <- DateTime.Now + TimeSpan.FromMilliseconds(50.0)
        | None -> ()
            
        prevState <- Some state
    )
    
    threadLock.Wait()
    
    0
    