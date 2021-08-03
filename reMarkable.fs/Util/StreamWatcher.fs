namespace reMarkable.fs.Util

open System
open System.IO

/// Provides a set of methods that allow a stream to be watched for new data to be written,
///   and to have events dispatched when new data is available and parsed from the stream
/// 
/// <param name="stream">The stream from which data is read</param>
/// <param name="bufferSize">The exact size of the type to read</param>
type StreamWatcher<'T when 'T : struct and 'T: (new: unit -> 'T)>(stream: Stream, bufferSize: int) =

    /// The buffer into which data is read
    let buffer: byte array =
        Array.zeroCreate bufferSize
        
    let dataAvailable = Event<DataAvailableEventArgs<'T>>()
    
    /// Fires the DataAvailable event with the specified data arguments
    /// <param name="e">The data argument to pass to the event handlers</param>
    let OnDataAvailable(e: DataAvailableEventArgs<'T>) =
        dataAvailable.Trigger e
        ()

    /// Process the data when the read buffer is saturated, which is when one struct has been read
    /// <param name="ar">The async stream operation result</param>
    let rec readCallback(ar: IAsyncResult): unit =
        let bytesRead = stream.EndRead(ar)
        
        if (bytesRead <> buffer.Length) then
            raise <| InvalidOperationException("Buffer underflow");
        
        OnDataAvailable(new DataAvailableEventArgs<'T>(BufferExtensions.ToStruct<'T>(buffer)))
        
        watchNext()
    
    /// Wait for the next data to appear on the stream
    and watchNext(): unit =
        stream.BeginRead(buffer, 0, buffer.Length, AsyncCallback(readCallback), null)
        |> ignore
    
    do
        if stream = null then
            failwith "nooo"
        
        watchNext() // as part of construction
    
    
    /// Fired when new data has been parsed from the stream
    member this.DataAvailable = dataAvailable.Publish
    
    interface IDisposable with
        member _.Dispose() = stream.Dispose()
