namespace VMS.TPS

open System.Windows.Forms      // Required for MessageBox.Show
open VMS.TPS.Common.Model.API

open TableParser
open DatabaseAccess
open CopyVolume

module DataBaseAccessError =
    let message: DataBaseAccessError -> string =
        function
        | UnexpectedHttpResponse code ->  $"Error: The server returned an unexpected response status code {code}"
        | JsonParsingError -> $"Error parsing JSON"
        | HtmlParsingError msg -> $"Error parsing HTML:\n-----\n{msg}"
        | UnexpectedError msg ->  $"Unexpected error:\n-----\n{msg}"
        | ProcessError msg -> $"Process error:\n-----\n{msg}"
        | TableParseError NoTableFound -> "Error: No table found in the HTML."
        | TableParseError NoRowsFound -> "Error: No rows found in the table."
        | TableParseError (ParseError msg) -> "Error parsing table:\n-----\n{msg}"

module OnboardAiStructures =

    let Run(context: ScriptContext) =
        
        let url = "http://rghrhkfedoc001/radiowiki/api.php?action=parse&format=json&page=Wiki_improvements&prop=text"
        // page will be updated to: DcmCollab_AI_Autosegmentering_af_HH
    
        let ss = context.StructureSet
        
        /// Processes a single pair of structure IDs and attempts to copy volume
        let processPair (aiId, rhId) =
            match copyVolume context.StructureSet aiId rhId with
            | VolumeCopiedSuccessfully message ->
                Some (aiId, rhId, message)
            
            | VolumeCopyFailed message ->
                Some (aiId, rhId, sprintf "ERROR: %s" message)

        /// Processes results and appends them as table rows or shows error message
        let processResult maybeResult =
            match maybeResult with
            | Some (aiId, rhId, message) ->
                HtmlOutput.appendTableRow (aiId, rhId, message)
            
            | None ->
                ()  // Do nothing if there's no result to process

        HtmlOutput.initializeHtml()

        getData(url)
        |> Result.mapError( DataBaseAccessError.message >> MessageBox.Show >>ignore)
        |> Result.map(List.iter (processPair>> processResult))
        |> ignore

        HtmlOutput.finalizeAndDisplayHtml()