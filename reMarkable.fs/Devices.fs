module reMarkable.fs.Devices

open System
open reMarkable.fs.Digitizer
open reMarkable.fs.Display
open reMarkable.fs.PhysicalButtons
open reMarkable.fs.Touchscreen
open reMarkable.fs.PowerSupply
open reMarkable.fs.Wireless

let currentDeviceMap = Device.GetInputDeviceEventHandlers()

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
let Battery =
    HardwarePowerSupplyMonitor("/sys/class/power_supply/bq27441-0")
    :> IPowerSupplyMonitor

let Wireless =
    HardwareWirelessMonitor()

/// Holds an instance of a digitizer driver 
let Digitizer =
    match CurrentDevice with
    | Device.RM1-> failwith """HardwareDigitizerDriver(rm1DeviceMap["Wacom I2C Digitizer"], 20967, 15725)"""
    | Device.RM2 -> HardwareDigitizerDriver(currentDeviceMap.["Wacom I2C Digitizer"], 20967, 15725) :> IDigitizerDriver

/// Holds an instance of a physical button driver
let PhysicalButtons =
    match CurrentDevice with
    | Device.RM1 -> PhysicalButtonDriver(currentDeviceMap.["gpio-keys"])
    | Device.RM2 -> PhysicalButtonDriver(currentDeviceMap.["30370000.snvs:snvs-powerkey"])

/// Holds an instance of a touchscreen driver
let Touchscreen =
    match CurrentDevice with
    | Device.RM1 -> HardwareTouchscreenDriver(currentDeviceMap.["cyttsp5_mt"], 767, 1023, 32, false)
    | Device.RM2 -> HardwareTouchscreenDriver(currentDeviceMap.["pt_mt"], 1403, 1871, 32, false)

// /// Holds an instance of a performance monitor
//let Performance = new HardwarePerformanceMonitor();

// /// Holds an instance of a power supply monitor for USB power
//let UsbPower = new HardwarePowerSupplyMonitor("/sys/class/power_supply/imx_usb_charger");

