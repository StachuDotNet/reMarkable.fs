module reMarkable.fs.Device

open System
open System.IO
open System.Text.RegularExpressions
open System.Collections.Generic
open System.Linq

/// Possible Remarkable or emulated device types
type Device = RM1 | RM2

/// Parses `/proc/bus/input/devices` to create a mapped dictionary of input device names and their event streams
/// 
/// Returns: A <see cref="Dictionary{TKey,TValue}" /> mapping device names to handler event stream filenames
let GetInputDeviceEventHandlers(): Dictionary<string, string> =
    use r = new StreamReader("/proc/bus/input/devices")
    let nameRegex = Regex("^N: Name=\"(?<name>.+)\"$", RegexOptions.Compiled)
    let handlersRegex = Regex("^H: Handlers=(?<handlers>.+)$", RegexOptions.Compiled)

    let deviceMap = Dictionary<string, string>()

    let mutable currentDevice: string = null

    while not r.EndOfStream do
        let line = r.ReadLine()
        
        if String.IsNullOrWhiteSpace line then
            ()
        else
            match line.[0] with
            | 'N' -> // found a new device
                let matchedThing = nameRegex.Match(line)
                if not matchedThing.Success then
                    raise <| InvalidDataException("Unexpected name formatting in /proc/bus/input/devices");

                currentDevice <- matchedThing.Groups.["name"].Value
            | 'H' ->
                if currentDevice = null then
                    raise <| InvalidDataException("Unexpected handlers entry without device");

                let matchedThing = handlersRegex.Match(line)
                if not matchedThing.Success then
                    raise <| InvalidDataException("Unexpected handlers formatting in /proc/bus/input/devices");

                let handlers = matchedThing.Groups.["handlers"].Value.Split(' ')

                let eventHandler = handlers.FirstOrDefault(fun s -> s.StartsWith("event"))

                deviceMap.Add(currentDevice, $"/dev/input/{eventHandler}")

                currentDevice <- null
            | _ -> ()

    deviceMap