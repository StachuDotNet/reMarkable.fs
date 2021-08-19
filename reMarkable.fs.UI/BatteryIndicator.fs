module reMarkable.fs.UI.BatteryIndicatorHelpers

open System
open reMarkable.fs.UI.SegoeMdl2Glyphs
open reMarkable.fs
open reMarkable.fs.PowerSupply

let getBatteryIcon =
    let batteryIndicators =
        [ Glyphs.VerticalBattery0
          Glyphs.VerticalBattery1
          Glyphs.VerticalBattery2
          Glyphs.VerticalBattery3
          Glyphs.VerticalBattery4
          Glyphs.VerticalBattery5
          Glyphs.VerticalBattery6
          Glyphs.VerticalBattery7
          Glyphs.VerticalBattery8
          Glyphs.VerticalBattery9
          Glyphs.VerticalBattery10 ]

    let chargingBatteryIndicators =
        [ Glyphs.VerticalBatteryCharging0
          Glyphs.VerticalBatteryCharging1
          Glyphs.VerticalBatteryCharging2
          Glyphs.VerticalBatteryCharging3
          Glyphs.VerticalBatteryCharging4
          Glyphs.VerticalBatteryCharging5
          Glyphs.VerticalBatteryCharging6
          Glyphs.VerticalBatteryCharging7
          Glyphs.VerticalBatteryCharging8
          Glyphs.VerticalBatteryCharging9
          Glyphs.VerticalBatteryCharging10 ]
    
    fun batteryPercentage ->
        printfn "%%: %.2f" batteryPercentage

        let indicators =
            if ReMarkable.PowerSupply.GetStatus() <> PowerSupplyStatus.Charging then 
                chargingBatteryIndicators
            else
                batteryIndicators
        
        let batteryIndex =
            let thingToRound = (indicators.Length - 1 |> float32) * batteryPercentage
            Math.Round(thingToRound |> float) |> int //todo: float _maybe_ unsafe? seems really unlikely.
        
        indicators.[batteryIndex]
