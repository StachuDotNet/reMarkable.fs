namespace reMarkable.fs.Unix.Driver.Performance

open System

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

    /// Gets the total amount of free memory
    /// <returns> The amount of free memory, in bytes </returns>
    abstract member GetFreeMemory: unit-> int64

    /// Gets the total amount of free swap
    /// <returns> The amount of free swap, in bytes </returns>
    abstract member GetFreeSwap: unit -> int64

    /// Lists all of the available network adapters
    abstract member GetNetworkAdapters: unit -> string array

    /// Gets the instantaneous network download speed
    /// <param name="adapter">The adapter to query</param>
    /// <returns> The speed in bytes/second </returns>
    abstract member GetNetworkRxSpeed: string -> int64

    /// Gets the total network download utilization
    /// <param name="adapter">The adapter to query</param>
    /// <returns> The utilization in bytes </returns>
    abstract member GetNetworkRxTotal: string -> int64

    /// Gets the instantaneous network upload speed
    /// <param name="adapter">The adapter to query</param>
    /// <returns> The speed in bytes/second </returns>
    abstract member GetNetworkTxSpeed: string -> int64

    /// Gets the total network upload utilization
    /// <param name="adapter">The adapter to query</param>
    /// <returns> The utilization in bytes </returns>
    abstract member GetNetworkTxTotal: string -> int64

    /// Gets the total processor utilization
    /// <returns> The percentage utilization, from 0-1 </returns>
    abstract member GetProcessorTime: unit -> float32

    /// Gets a specific processor's utilization
    /// <param name="processor">The processor to query</param>
    /// <returns> The percentage utilization, from 0-1 </returns>
    abstract member GetProcessorTime: int -> float32

    /// Gets a specific core's utilization
    /// <param name="processor">The processor to query</param>
    /// <param name="core">The core to query</param>
    /// <returns> The percentage utilization, from 0-1 </returns>
    abstract member GetProcessorTime: (int * int) -> float32
    

/// Contains data associated with an instantaneous CPU measurement
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
        let time = DateTime.Now;

        let dT = time - _previousTime
        let dM = measurement - _previousValue

        _previousTime <- time
        _previousValue <- measurement

        dM / dT.TotalSeconds

    
    
