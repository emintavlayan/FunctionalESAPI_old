namespace VMS.TPS

open TableParser
open System.Diagnostics
open Newtonsoft.Json.Linq

/// Module for accessing and parsing data from a database.
module DatabaseAccess =

    /// Represents the result of database access attempt.
    type DatabaseAccessResult =
        | TableParseResult of TableParseResult
        | CurlNotInstalled
        | JsonParsingError of string
        | HtmlParsingError of string
        | ProcessError of string
        | UnexpectedError of string

    /// Sets up and starts a process with given parameters.
    let startProcess (fileName: string) (arguments: string) =
        let startInfo = ProcessStartInfo(
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        )
        try
            Some(Process.Start(startInfo))
        with
        | :? System.ComponentModel.Win32Exception as ex ->
            None 

    /// Executes curl and returns the output.
    let executeCurl url =
        let arguments = sprintf "-s \"%s\"" url
        match startProcess "curl" arguments with
        | Some myProcess ->
            let output = myProcess.StandardOutput.ReadToEnd()
            myProcess.WaitForExit()
            Some output
        | None -> None

    /// Parses HTML content from JSON.
    let parseHtmlFromJson output =
        try
            let json = JObject.Parse(output)
            let htmlContent = json.SelectToken("parse.text.*").Value<string>()
            Some htmlContent
        with
        | :? Newtonsoft.Json.JsonReaderException as ex ->
            None

    /// Main function to get data from a URL.
    let getData (url: string) : DatabaseAccessResult =
        match executeCurl url with
        | Some output ->
            match parseHtmlFromJson output with
            | Some htmlContent ->
                let parseResult = parseTableToTuples htmlContent
                TableParseResult parseResult
            | None ->
                JsonParsingError "Failed to parse JSON from the output."
        | None -> CurlNotInstalled
