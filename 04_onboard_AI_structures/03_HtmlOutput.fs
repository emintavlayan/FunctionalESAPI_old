namespace VMS.TPS

open System.IO
open System.Diagnostics

module HtmlOutput =

    /// Unified result type for the module
    type HtmlOutputResult<'T> = Result<'T, HtmlOutputError>
    
    and HtmlOutputError =
        | HtmlWriteError of string
        | FileAppendError of string
        | ProcessError of string
    
    /// HTML header template
    let private htmlHeader : string = 
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
    let private htmlFooter : string = "</table></body></html>"

    // Configurable file path and name
    let tempPath : string = Path.GetTempPath()
    let htmlFileName : string = "output.html"
    let htmlFilePath : string = Path.Combine(tempPath, htmlFileName)
    
    /// Initializes the HTML output by creating or overwriting the HTML file with the header
    let writeHeaderToHtml (path: string) : HtmlOutputResult<unit> =
        try
            File.WriteAllText(path, htmlHeader)
            Ok ()
        with
        | ex -> Error (HtmlWriteError $"Failed to write to file: {ex.Message}")
    
    /// Appends a table row to the HTML file
    let appendStructureOperationResultsToHtmlTable (path: string) (aiId, rhId, message) : HtmlOutputResult<unit> =
        let row = sprintf "<tr><td>%s</td><td>%s</td><td>%s</td></tr>" aiId rhId message
        try
            File.AppendAllText(path, row)
            Ok ()
        with
        | ex -> Error (FileAppendError $"Failed to append to file: {ex.Message}")
    
    /// Finalizes the HTML output by closing HTML tags
    let writeFooterToHtml (path: string) : HtmlOutputResult<unit> =
        try
            File.WriteAllText(path, htmlFooter)
            Ok ()
        with
        | ex -> Error (HtmlWriteError $"Failed to write to file: {ex.Message}")
    
    /// Displays the Html with the default browser.
    let displayHtml (path: string) : HtmlOutputResult<unit> =
        try
            let psi = ProcessStartInfo(FileName = path, UseShellExecute = true)
            Process.Start(psi) |> ignore
            Ok ()
        with
        | ex -> Error (ProcessError $"Failed to start process: {ex.Message}")