// /// Provides a set of methods to profile hardware performance metrics
// public class HardwarePerformanceMonitor : IPerformanceMonitor
// {
//     /// Contains the instantaneous CPU time-based measurements
//     private readonly Dictionary<string, PerformanceMeasurement> _cpuMeasurements;
//
//     /// Contains the instantaneous network time-based measurements
//     private readonly Dictionary<string, PerformanceMeasurement> _networkMeasurements;
//
//     public int NumberOfCores { get; }
//
//     public int NumberOfProcessors { get; }
//
//     public long TotalMemory { get; }
//
//     public long TotalSwap { get; }
//
//     public HardwarePerformanceMonitor()
//     {
//         _cpuMeasurements = new Dictionary<string, PerformanceMeasurement>();
//         var cpuMeasurements = GetCpuMeasurements();
//
//         foreach (var (key, _) in cpuMeasurements)
//         {
//             _cpuMeasurements.Add($"idle-{key}", new PerformanceMeasurement());
//             _cpuMeasurements.Add($"total-{key}", new PerformanceMeasurement());
//         }
//
//         NumberOfProcessors = 1;
//         NumberOfCores = cpuMeasurements.Count - 1;
//
//         var memoryMeasurements = GetMemoryMeasurements();
//
//         TotalMemory = memoryMeasurements["MemTotal"];
//         TotalSwap = memoryMeasurements["SwapTotal"];
//
//         _networkMeasurements = new Dictionary<string, PerformanceMeasurement>();
//         var networkMeasurements = GetNetworkMeasurements();
//
//         foreach (var (key, _) in networkMeasurements)
//         {
//             _networkMeasurements.Add($"tx-{key}", new PerformanceMeasurement());
//             _networkMeasurements.Add($"rx-{key}", new PerformanceMeasurement());
//         }
//     }
//
//     public long GetFreeMemory()
//     {
//         var memoryMeasurements = GetMemoryMeasurements();
//         return memoryMeasurements["MemAvailable"];
//     }
//
//     public long GetFreeSwap()
//     {
//         var memoryMeasurements = GetMemoryMeasurements();
//         return memoryMeasurements["SwapFree"];
//     }
//
//     public string[] GetNetworkAdapters()
//     {
//         return GetNetworkMeasurements().Keys.ToArray();
//     }
//
//     public long GetNetworkRxSpeed(string adapter)
//     {
//         var nowBytes = GetNetworkMeasurements()[adapter].RxBytes;
//         var measurement = _networkMeasurements[$"rx-{adapter}"].PushMeasurementPerSecond(nowBytes);
//
//         return (long)measurement;
//     }
//
//     public long GetNetworkRxTotal(string adapter)
//     {
//         return GetNetworkMeasurements()[adapter].RxBytes;
//     }
//
//     public long GetNetworkTxSpeed(string adapter)
//     {
//         var nowBytes = GetNetworkMeasurements()[adapter].TxBytes;
//         var measurement = _networkMeasurements[$"tx-{adapter}"].PushMeasurementPerSecond(nowBytes);
//
//         return (long)measurement;
//     }
//
//     public long GetNetworkTxTotal(string adapter)
//     {
//         return GetNetworkMeasurements()[adapter].TxBytes;
//     }
//
//     public float GetProcessorTime()
//     {
//         var cpuMeasurements = GetCpuMeasurements();
//         var cpuTotal = cpuMeasurements["cpu"];
//
//         var idle = _cpuMeasurements["idle-cpu"].PushMeasurement(cpuTotal.Idle);
//         var total = _cpuMeasurements["total-cpu"].PushMeasurement(cpuTotal.Total);
//
//         return (float)(1 - idle / total);
//     }
//
//     public float GetProcessorTime(int processor)
//     {
//         if (processor != 0)
//             throw new ArgumentException(nameof(processor));
//
//         return GetProcessorTime();
//     }
//
//     public float GetProcessorTime(Tuple<int, int> a)
//     {
//         var (processor, core) = a;
//         
//         if (processor != 0)
//             throw new ArgumentException(nameof(processor));
//
//         var cpuMeasurements = GetCpuMeasurements();
//         var cpuTotal = cpuMeasurements["cpu"];
//
//         var idle = _cpuMeasurements[$"idle-cpu{core}"].PushMeasurement(cpuTotal.Idle);
//         var total = _cpuMeasurements[$"total-cpu{core}"].PushMeasurement(cpuTotal.Total);
//
//         return (float)(1 - idle / total);
//     }
//
//     private static Dictionary<string, CpuMeasurement> GetCpuMeasurements()
//     {
//         var d = new Dictionary<string, CpuMeasurement>();
//
//         using var sr = new StreamReader("/proc/stat");
//         
//         while (!sr.EndOfStream)
//         {
//             var line = sr.ReadLine();
//
//             if (line == null || !line.StartsWith("cpu"))
//                 break;
//
//             var columns = Regex.Split(line, "\\s+");
//
//             var cpuIdx = columns[0];
//
//             var totalTime = columns.Skip(1).Aggregate(0L, (i, s) => i + long.Parse(s));
//             var idleTime = long.Parse(columns[4]);
//
//             d.Add(cpuIdx, new CpuMeasurement(totalTime, idleTime));
//         }
//
//         return d;
//     }
//
//     private static Dictionary<string, long> GetMemoryMeasurements()
//     {
//         var d = new Dictionary<string, long>();
//
//         using var sr = new StreamReader("/proc/meminfo");
//         
//         while (!sr.EndOfStream)
//         {
//             var line = sr.ReadLine();
//
//             if (line == null)
//                 continue;
//
//             var match = Regex.Match(line, "(.+):\\s+(\\d+) kB");
//
//             if (!match.Success)
//                 continue;
//
//             // Kilobytes
//             d.Add(match.Groups[1].Value, long.Parse(match.Groups[2].Value) * 1000);
//         }
//
//         return d;
//     }
//
//     private static Dictionary<string, NetDeviceInfo> GetNetworkMeasurements()
//     {
//         var d = new Dictionary<string, NetDeviceInfo>();
//
//         using var sr = new StreamReader("/proc/net/dev");
//         
//         // 2 header lines - throw them away
//         sr.ReadLine();
//         sr.ReadLine();
//
//         while (!sr.EndOfStream)
//         {
//             var line = sr.ReadLine();
//
//             if (line == null)
//                 continue;
//
//             var columns = Regex.Split(line.Trim(), "\\s+");
//
//             var adapterName = columns[0];
//             adapterName = adapterName.Remove(adapterName.Length - 1);
//
//             var rxBytes = long.Parse(columns[1]);
//             var rxPackets = long.Parse(columns[2]);
//             var rxErrs = long.Parse(columns[3]);
//             var rxDrop = long.Parse(columns[4]);
//             var rxFifo = long.Parse(columns[5]);
//             var rxFrame = long.Parse(columns[6]);
//             var rxCompressed = long.Parse(columns[7]);
//             var rxMulticast = long.Parse(columns[8]);
//
//             var txBytes = long.Parse(columns[9]);
//             var txPackets = long.Parse(columns[10]);
//             var txErrs = long.Parse(columns[11]);
//             var txDrop = long.Parse(columns[12]);
//             var txFifo = long.Parse(columns[13]);
//             var txFrame = long.Parse(columns[14]);
//             var txCompressed = long.Parse(columns[15]);
//             var txMulticast = long.Parse(columns[16]);
//
//             d.Add(adapterName,
//                 new NetDeviceInfo(rxBytes, rxPackets, rxErrs, rxDrop, rxFifo, rxFrame, rxCompressed,
//                     rxMulticast, txBytes, txPackets, txErrs, txDrop, txFifo, txFrame, txCompressed,
//                     txMulticast));
//         }
//
//         return d;
//     }
// }