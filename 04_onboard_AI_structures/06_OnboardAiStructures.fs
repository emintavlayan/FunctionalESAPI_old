namespace VMS.TPS

open System.Windows.Forms      // Required for MessageBox.Show
open VMS.TPS.Common.Model.API

open TableParser
open DatabaseAccess
open CopyVolume

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

        match getData(url) with
        | TableParseResult tableResult ->  // Assuming TableParseResult contains the (string * string) list
            
            match tableResult with
            | ParsedTableData pairs ->
                pairs
                |> List.map processPair  // Apply processPair to each tuple in the list
                |> List.iter processResult  // Print each result to the console

            | NoTableFound ->
                MessageBox.Show("Error: No table found in the HTML.") |> ignore

            | NoRowsFound ->
                MessageBox.Show("Error: No rows found in the table.") |> ignore

            | ParseError msg ->
                MessageBox.Show( $"Error parsing table:\n-----\n{msg}") |> ignore

        | UnexpectedHttpResponse responseCode ->
            MessageBox.Show( $"Error: The server returned an unexpected response status code: {responseCode}") |> ignore

        | JsonParsingError msg ->
            MessageBox.Show( $"Error parsing JSON:\n-----\n{msg}") |> ignore

        | HtmlParsingError msg ->
            MessageBox.Show( $"Error parsing HTML:\n-----\n{msg}") |> ignore

        | ProcessError msg ->
            MessageBox.Show( $"Process error:\n-----\n{msg}") |> ignore

        | UnexpectedError msg ->
            MessageBox.Show( $"Unexpected error:\n-----\n{msg}") |> ignore

        HtmlOutput.finalizeAndDisplayHtml()

    
