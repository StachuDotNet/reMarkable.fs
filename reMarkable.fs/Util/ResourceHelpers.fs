module reMarkable.fs.ResourceHelpers

open System.IO
open System.Reflection

let getFileInBytes (assembly: Assembly) resourceName =
    assembly.Location
    |> Path.GetDirectoryName
    |> fun dirName -> Path.Combine(dirName, "Resources", resourceName)
    |> File.ReadAllBytes