namespace VMS.TPS

open System
open System.IO
open System.Windows.Forms
open FsToolkit.ErrorHandling

module Utils =

    // Path to the log file
    let logFilePath: string = "log.txt"

    // Function to log a message to the log file with a timestamp
    let log (msg: string) : unit =
        let timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        let logMessage = sprintf "[%s] %s" timestamp msg
        // Append the log message to the file
        File.AppendAllText(logFilePath, logMessage + Environment.NewLine)

    let warn (msg: string) : unit = 
        MessageBox.Show(msg) 
        |> ignore

    let warnAndQuit (msg: string) : unit = 
        MessageBox.Show(msg) 
        |> failwith msg

    let logAndWarn (msg: string) : unit =
        msg |> log
        msg |> warnAndQuit

    let roundTo1 (f: float) : float = Math.Round(f, 1)

    let roundTo0 (f: float) : float = Math.Round(f, 0)
    
    let tryMe (expr: 'a) (errorMsg: string) : 'a =
        try
            expr
        with ex ->
            logAndWarn errorMsg
            raise ex // Re-throw the exception after handling it
