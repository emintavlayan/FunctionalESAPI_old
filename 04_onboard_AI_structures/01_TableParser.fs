namespace VMS.TPS

open HtmlAgilityPack
open System

/// Module for parsing HTML tables into tuples of string data.
module TableParser =

    /// Represents the result of parsing the structure pairs table.
    type TableParseResult =
        | ParsedTableData of (string * string) list
        | NoTableFound
        | NoRowsFound
        | ParseError of string

    /// Loads HTML from a string.
    let loadHtml html =
        let doc = HtmlDocument()
        doc.LoadHtml(html)
        doc

    /// Selects the first table with a specific class.
    let selectTableWithClass className (doc: HtmlDocument) =
        doc.DocumentNode.SelectSingleNode($"//table[@class='{className}']")
    
    /// Extracts all rows from a table.
    let extractRows (table: HtmlNode) =
        table.SelectNodes(".//tr")

    /// Extracts cells from a row and creates a tuple if there are at least two cells.
    let extractCells (row: HtmlNode) =
        let cells = row.SelectNodes(".//td")
        if cells <> null && cells.Count >= 2 && 
           not (String.IsNullOrWhiteSpace(cells.[0].InnerText)) && 
           not (String.IsNullOrWhiteSpace(cells.[1].InnerText)) then
            Some (cells.[0].InnerText.Trim(), cells.[1].InnerText.Trim())
        else
            None
    
    /// Tries to load an HTML string and parse it to find a specified table.
    /// <param name="html">The HTML string to parse.</param>
    /// <returns>List of tuples from the table rows if successful, otherwise throws an exception.</returns>
    let parseTableToTuples html =
        let doc = loadHtml(html)
        let table = selectTableWithClass "wikitable script" doc // hardcoded table class
        
        // Attempt to find a table with the class 'wikitable'.
        match table with
        | null -> NoTableFound
        
        | table ->
            // Attempt to extract all rows from the table.
            let rows = extractRows table
            match rows with
            | null -> NoRowsFound
            
            | rows ->
                rows 
                |> Seq.cast
                |> Seq.choose extractCells
                |> Seq.toList
                |> ParsedTableData
        

        
