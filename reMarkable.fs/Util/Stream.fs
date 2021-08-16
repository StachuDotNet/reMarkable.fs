module reMarkable.fs.Util.Stream

open System
open System.Runtime.ConstrainedExecution
open System.Runtime.InteropServices
open System.Security.Permissions
open System.Diagnostics.CodeAnalysis
open reMarkable.fs

[<ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>]
[<DllImport("libc", EntryPoint = "close", SetLastError = true)>]
extern int Close(IntPtr handle)


/// Provides a safe handle to a device that can be have IOCTL commands issued to it
[<SecurityPermission(SecurityAction.InheritanceDemand, UnmanagedCode = true)>]
[<SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)>]
[<ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>]
type SafeUnixHandle() as this =
    inherit SafeHandle(IntPtr(-1), true)

    override _.IsInvalid =
        this.handle = IntPtr(-1)

    [<ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)>]
    override _.ReleaseHandle() =
        Close(this.handle) <> -1


[<DllImport("libc", EntryPoint = "open", SetLastError = true)>]
[<SuppressMessage(
     "Globalization", 
     "CA2101:Specify marshaling for P/Invoke string arguments", 
     Justification = "Specifying a marshaling breaks rM compatibility"
 )>]
extern SafeUnixHandle Open(string path, uint flags, UnixFileMode mode)
