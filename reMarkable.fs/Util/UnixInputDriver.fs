namespace reMarkable.fs.UnixInput

open System
open System.IO
open System.Runtime.InteropServices

/// Provides extension methods that operate on byte arrays as buffers
module BufferExtensions = 
    /// Parses a struct from a byte stream
    /// 
    /// <typeparam name="T">The struct type to read</typeparam>
    /// <param name="buffer">The buffer from which to read the struct</param>
    /// <returns>A populated struct of type <typeparamref name="T" /></returns>
    let ToStruct<'T when 'T : struct and 'T : (new: unit -> 'T)>(buffer: byte[]): 'T = 
        let temp = new 'T()
        let size = Marshal.SizeOf(temp)
        let ptr = Marshal.AllocHGlobal(size)

        Marshal.Copy(buffer, 0, ptr, size)

        let result = Marshal.PtrToStructure(ptr, temp.GetType()) :?> 'T
        
        Marshal.FreeHGlobal(ptr)

        result

// Without this #nowarn, we get a compilation warning on EvEvent:
// "Uses of this construct may result in the generation of unverifiable .NET IL code."
#nowarn "9" 

/// Interchange format for an input device event
/// See https://www.kernel.org/doc/Documentation/input/event-codes.txt for details around Type and Code meanings.
[<StructLayout(LayoutKind.Explicit, Size = 16)>]
type EvEvent =
    struct
        /// The epoch timestamp whole seconds component
        [<FieldOffset(0)>]
        val TimeWholeSeconds: uint16

        /// The epoch timestamp fractional seconds component, in microseconds
        [<FieldOffset(4)>]
        val TimeFractionMicroseconds: uint

        /// The application-specific event type
        [<FieldOffset(8)>]
        val Type: uint16

        /// The application-specific event code
        [<FieldOffset(10)>]
        val Code: uint16

        /// The application-specific event value
        [<FieldOffset(12)>]
        val Value: int
    end

/// Contains data related to data availability events raised by monitored streams
type DataAvailableEventArgs<'T>(data: 'T) =
    inherit EventArgs()
    
    /// The data referenced by the event
    member _.Data = data


/// A set of methods that allow a stream to be watched for new data to be written,
///   and to have events dispatched when new data is available and parsed from the stream.
/// 
/// <param name="stream">The stream from which data is read</param>
/// <param name="bufferSize">The exact size of the type to read</param>
type StreamWatcher<'T when 'T : struct and 'T: (new: unit -> 'T)>(stream: Stream, bufferSize: int) =
    /// The buffer into which data is read
    let buffer: byte array = Array.zeroCreate bufferSize
        
    let dataAvailable = Event<DataAvailableEventArgs<'T>>()
    
    /// Process the data when the read buffer is saturated, which is when one struct has been read
    /// <param name="ar">The async stream operation result</param>
    let rec readCallback(ar: IAsyncResult): unit =
        let bytesRead = stream.EndRead(ar)
        
        if bytesRead <> buffer.Length then
            raise <| InvalidOperationException("Buffer underflow")
        
        dataAvailable.Trigger(new DataAvailableEventArgs<'T>(BufferExtensions.ToStruct<'T>(buffer)))
        watchNext()
    /// Wait for the next data to appear on the stream
    and watchNext(): unit =
        stream.BeginRead(buffer, 0, buffer.Length, AsyncCallback(readCallback), null)
        |> ignore
    
    do
        if stream = null then
            failwith "Unexpected: null stream"
        
        watchNext()
    
    /// Fired when new data has been parsed from the stream
    member this.DataAvailable = dataAvailable.Publish
    
    interface IDisposable with
        member _.Dispose() = stream.Dispose()


/// Provides a base set of methods that allow an input device event stream to be parsed and delegated to event handlers
/// <param name="device">The device event stream file to poll for new events</param>
[<AbstractClass>]
type UnixInputDriver(devicePath: string) as this =
    /// The continuous stream parser
    let eventWatcher = new StreamWatcher<EvEvent>(File.OpenRead(devicePath), 16)
    
    do
        eventWatcher.DataAvailable.Add(this.DataAvailable)
    
    /// The device event stream file to poll for new events
    member _.Device = devicePath

    /// Called when a new event has been parsed from the event stream
    abstract member DataAvailable: DataAvailableEventArgs<EvEvent> -> unit
    
    interface IDisposable with
        member this.Dispose(): unit =
            (eventWatcher :> IDisposable).Dispose()
            GC.SuppressFinalize(this)