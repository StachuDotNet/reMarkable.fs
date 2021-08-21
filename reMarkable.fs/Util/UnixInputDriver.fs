namespace reMarkable.fs.UnixInput

open System
open System.IO
open reMarkable.fs.Util
open System.Runtime.InteropServices

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