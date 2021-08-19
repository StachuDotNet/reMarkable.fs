module reMarkable.fs.Unix.Driver.Performance

open System
open System.Collections.Generic
open System.IO
open System.Text.RegularExpressions

/// Provides an interface through which the system performance can be profiled
type IPerformanceMonitor =
    /// Gets the number of total logical cores in the device
    abstract member NumberOfCores: int

    /// Gets the number of physical processors in the device
    abstract member NumberOfProcessors: int

    /// Gets the total amount of memory in the device, in bytes
    abstract member TotalMemory: int64

    /// Gets the total amount of swap in the device, in bytes
    abstract member TotalSwap: int64

    /// Gets the total amount of free memory, in bytes
    abstract member GetFreeMemory: unit-> int64

    /// Gets the total amount of free swap, in bytes
    abstract member GetFreeSwap: unit -> int64

    /// Lists all of the available network adapters
    abstract member GetNetworkAdapters: unit -> string array

    /// Gets the instantaneous network download speed, in bytes/second
    abstract member GetNetworkRxSpeed: adapter: string -> int64

    /// Gets the total network download utilization, in bytes
    abstract member GetNetworkRxTotal: adapter: string -> int64

    /// Gets the instantaneous network upload speed, in bytes/second
    abstract member GetNetworkTxSpeed: adapter: string -> int64

    /// Gets the total network upload utilization, in bytes
    abstract member GetNetworkTxTotal: adapter: string -> int64

    /// Gets the total processor utilization, from 0-1
    abstract member GetProcessorTime: unit -> float32

    /// Gets a specific processor's utilization, from 0-1
    abstract member GetProcessorTime: processor: int -> float32

    /// Gets a specific core's utilization, from 0-1
    abstract member GetProcessorTime: processor:int * core:int -> float32
    

/// Contains data associated with an instantaneous CPU measurement
/// 
/// <param name="total">The total processor time for this sample</param>
/// <param name="idle">The idle processor time for this sample</param>
type CpuMeasurement(total: int64, idle: int64) =
    /// The idle processor time for this sample
    member _.Idle = idle

    /// The total processor time for this sample
    member _.Total = total

 /// Contains data related to the statistics of a network adapter
type NetDeviceInfo =
    { RxBytes: int64 
      RxCompressed: int64 
      RxDrop: int64 
      RxErrs: int64 
      RxFifo: int64 
      RxFrame: int64 
      RxMulticast: int64 
      RxPackets: int64 
      TxBytes: int64 
      TxCompressed: int64 
      TxDrop: int64 
      TxErrs: int64 
      TxFifo: int64 
      TxFrame: int64 
      TxMulticast: int64 
      TxPackets: int64 }
        
/// Provides methods for calculating static and time-based performance metrics
type PerformanceMeasurement() =
    let mutable _previousTime: DateTime = DateTime.Now // ?
    let mutable _previousValue: double = Double.MinValue //?

    /// Calculates a differential based on the previous and new values
    /// <param name="measurement">The new value to compare to the old value</param>
    /// <returns>The calculated delta</returns>
    member _.PushMeasurement (measurement: double): double =
        let dM = measurement - _previousValue
        _previousValue <- measurement

        dM

    /// Calculates a time-based differential based on the previous and new values and the previous and current time
    /// <param name="measurement">The new value to compare to the old value</param>
    /// <returns>The calculated delta in units per second</returns>
    member _.PushMeasurementPerSecond(measurement: double): double =
        let time = DateTime.Now

        let dT = time - _previousTime
        let dM = measurement - _previousValue

        _previousTime <- time
        _previousValue <- measurement

        dM / dT.TotalSeconds
    
let getNetworkMeasurements(): Dictionary<string, NetDeviceInfo> =
    let result = Dictionary<string, NetDeviceInfo>()

    use sr = new StreamReader("/proc/net/dev")
    
    // 2 header lines - throw them away
    sr.ReadLine() |> ignore
    sr.ReadLine() |> ignore

    while not sr.EndOfStream do
        let line = sr.ReadLine()

        if line = null then
            ()
        else
            let columns = Regex.Split(line.Trim(), "\\s+")

            let info: NetDeviceInfo =
                { RxBytes = Int64.Parse(columns.[1]) 
                  RxCompressed = Int64.Parse(columns.[7]) 
                  RxDrop = Int64.Parse(columns.[4]) 
                  RxErrs = Int64.Parse(columns.[3]) 
                  RxFifo = Int64.Parse(columns.[5]) 
                  RxFrame = Int64.Parse(columns.[6]) 
                  RxMulticast = Int64.Parse(columns.[8]) 
                  RxPackets = Int64.Parse(columns.[2])
                  
                  TxBytes = Int64.Parse(columns.[9]) 
                  TxCompressed = Int64.Parse(columns.[15]) 
                  TxDrop = Int64.Parse(columns.[12]) 
                  TxErrs = Int64.Parse(columns.[11]) 
                  TxFifo = Int64.Parse(columns.[13]) 
                  TxFrame = Int64.Parse(columns.[14]) 
                  TxMulticast = Int64.Parse(columns.[16]) 
                  TxPackets = Int64.Parse(columns.[10]) }
            
            let mutable adapterName = columns.[0]
            adapterName <- adapterName.Remove(adapterName.Length - 1)

            result.Add(adapterName, info)
    result
    
