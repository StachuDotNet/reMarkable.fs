/// Provides extension methods that operate on byte arrays as buffers
module reMarkable.fs.Util.BufferExtensions

open System.Runtime.InteropServices

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