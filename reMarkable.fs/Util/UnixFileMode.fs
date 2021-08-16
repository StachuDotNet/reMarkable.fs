namespace reMarkable.fs

/// Flags a Unix file stream can be opened with
type UnixFileMode =
    | ReadOnly = 0
    | WriteOnly = 1
    | ReadWrite = 2