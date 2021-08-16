module reMarkable.fs.PowerSupply

// context: https://remarkablewiki.com/tech/power

open System
open System.IO

/// Converts "micro" units to their base unit
/// <param name="value">The value offset by 10^6</param>
/// <returns>The value multiplied by 10^(-6)</returns>
let convertToMicro = (*) (Math.Pow(10.0, -6.0))

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
    // /// Gets the charge, in Wh, of the power supply the last time it was full
    // /// <returns>The charge, in Wh</returns>
    // float GetChargeFull();
    //
    // /// Gets the charge, in Wh, of the power supply when full as designed
    // /// <returns>The charge, in Wh</returns>
    // float GetChargeFullDesign();
    //
    // /// Gets the instantaneous charge, in Wh, of the power supply
    // /// <returns>The charge, in Wh</returns>
    // float GetChargeNow();
    //
    // /// <summary>
    // ///     Gets the instantaneous current, in A, of the power supply
    // ///
    // /// <returns>The current, in A</returns>
    // float GetCurrentNow();

    /// Gets the power supply percentage remaining
    /// <returns>A float between 0 and 1, inclusive</returns>
    abstract member GetPercentage: unit -> float32

    /// Gets the status of the power supply
    /// <returns> The instantaneous status of the power supply </returns>
    abstract member GetStatus: unit -> PowerSupplyStatus

    // /// Gets the instantaneous temperature, in degrees C, of the power supply
    // /// <returns>The temperature, in degrees C</returns>
    // float GetTemperature();
    //
    // /// Gets the instantaneous voltage, in V, of the power supply
    // /// <returns>The voltage, in V</returns>
    // float GetVoltageNow();
    //
    // /// True if the power is flowing from an external source
    // bool IsOnline();
    //
    // /// Determines if the power supply is present in the device
    // /// <returns>True if the power supply is present in the device</returns>
    // bool IsPresent();

/// Provides methods for reading power supply information from the installed hardware
/// <param name="device">The base device path to read information from</param>
type HardwarePowerSupplyMonitor(device: string) =
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

    // public float GetChargeFull()
    //     if (!TryReadAttr("charge_full", out var value))
    //         return 0;
    //
    //     return DeviceUtils.MicroToBaseUnit(int.Parse(value));
    //
    // public float GetChargeFullDesign()
    //     if (!TryReadAttr("charge_full_design", out var value))
    //         return 0;
    //
    //     return DeviceUtils.MicroToBaseUnit(int.Parse(value));
    //
    // public float GetChargeNow()
    //     if (!TryReadAttr("charge_now", out var value))
    //         return 0;
    //
    //     return DeviceUtils.MicroToBaseUnit(int.Parse(value));

    // public float GetCurrentNow()
    //     if (!TryReadAttr("current_now", out var value))
    //         return 0;
    //
    //     return DeviceUtils.MicroToBaseUnit(int.Parse(value));

    // public float GetTemperature()
    //     if (!TryReadAttr("temp", out var value))
    //         return 0;
    //
    //     // tenths of a degree C
    //     return int.Parse(value) / 10f;
    //
    // public float GetVoltageNow()
    //     if (!TryReadAttr("voltage_now", out var value))
    //         return 0;
    //
    //     return DeviceUtils.MicroToBaseUnit(int.Parse(value));
    //
    // public bool IsOnline()
    //     if (!TryReadAttr("online", out var value))
    //         return false;
    //
    //     return int.Parse(value) == 1;
    //
    // public bool IsPresent()
    //     if (!TryReadAttr("present", out var value))
    //         return false;
    //
    //     return int.Parse(value) == 1;
