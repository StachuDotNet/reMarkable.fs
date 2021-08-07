/// Provides a source for creating loggers
module reMarkable.fs.Logger

open NLog
open NLog.Config
open NLog.Layouts
open NLog.Targets

/// Creates a logger with the specified header
let CreateLogger(header: string): Logger =
    let config = LoggingConfiguration()

    let layout = SimpleLayout("[${longdate}] [${threadname}/${level:uppercase=true}] [${logger}] ${message}")

    let logfile = new FileTarget("logfile")
    logfile.FileName <- Layout.FromString "output.log"
    logfile.Layout <- layout

    let logconsole = new ConsoleTarget("logconsole")
    logconsole.Layout <- layout

    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logconsole)
    config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile)

    LogManager.Configuration <- config
    
    LogManager.GetLogger(header);
