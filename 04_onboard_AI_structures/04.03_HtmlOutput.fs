namespace VMS.TPS

open System.IO
open System.Diagnostics
open FsToolkit.ErrorHandling
open System.Windows.Forms
open VMS.TPS.Common.Model.Types
open VMS.TPS.Common.Model.API

/// Unified result type for the module
type HtmlOutputResult<'T> = Result<'T, HtmlOutputError>

and HtmlOutputError =
    | HtmlWriteError of string
    | FileAppendError of string
    | ProcessError of string
    
module HtmlOutputError =
    let message =
        function
        | HtmlWriteError e -> $"Failed to write to HTML: {e}"
        | FileAppendError e -> $"Failed to append to HTML: {e}"
        | ProcessError e -> $"Failed to start HTML viewer: {e}"
    

module HtmlOutput =

    /// HTML header template
    let createHtmlHeader (structureSetId : string ): string = 
        sprintf """
        <!DOCTYPE html>
        <html>
        <head>
            <title>Volume Move Results </title>
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
            <h2>StructureSet: %s</h2>
            <table>
                <tr><th>AI Structure ID</th><th>RH Structure ID</th><th>Operation Result Message</th></tr>
        """ structureSetId
    
    /// HTML footer template
    let private htmlFooter : string = "</table></body></html>"
    
    /// Initializes the HTML output by creating or overwriting the HTML file with the header
    let writeHeaderToHtml (ssId: string) (path: string) =
        try
            File.WriteAllText(path, createHtmlHeader ssId)
            Ok ()
        with
        | ex -> Error ex.Message
    
    /// Appends a table row to the HTML file
    let appendStructureOperationResultsToHtmlTable (path: string) (aiId, rhId, message: StructureOperationResult<string> ) =
        let message =
            message |> Result.defaultWith (fun error -> sprintf "%A" error )
        
        let row = sprintf "<tr><td>%s</td><td>%s</td><td>%s</td></tr>" aiId rhId message
        try
            File.AppendAllText(path, row)
            Ok ()
        with
        | ex -> Error  ex.Message
           
    /// Finalizes the HTML output by closing HTML tags
    let writeFooterToHtml (path: string) =
        try
            File.AppendAllText(path, htmlFooter)
            Ok ()
        with
        | ex -> Error ex.Message
    
    /// Displays the Html with the default browser.
    let displayHtml (path: string) =
        try
            let psi = ProcessStartInfo(FileName = path, UseShellExecute = true)
            Process.Start(psi) |> ignore
            Ok ()
        with
        | ex -> Error  ex.Message
   

    let writeHtml htmlFilePath ssid (structureCopyResults: (string*string*_) list) = result {
            // Write the HTML header to start the output file
            do! writeHeaderToHtml htmlFilePath ssid |> Result.mapError HtmlOutputError.HtmlWriteError
            
            for copyResult in structureCopyResults do
                do!
                    appendStructureOperationResultsToHtmlTable htmlFilePath copyResult
                    |> Result.mapError HtmlOutputError.FileAppendError
                
            // Write the HTML footer to finalize the output file
            do! writeFooterToHtml(htmlFilePath) |> Result.mapError HtmlOutputError.FileAppendError

            // Display the HTML output file in the default web browser
            do! displayHtml(htmlFilePath) |> Result.mapError HtmlOutputError.ProcessError
        }