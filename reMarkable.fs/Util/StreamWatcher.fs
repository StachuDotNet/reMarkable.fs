namespace reMarkable.fs.Util

open System
open System.IO


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