let getCpuMeasurements(): Dictionary<string, CpuMeasurement> =
     let result = Dictionary<string, CpuMeasurement>()

     use sr = new StreamReader("/proc/stat")
     
     while not sr.EndOfStream do
         let line = sr.ReadLine()

         match line with
         | null -> ()
         | line when line.StartsWith "cpu" -> 
             let columns = Regex.Split(line, "\\s+")

             let cpuIndex = columns.[0]

             let totalTime = columns |> Seq.tail |> Seq.sumBy Int64.Parse
             let idleTime = Int64.Parse(columns.[4])

             result.Add(cpuIndex, CpuMeasurement(totalTime, idleTime))
         | _ -> ()

     result

let getMemoryMeasurements():  Dictionary<string, int64> =
     let result = Dictionary<string, int64>()

     use sr = new StreamReader("/proc/meminfo")
     
     while not sr.EndOfStream do
         let line = sr.ReadLine()

         if line = null then
             () // todo: return early?
         else
             let regexMatch = Regex.Match(line, "(.+):\\s+(\\d+) kB")

             if not regexMatch.Success then
                 ()
             else
                 // Kilobytes
                 result.Add(regexMatch.Groups.[1].Value, Int64.Parse(regexMatch.Groups.[2].Value) * (int64 1000))

     result
 
 /// Provides a set of methods to profile hardware performance metrics
type HardwarePerformanceMonitor() =
    /// Contains the instantaneous CPU time-based measurements
    let globalCpuMeasurements = Dictionary<string, PerformanceMeasurement>()
    
    /// Contains the instantaneous network time-based measurements
    let globalNetworkMeasurements = Dictionary<string, PerformanceMeasurement>()
    
    let globalMemoryMeasurements = getMemoryMeasurements()
    
    do
        for keyValuePair in getCpuMeasurements() do
            globalCpuMeasurements.Add($"idle-{keyValuePair.Key}", PerformanceMeasurement())
            globalCpuMeasurements.Add($"total-{keyValuePair.Key}", PerformanceMeasurement())
        
        for keyValuePair in getNetworkMeasurements() do
            globalNetworkMeasurements.Add($"tx-{keyValuePair.Key}", PerformanceMeasurement())
            globalNetworkMeasurements.Add($"rx-{keyValuePair.Key}", PerformanceMeasurement())
        
    interface IPerformanceMonitor with
        member this.NumberOfCores = globalCpuMeasurements.Count - 1
        member this.NumberOfProcessors = 1
        member this.TotalMemory = globalMemoryMeasurements.["MemTotal"]
        member this.TotalSwap = globalMemoryMeasurements.["SwapTotal"]
        
        member this.GetFreeMemory() =
            let measurements = getMemoryMeasurements()
            measurements.["MemAvailable"]

        member this.GetFreeSwap() =
            let measurements = getMemoryMeasurements()
            measurements.["SwapFree"]
        
        member this.GetNetworkAdapters() = (getNetworkMeasurements()).Keys |> Seq.toArray
        member this.GetNetworkRxSpeed(adapter) =
            let nowBytes = (getNetworkMeasurements()).[adapter].RxBytes |> double
            let measurement = globalNetworkMeasurements.[$"rx-{adapter}"].PushMeasurementPerSecond nowBytes
            int64 measurement
            
        member this.GetNetworkRxTotal(adapter) =
            (getNetworkMeasurements()).[adapter].RxBytes
            
        member this.GetNetworkTxSpeed(adapter) =
            let nowBytes = (getNetworkMeasurements()).[adapter].TxBytes |> double
            let measurement = globalNetworkMeasurements.[$"tx-{adapter}"].PushMeasurementPerSecond(nowBytes)
            int64 measurement
            
        member this.GetNetworkTxTotal(adapter) =
            (getNetworkMeasurements()).[adapter].TxBytes
        
        member this.GetProcessorTime() =
            let cpuMeasurements = getCpuMeasurements()
            let cpuTotal = cpuMeasurements.["cpu"]

            let idle = globalCpuMeasurements.["idle-cpu"].PushMeasurement(double cpuTotal.Idle)
            let total = globalCpuMeasurements.["total-cpu"].PushMeasurement(double cpuTotal.Total)

            float32 ((double 1) - idle / total)
            
        member this.GetProcessorTime(processor, core) =
            if (processor <> 0) then
                raise <| ArgumentException(nameof processor)

            let cpuMeasurements = getCpuMeasurements()
            let cpuTotal = cpuMeasurements.["cpu"]

            let idle = globalCpuMeasurements.[$"idle-cpu{core}"].PushMeasurement(double cpuTotal.Idle)
            let total = globalCpuMeasurements.[$"total-cpu{core}"].PushMeasurement(double cpuTotal.Total)

            float32 ((double 1) - idle / total)

        member this.GetProcessorTime(processor) =
            if processor <> 0 then
                raise <| ArgumentException(nameof processor)

            (this :> IPerformanceMonitor).GetProcessorTime()