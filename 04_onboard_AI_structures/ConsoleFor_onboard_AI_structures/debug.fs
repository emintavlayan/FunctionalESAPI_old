
open System
open VMS.TPS
open TableParser
open DatabaseAccess

[<EntryPoint>]
let main argv =
    let url = "http://rghrhkfedoc001/radiowiki/api.php?action=parse&format=json&page=Wiki_improvements&prop=text"
    
    let reportToConsole (a:string, b: string) =
        Console.WriteLine $" ({a}, {b}) "

    match getData(url) with
    | TableParseResult tableResult ->  // Assuming TableParseResult contains the (string * string) list
            
        match tableResult with
        | ParsedTableData pairs ->
            pairs
            |> List.iter reportToConsole  // Print each result to the console
            0

        | NoTableFound ->
            Console.WriteLine "Error: No table found in the HTML."  
            0

        | NoRowsFound ->
            Console.WriteLine "Error: No rows found in the table."
            0

        | ParseError msg ->
            Console.WriteLine  $"Error parsing table:\n-----\n{msg}" 
            0

    | CurlNotInstalled ->
        Console.WriteLine  $"Error: Curl is not installed on this system."  
        0

    | JsonParsingError msg ->
        Console.WriteLine  $"Error parsing JSON:\n-----\n{msg}"  
        0

    | HtmlParsingError msg ->
        Console.WriteLine  $"Error parsing HTML:\n-----\n{msg}"  
        0

    | ProcessError msg ->
        Console.WriteLine  $"Process error:\n-----\n{msg}"  
        0

    | UnexpectedError msg ->
        Console.WriteLine  $"Unexpected error:\n-----\n{msg}"  
        0
