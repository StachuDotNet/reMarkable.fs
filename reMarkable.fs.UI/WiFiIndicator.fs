module reMarkable.fs.UI.WifiIndicator

open System
open reMarkable.fs.UI.SegoeMdl2Glyphs
open reMarkable.fs.Wireless

type WifiStatus =
    | Connected
    | Disabled
    | NotConnected
    | NoInternet

let getWifiStatusIcon =
    let wiFiIndicators = [Glyphs.Wifi1; Glyphs.Wifi2; Glyphs.Wifi3; Glyphs.Wifi4]
    let wiFiWarningIndicators = [Glyphs.WifiWarning1; Glyphs.WifiWarning2; Glyphs.WifiWarning3; Glyphs.WifiWarning4]
        
    fun (quality: WirelessQuality)  ->
        
        let linkQuality = quality.Link
        let signalStrength = quality.Gain
        
        let wifiStatus =
            match signalStrength with
            | 0 -> WifiStatus.NotConnected
            | _ -> WifiStatus.Connected
                
        let wifiStrength = linkQuality
        
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