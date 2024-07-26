module VMS.TPS.DatabaseAccess

open TableParser
open Newtonsoft.Json.Linq
open System.Net.Http
open Util

/// Module for accessing and parsing data from a database.

type DataBaseAccessError =
    | UnexpectedHttpResponse of int
    | JsonParsingError
    | HtmlParsingError of string
    | ProcessError of string
    | UnexpectedError of string
    | TableParseError of TableParseError

/// Represents the result of database access attempt.
type DatabaseAccessResult = Result<(string * string) list, DataBaseAccessError>


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
    url
    |> (executeHttpRequest >> Result.mapError (int >> UnexpectedHttpResponse))
    |> Result.bind (parseHtmlFromJson >> Result.ofOption JsonParsingError)
    |> Result.bind (parseTableToTuples >> (Result.mapError TableParseError))

/// Example of how it would look like with a result CE (FsToolkit.ErrorHandling)
// let getData url = result {

//     let! httpResult = (executeHttpRequest >> Result.mapError (int >> UnexpectedHttpResponse))
//     let! parsedData = 
// }