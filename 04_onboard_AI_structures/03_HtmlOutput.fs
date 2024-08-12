namespace VMS.TPS

open System
open System.IO
open System.Diagnostics

module HtmlOutput =

    /// Result type for HTML output operations
    type HtmlOutputResult<'T> = Result<'T, HtmlOutputError>

    and HtmlOutputError =
        | HtmlWriteError of string
        | FileAppendError of string
        | ProcessError of string

    /// Creates an HTML header with a custom title
    let private htmlHeader = 
        sprintf """
        <!DOCTYPE html>
        <html>
        <head>
            <title>Volume Move Results</title>
            <style>
                body {
                    margin-left: 5%%;
                    margin-right: 5%%;
                    font-family: sans-serif;
                }
                h1 {
                    display: block;
                    font-size: 2em;
                    margin-block-start: 0.67em;
                    margin-block-end: 0.67em;
                    margin-inline-start: 0px;
                    margin-inline-end: 0px;
                    font-weight: bold;
                    margin-left: -3%%;
                }
                table {
                    font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;
                    border-collapse: collapse;
                    width: 100%%;
                }
                th, td {
                    border: 1px solid blue;
                    text-align: left;
                    padding: 8px;
                }
                th {
                    background-color: lightgray;
                }
            </style>
        </head>
        <body>
            <h1>Volume Move Results</h1>
            <table>
                <tr><th>AI Structure ID</th><th>RH Structure ID</th><th>Operation Result Message</th></tr>
        """
    
    /// HTML footer template
    let private htmlFooter = "</table></body></html>"
    
    /// Configurable file path and name
    let private tempPath = Path.GetTempPath()
    let private htmlFileName = "output.html"
    let private htmlFilePath = Path.Combine(tempPath, htmlFileName)
    
    /// Writes content to a file
    let private writeFileToTemp (path: string) (content: string) : HtmlOutputResult<unit> =
        try
            File.WriteAllText(path, content)
            Ok ()
        with
        | ex -> Error (HtmlWriteError $"Failed to write to file: {ex.Message}")
    
    /// Appends content to a file
    let private appendToFile (path: string) (content: string) : HtmlOutputResult<unit> =
        try
            File.AppendAllText(path, content)
            Ok ()
        with
        | ex -> Error (FileAppendError $"Failed to append to file: {ex.Message}")

    /// Starts a process to display the HTML file
    let private startProcess (path: string) : HtmlOutputResult<unit> =
        try
            let psi = ProcessStartInfo(FileName = path, UseShellExecute = true)
            Process.Start(psi) |> ignore
            Ok ()
        with
        | ex -> Error (ProcessError $"Failed to start process: {ex.Message}")
    
    /// Initializes the HTML output by creating or overwriting the HTML file with the header
    let initializeHtml () =
        htmlHeader 
        |> writeFileToTemp htmlFilePath
        |> ignore
    
    /// Appends a table row to the HTML file
    let appendTableRow (aiId, rhId, message) =
        let row = sprintf "<tr><td>%s</td><td>%s</td><td>%s</td></tr>" aiId rhId message
        appendToFile htmlFilePath row
    
    /// Finalizes the HTML output by closing HTML tags and opening the file in a browser
    let finalizeAndDisplayHtml () : HtmlOutputResult<unit> =
        appendToFile htmlFilePath htmlFooter
        |> Result.bind (fun _ -> startProcess htmlFilePath)