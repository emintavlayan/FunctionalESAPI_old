namespace VMS.TPS

open System.Windows.Forms      // Required for MessageBox.Show
open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling

open HtmlTableFetcher
open StructureOperations
open HtmlOutput

module OnboardAiStructures =

    /// Processes a single pair of structure IDs, attempts to copy volume, and logs the result
    let processAndLogPair (ss: StructureSet) (aiId: string, rhId: string) =
        match copyStructureVolume ss aiId rhId with
        | Ok message -> appendTableRow (aiId, rhId, message) 
            
        | Error err -> ( err >> MessageBox.Show >> ignore)
            

    /// Runs the main workflow
    let Run(context: ScriptContext) =
        let url = "http://rghrhkfedoc001/radiowiki/api.php?action=parse&format=json&page=Wiki_improvements&prop=text"
        let ss = context.StructureSet

        HtmlOutput.initializeHtml()

        fetchTableData(url)
        |> Result.mapError(  MessageBox.Show >> ignore)
        |> Result.map(List.iter (processAndLogPair ss))
        |> ignore

        HtmlOutput.finalizeAndDisplayHtml()
