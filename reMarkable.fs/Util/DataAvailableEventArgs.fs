namespace reMarkable.fs.Util

open System

/// Contains data related to data availability events raised by monitored streams
/// <typeparam name="T">The type of data referenced in the event</typeparam>
type DataAvailableEventArgs<'T>(data: 'T) =
    inherit EventArgs()
    
    /// The data referenced by the event
    member _.Data = data