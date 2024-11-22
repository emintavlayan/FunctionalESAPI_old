namespace VMS.TPS

open HtmlAgilityPack
open System
open Newtonsoft.Json.Linq
open System.Net.Http
open FsToolkit.ErrorHandling

open System.Threading.Tasks

/// Unified result type for the module
type FetchResult<'T> = Result<'T, FetchError>

and FetchError =
    | HttpRequestFailed of string
    | JsonParsingFailed of string
    | TableNotFound of string
    | RowsNotFound
    | CellExtractionFailed
    
module FetchError =
    
    let message =
        function
        | HttpRequestFailed message -> $"Http request failed: {message}"
        | JsonParsingFailed message -> $"JSON parsing failed: {message}"
        | TableNotFound html -> $"No table with class 'wikitable script' found. {html}"
        | RowsNotFound -> "No rows found in the table"
        | CellExtractionFailed -> "Failed to extract any valid tuples from rows"

module HtmlTableFetcher =

    /// Makes an HTTP request to a URL and returns the response content.
    let fetchJsonFromUrl (url: string) : FetchResult<string> =
        try
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
                Error (HttpRequestFailed $"Failed to fetch URL: {url}. Status code: {response.StatusCode}")
        with
        | ex -> Error (HttpRequestFailed $"Exception during HTTP request: {ex.Message}")

    /// Parses HTML content from a JSON response.
    let extractHtmlFromJson (jsonString: string) : FetchResult<string> =
        try
            let json = JObject.Parse(jsonString)
            let htmlContent = json.SelectToken("parse.text.*").Value<string>()
            Ok htmlContent
        with
        | :? Newtonsoft.Json.JsonReaderException as ex ->
            Error (JsonParsingFailed ex.Message)

    /// Parses an HTML string and returns the specified table.
    let parseTableFromHtml (html: string) : FetchResult<HtmlNode> =
        let doc = HtmlDocument()
        doc.LoadHtml(html)
        let table = doc.DocumentNode.SelectSingleNode("//table[contains(@class, 'wikitable') and contains(@class, 'script')]")
        match table with
        | null -> Error (TableNotFound doc.ParsedText )
        | _ -> Ok table

    /// Extracts rows from the HTML table.
    let extractRowsFromTable (table: HtmlNode) : FetchResult<HtmlNodeCollection> =
        let rows = table.SelectNodes(".//tr")
        match rows with
        | null -> Error RowsNotFound
        | _ -> Ok rows
    
    /// Extracts cell values from a row if it contains at least two valid cells.
    let extractPairFromSingleRow (row: HtmlNode) : option<(string * string)> =
        match row.SelectNodes(".//td") with
        | null -> None
        | cells when cells.Count < 2 -> None
        | cells ->
            let cell1 = cells.[0].InnerText.Trim()
            let cell2 = cells.[1].InnerText.Trim()
            
            match String.IsNullOrWhiteSpace(cell1), String.IsNullOrWhiteSpace(cell2) with
            | false, false -> Some (cell1, cell2)
            | _ -> None
    
    /// Extracts tuples of data from table rows.
    let extractPairsFromRows (rows: HtmlNodeCollection) : FetchResult<(string * string) list> =
        let tuples = 
            rows
            |> Seq.cast<HtmlNode>
            |> Seq.choose extractPairFromSingleRow
            |> Seq.toList

        if not (tuples.IsEmpty) then 
            Ok tuples
        else 
            Error CellExtractionFailed

    /// Main function to fetch, parse, and return data from a URL as a list of tuples.
    let fetchTableData (url: string) : FetchResult<(string * string) list> =
        result {
            let! json = fetchJsonFromUrl url

            let! html = extractHtmlFromJson json
            
            let! table = parseTableFromHtml html  

            let! tableRows = extractRowsFromTable table  

            return! extractPairsFromRows tableRows
        }