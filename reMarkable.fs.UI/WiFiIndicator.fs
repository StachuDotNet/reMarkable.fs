namespace reMarkable.fs.UI.Controls

open System
open reMarkable.fs.UI.SegoeMdl2Glyphs
open reMarkable.fs

type WifiStatus =
    | Connected
    | Disabled
    | NotConnected
    | NoInternet
    
type WiFiIndicator(iconSize: int option) as this =
    inherit IconLabel<Glyphs>(SegoeMdl2, iconSize, Glyphs.ProgressRingDots)
    
    let wiFiIndicators: Glyphs[] =
        [| Glyphs.Wifi1
           Glyphs.Wifi2
           Glyphs.Wifi3
           Glyphs.Wifi4 |]
        
    let wiFiWarningIndicators: Glyphs[] =
        [| Glyphs.WifiWarning1
           Glyphs.WifiWarning2
           Glyphs.WifiWarning3
           Glyphs.WifiWarning4 |]
    
    do
        this.Poll()
    
    member this.Poll() =
        let quality = ReMarkable.Wireless.GetQuality()
        let linkQuality = quality.Link
        let signalStrength = quality.Gain
        
        let wifiStatus =
            match signalStrength with
            | 0 -> WifiStatus.NotConnected
            | _ -> WifiStatus.Connected
                
        let wifiStrength = linkQuality
        
        this.Icon <-
            match wifiStatus with
            | WifiStatus.Connected
            | WifiStatus.NoInternet ->
                let indicators =
                    match wifiStatus with
                    | WifiStatus.Connected -> wiFiIndicators
                    | WifiStatus.NoInternet -> wiFiWarningIndicators
                    | _ -> failwith "unexpected"
            
                let batteryIndex =
                    let roundMe = (indicators.Length - 1 |> float32) * wifiStrength
                    roundMe |> float |> Math.Round |> int
                
                indicators.[batteryIndex]
            | WifiStatus.NotConnected ->  Glyphs.NetworkOffline
            | WifiStatus.Disabled ->  Glyphs.Airplane
