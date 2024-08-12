namespace VMS.TPS

open HtmlAgilityPack
open System
open Newtonsoft.Json.Linq
open System.Net.Http

/// Unified result type for the module
type FetchResult<'T> = Result<'T, FetchError>

/// Error types in the order of occurrence
and FetchError =
    | HttpRequestFailed of string
    | JsonParsingFailed of string
    | HtmlParsingFailed of string
    | TableNotFound of string
    | RowsNotFound of string
    | CellExtractionFailed of string

module HtmlTableFetcher =

    /// Makes an HTTP request to a URL and returns the response content.
    let fetchHtmlContent (url: string) : FetchResult<string> =
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
            Error (JsonParsingFailed $"Failed to parse JSON. Error: {ex.Message}")

    /// Parses an HTML string and returns the specified table.
    let parseHtmlForTable (html: string) : FetchResult<HtmlNode> =
        let doc = HtmlDocument()
        doc.LoadHtml(html)
        let table = doc.DocumentNode.SelectSingleNode("//table[@class='wikitable script']")
        match table with
        | null -> Error (TableNotFound "No table with class 'wikitable script' found.")
        | _ -> Ok table

    /// Extracts rows from the HTML table.
    let extractTableRows (table: HtmlNode) : FetchResult<HtmlNodeCollection> =
        let rows = table.SelectNodes(".//tr")
        match rows with
        | null -> Error (RowsNotFound "No rows found in the table.")
        | _ -> Ok rows

    /// Extracts tuples of data from table rows.
    let extractTuplesFromRows (rows: HtmlNodeCollection) : FetchResult<(string * string) list> =
        let tuples = 
            rows
            |> Seq.cast<HtmlNode>
            |> Seq.choose (fun row ->
                let cells = row.SelectNodes(".//td")
                if cells <> null && cells.Count >= 2 then
                    let cell1 = cells.[0].InnerText.Trim()
                    let cell2 = cells.[1].InnerText.Trim()
                    if not (String.IsNullOrWhiteSpace(cell1)) && not (String.IsNullOrWhiteSpace(cell2)) then
                        Some (cell1, cell2)
                    else None
                else None)
            |> Seq.toList

        if tuples.IsEmpty then 
            Error (CellExtractionFailed "Failed to extract any valid tuples from rows.")
        else 
            Ok tuples

    /// Main function to fetch, parse, and return data from a URL as a list of tuples.
    let fetchTableData (url: string) : FetchResult<(string * string) list> =
        url
        |> fetchHtmlContent
        |> Result.bind extractHtmlFromJson
        |> Result.bind parseHtmlForTable
        |> Result.bind extractTableRows
        |> Result.bind extractTuplesFromRows
