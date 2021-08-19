// context: https://remarkablewiki.com/tech/power
module reMarkable.fs.PowerSupply

open System
open System.IO

/// Defines valid statuses for a power supply
type PowerSupplyStatus =
    /// It is impossible to determine the status of the power supply
    | Unknown

    /// The power supply is attached to external power and is charging
    | Charging

    /// The power supply is draining stored power
    | Discharging

    /// The power supply is attached to external power and is neither charging nor full
    | NotCharging

    /// The power supply contains the maximum possible power
    | Full

/// Provides an interface through which the device's power supply information can be monitored
type IPowerSupplyMonitor =
    /// Gets the charge, in Wh, of the power supply the last time it was full
    abstract member GetChargeFull: unit -> float32

    /// Gets the charge, in Wh, of the power supply when full as designed
    abstract member GetChargeFullDesign: unit -> float32

    /// Gets the instantaneous charge, in Wh, of the power supply
    abstract member GetChargeNow: unit -> float32

    /// Gets the instantaneous current, in A, of the power supply
    abstract member GetCurrentNow: unit -> float32

    /// Gets the power supply percentage remaining, between 0 and 1, inclusive
    abstract member GetPercentage: unit -> float32

    /// Gets the status of the power supply
    abstract member GetStatus: unit -> PowerSupplyStatus

    /// Gets the instantaneous temperature, in degrees C, of the power supply
    abstract member GetTemperature: unit -> float32
    
    /// Gets the instantaneous voltage, in V, of the power supply
    abstract member GetVoltageNow: unit -> float32

    /// True if the power is flowing from an external source
    abstract member IsOnline: unit -> bool

    /// Checks if the power supply is present in the device
    abstract member IsPresent: unit -> bool

/// Provides methods for reading power supply information from the installed hardware
/// <param name="device">The base device path to read information from</param>
type HardwarePowerSupplyMonitor(device: string) =
    /// Converts "micro" units to their base unit
    let convertToMicro (v: int) =
        float32 v * (float32 (Math.Pow(10.0, -6.0)))
    
    /// Attempts to read an attribute file as a file located in the directory specified by <see cref="Device" />
    /// <param name="attr">The attribute file to read</param>
    /// <param name="value">The value as read from the file</param>
    /// <returns>True if the attribute exists and the read was successful, false otherwise</returns>
    let tryReadAttr (attr: string): string option =
        let file = Path.Combine(device, attr)
        
        if (File.Exists(file)) then
            File.ReadAllText(file) |> Some
        else
            printfn "file didn't exist: %s" file
            None

    /// The base device path to read information from
    member _.Device = device
    
    interface IPowerSupplyMonitor with
        member _.GetPercentage(): float32 =
            match tryReadAttr "capacity" with
            | None -> 0 |> float32
            | Some capacity ->
                Int32.Parse(capacity)
                |> float32
                |> (/) 100f

        member _.GetStatus(): PowerSupplyStatus =
            let status = tryReadAttr "status"
            match status with
            | Some "Charging\n" -> PowerSupplyStatus.Charging
            | Some "Discharging\n" -> PowerSupplyStatus.Discharging
            | Some "NotCharging\n" -> PowerSupplyStatus.NotCharging
            | Some "FULL\n" -> PowerSupplyStatus.Full
            | _ -> PowerSupplyStatus.Unknown

        member _.GetCurrentNow(): float32 =
            match tryReadAttr "current_now" with
            | None -> 0 |> float32
            | Some current -> Int32.Parse current |> convertToMicro
    
        member _.GetChargeFull(): float32 =
            match tryReadAttr "charge_full" with
            | None -> 0 |> float32
            | Some charge -> Int32.Parse charge |> convertToMicro

        member _.GetChargeFullDesign(): float32 =
            match tryReadAttr "charge_full_design" with
            | None -> 0 |> float32
            | Some charge -> Int32.Parse charge |> convertToMicro

        member _.GetChargeNow(): float32 =
            match tryReadAttr "charge_now" with
            | None -> 0 |> float32
            | Some charge -> Int32.Parse charge |> convertToMicro

        member _.GetTemperature(): float32 =
            match tryReadAttr "charge_now" with
            | None -> 0 |> float32
            | Some charge -> Int32.Parse charge |> float32 |> (/) (float32 10)

        member _.GetVoltageNow() =
            match tryReadAttr "voltage_now" with
            | None -> 0 |> float32
            | Some voltage -> Int32.Parse voltage |> convertToMicro
            
        member _.IsOnline() =
            match tryReadAttr "online" with
            | None -> false
            | Some online -> Int32.Parse online = 1
            
        member _.IsPresent() =
            match tryReadAttr "present" with
            | None -> false
            | Some present -> Int32.Parse present = 1