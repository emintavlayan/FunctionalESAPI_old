namespace VMS.TPS

open System
open System.IO
open System.Diagnostics

module HtmlOutput =
    
    let private htmlHeader = 
        """
        <!DOCTYPE html>
        <html>
        <head>
            <title>Volume Move Results</title>
            <style>
                body {
                    margin-left: 5%;
                    margin-right: 5%;
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
                    margin-left: -3%;
                }
                table {
                    font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;
                    border-collapse: collapse;
                    width: 100%;
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
            <h1>Results</h1>
            <table>
                <tr><th>AI Structure ID</th><th>RH Structure ID</th><th>Operation Result Message</th></tr>
        """

    let private htmlFooter = "</table></body></html>"
    let private tempPath = Path.GetTempPath()
    let private htmlFileName = "output.html"
    let private htmlFilePath = Path.Combine(tempPath, htmlFileName)

    /// Initializes the HTML output by creating or overwriting the HTML file with the header.
    let initializeHtml () =
        File.WriteAllText(htmlFilePath, htmlHeader)

    /// Appends a table row to the HTML file.
    let appendTableRow (aiId, rhId, message) =
        let row = sprintf "<tr><td>%s</td><td>%s</td><td>%s</td></tr>" aiId rhId message
        File.AppendAllText(htmlFilePath, row)

    /// Finalizes the HTML output by closing HTML tags and opening the file in a browser.
    let finalizeAndDisplayHtml () =
        File.AppendAllText(htmlFilePath, htmlFooter)
        let psi = ProcessStartInfo(FileName = htmlFilePath, UseShellExecute = true)
        Process.Start(psi) |> ignore
