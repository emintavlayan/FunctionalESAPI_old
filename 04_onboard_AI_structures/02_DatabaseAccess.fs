namespace VMS.TPS

open TableParser
open Newtonsoft.Json.Linq
open System.Net.Http

/// Module for accessing and parsing data from a database.
module DatabaseAccess =

    /// Represents the result of database access attempt.
    type DatabaseAccessResult =
        | TableParseResult of TableParseResult
        | UnexpectedHttpResponse of int
        | JsonParsingError of string
        | HtmlParsingError of string
        | ProcessError of string
        | UnexpectedError of string


    // makes a HTTP request to a URL and returns the response content
    let executeHttpRequest (url: string) =
         use client = new HttpClient()

         let response = 
            client.GetAsync(url)
            |> Async.AwaitTask
            |> Async.RunSynchronously

         if response.IsSuccessStatusCode then
            response.Content.ReadAsStringAsync()
            |> Async.AwaitTask
            |> Async.RunSynchronously
            |> Ok
         else
            Error response.StatusCode
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
        match executeHttpRequest url with
        | Ok output ->
            match parseHtmlFromJson output with
            | Some htmlContent ->
                let parseResult = parseTableToTuples htmlContent
                TableParseResult parseResult
            | None ->
                JsonParsingError "Failed to parse JSON from the output."
        | Error e -> e |> int |> UnexpectedHttpResponse
