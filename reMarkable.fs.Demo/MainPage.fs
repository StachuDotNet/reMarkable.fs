namespace Sandbox.Pages

open reMarkable.fs.UI
open reMarkable.fs.UI.Controls
open reMarkable.fs.UI.BatteryIndicatorHelpers
open SixLabors.Fonts
open SixLabors.ImageSharp
open reMarkable.fs.UI.Util.RectangleExtensions

type MainPage() =
    inherit Panel()
    
    let toolbarHeight = 64f

    override this.LayoutControls() =
        let wifiIndicator = WiFiIndicator(Some 64)
        wifiIndicator.Size <- SizeF(toolbarHeight, toolbarHeight)
        wifiIndicator.TextAlign <- { Vertical = Some Center; Horizontal = Some Right }
        
        let batteryIndicator = BatteryIndicator(Some 64)
        batteryIndicator.Size <- SizeF(toolbarHeight, toolbarHeight)
        batteryIndicator.TextAlign <- { Vertical = Some Center; Horizontal = Some HorizontalAlign.Center }
        
        let label = Label("Hi, Stachu!", Fonts.SegoeUi.CreateFont(60f))
        label.TextAlign <- { Vertical = Some Bottom; Horizontal = Some Left }
        label.Size <- SizeF(toolbarHeight * 5f, toolbarHeight)
        
        let bottomToolbar = HorizontalStackPanel()
        bottomToolbar.Size <- SizeF(this.Size.Width, toolbarHeight)
        bottomToolbar.Location <- PointF(0f, this.Bounds.Height - toolbarHeight)
        
        bottomToolbar.Add wifiIndicator
        bottomToolbar.Add batteryIndicator
        bottomToolbar.Add label
        
        this.Add bottomToolbar
