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
type NetDeviceInfo(rxBytes: int64, rxPackets: int64, rxErrs: int64, rxDrop: int64, rxFifo: int64, rxFrame: int64,
            rxCompressed: int64, rxMulticast: int64, txBytes: int64, txPackets: int64, txErrs: int64, txDrop: int64, txFifo: int64,
            txFrame: int64, txCompressed: int64, txMulticast: int64) =

    member _.RxBytes = rxBytes 
    member _.RxCompressed = rxCompressed 
    member _.RxDrop = rxDrop 
    member _.RxErrs = rxErrs 
    member _.RxFifo = rxFifo 
    member _.RxFrame = rxFrame 
    member _.RxMulticast = rxMulticast 
    member _.RxPackets = rxPackets 
    member _.TxBytes = txBytes 
    member _.TxCompressed = txCompressed 
    member _.TxDrop = txDrop 
    member _.TxErrs = txErrs 
    member _.TxFifo = txFifo 
    member _.TxFrame = txFrame 
    member _.TxMulticast = txMulticast 
    member _.TxPackets = txPackets
        
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
    let d = Dictionary<string, NetDeviceInfo>()

    use sr = new StreamReader("/proc/net/dev")
    
    // 2 header lines - throw them away
    sr.ReadLine() |> ignore
    sr.ReadLine() |> ignore

    while not sr.EndOfStream do
        let line = sr.ReadLine()

        if (line = null) then
            ()
        else
            let columns = Regex.Split(line.Trim(), "\\s+")

            let mutable adapterName = columns.[0]
            adapterName <- adapterName.Remove(adapterName.Length - 1)

            let rxBytes = Int64.Parse(columns.[1])
            let rxPackets = Int64.Parse(columns.[2])
            let rxErrs = Int64.Parse(columns.[3])
            let rxDrop = Int64.Parse(columns.[4])
            let rxFifo = Int64.Parse(columns.[5])
            let rxFrame = Int64.Parse(columns.[6])
            let rxCompressed = Int64.Parse(columns.[7])
            let rxMulticast = Int64.Parse(columns.[8])

            let txBytes = Int64.Parse(columns.[9])
            let txPackets = Int64.Parse(columns.[10])
            let txErrs = Int64.Parse(columns.[11])
            let txDrop = Int64.Parse(columns.[12])
            let txFifo = Int64.Parse(columns.[13])
            let txFrame = Int64.Parse(columns.[14])
            let txCompressed = Int64.Parse(columns.[15])
            let txMulticast = Int64.Parse(columns.[16])

            d.Add(adapterName,
                NetDeviceInfo(rxBytes, rxPackets, rxErrs, rxDrop, rxFifo, rxFrame, rxCompressed,
                    rxMulticast, txBytes, txPackets, txErrs, txDrop, txFifo, txFrame, txCompressed,
                    txMulticast))

    d
    
let getCpuMeasurements(): Dictionary<string, CpuMeasurement> =
     let d = Dictionary<string, CpuMeasurement>()

     use sr = new StreamReader("/proc/stat")
     
     while not sr.EndOfStream do
         let line = sr.ReadLine()

         if line = null || (not (line.StartsWith("cpu"))) then
            () // todo: return early?
         else
             let columns = Regex.Split(line, "\\s+")

             let cpuIdx = columns.[0]

             let totalTime = columns |> Seq.tail |> Seq.sumBy Int64.Parse
             let idleTime = Int64.Parse(columns.[4])

             d.Add(cpuIdx, CpuMeasurement(totalTime, idleTime))

     d

let getMemoryMeasurements():  Dictionary<string, int64> =
     let d = Dictionary<string, int64>()

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
                 d.Add(regexMatch.Groups.[1].Value, Int64.Parse(regexMatch.Groups.[2].Value) * (int64 1000))

     d
 
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