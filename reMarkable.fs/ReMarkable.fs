module reMarkable.fs.ReMarkable

open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Linq
open reMarkable.fs.Digitizer
open reMarkable.fs.Display
open reMarkable.fs.PhysicalButtons
open reMarkable.fs.Touchscreen
open reMarkable.fs.PowerSupply
open reMarkable.fs.Unix.Driver.Performance
open reMarkable.fs.Wireless

/// Possible Remarkable or emulated device types
type Device = RM1 | RM2
    
/// Parses `/proc/bus/input/devices` to create a mapped dictionary of input device names and their event streams
let currentDeviceMap: Dictionary<string, string> =
    use r = new StreamReader("/proc/bus/input/devices")
    let nameRegex = Regex("^N: Name=\"(?<name>.+)\"$", RegexOptions.Compiled)
    let handlersRegex = Regex("^H: Handlers=(?<handlers>.+)$", RegexOptions.Compiled)

    let deviceMap = Dictionary<string, string>()

    let mutable currentDevice: string = null

    while not r.EndOfStream do
        let line = r.ReadLine()
        
        if String.IsNullOrWhiteSpace line then
            ()
        else
            match line.[0] with
            | 'N' -> // found a new device
                let matchedThing = nameRegex.Match(line)
                if not matchedThing.Success then
                    raise <| InvalidDataException("Unexpected name formatting in /proc/bus/input/devices");

                currentDevice <- matchedThing.Groups.["name"].Value
            | 'H' ->
                if currentDevice = null then
                    raise <| InvalidDataException("Unexpected handlers entry without device");

                let matchedThing = handlersRegex.Match(line)
                if not matchedThing.Success then
                    raise <| InvalidDataException("Unexpected handlers formatting in /proc/bus/input/devices");

                let handlers = matchedThing.Groups.["handlers"].Value.Split(' ')

                let eventHandler = handlers.FirstOrDefault(fun s -> s.StartsWith("event"))

                deviceMap.Add(currentDevice, $"/dev/input/{eventHandler}")

                currentDevice <- null
            | _ -> ()

    deviceMap

let CurrentDevice =
    if (currentDeviceMap.ContainsKey("30370000.snvs:snvs-powerkey")) then // rm2 has a power button
        Device.RM2
    elif currentDeviceMap.ContainsKey("cyttsp5_mt") then // rm1 touch screen driver
        Device.RM1
    else
        raise <| Exception("Can't figure out which device we're working with.");

let Display =
    match CurrentDevice with
    | Device.RM1 -> failwith """new HardwareDisplayDriver("/dev/fb0")"""
    | Device.RM2 -> new RM2ShimDisplayDriver() :> IDisplayDriver

/// Holds an instance of a power supply monitor for the battery
let PowerSupply =
    HardwarePowerSupplyMonitor("/sys/class/power_supply/bq27441-0") // todo: don't hardcode :)
    :> IPowerSupplyMonitor

let Wireless = HardwareWirelessMonitor()

///// Holds an instance of a digitizer driver 
//let Digitizer =
//    match CurrentDevice with
//    | Device.RM1
//    | Device.RM2 ->
//        new HardwareDigitizerDriver(currentDeviceMap.["Wacom I2C Digitizer"], 20967, 15725)
//        :> IDigitizerDriver

/// Holds an instance of a physical button driver
let PhysicalButtons =
    match CurrentDevice with
    | Device.RM1 -> new PhysicalButtonDriver(currentDeviceMap.["gpio-keys"])
    | Device.RM2 -> new PhysicalButtonDriver(currentDeviceMap.["30370000.snvs:snvs-powerkey"])

// /// Holds an instance of a touchscreen driver
//let Touchscreen =
//    match CurrentDevice with
//    | Device.RM1 -> new HardwareTouchscreenDriver(currentDeviceMap.["cyttsp5_mt"], 767, 1023, 32, false)
//    | Device.RM2 -> new HardwareTouchscreenDriver(currentDeviceMap.["pt_mt"], 1403, 1871, 32, false)

/// Holds an instance of a performance monitor
let Performance = HardwarePerformanceMonitor() :> IPerformanceMonitor

// /// Holds an instance of a power supply monitor for USB power
//let UsbPower = new HardwarePowerSupplyMonitor("/sys/class/power_supply/imx_usb_charger");

//let Keyboard = new HardwareKeyboardDriver("TODO") :> IKeyboardDriver