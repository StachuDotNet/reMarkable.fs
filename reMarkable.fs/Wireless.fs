namespace reMarkable.fs.Wireless

open System
open System.IO
open System.Text.RegularExpressions

/// Contains data related to wireless network quality
type WirelessQuality =
  { /// The signal gain
    Gain: int
    
    /// The link quality
    Link: float32
    
    /// The signal noise baseline
    Noise: int }

/// Provides a set of methods to monitor the installed wireless networking hardware
type HardwareWirelessMonitor() =
    /// Parses the wireless network status file
    member _.GetQuality(): WirelessQuality =
        let lines = File.ReadAllLines("/proc/net/wireless")

        let wlan0 = lines.[2]
        let columns = Regex.Split(wlan0.Trim(), "\\s+")

        let qualityLink = Int32.Parse(columns.[2].Trim('.'))
        let qualityLevel = Int32.Parse(columns.[3].Trim('.'))
        let qualityNoise = Int32.Parse(columns.[4])

        { Link = (qualityLink |> float32) / 70f
          Gain = qualityLevel
          Noise = qualityNoise }