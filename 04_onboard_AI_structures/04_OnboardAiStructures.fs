namespace VMS.TPS

open System
open System.Windows.Forms      // Required for MessageBox.Show
open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling
open System.Linq


open HtmlTableFetcher
open StructureOperations
open HtmlOutput

open HtmlAgilityPack
open Newtonsoft.Json

// Simple JSON Serialization and Deserialization Example

 type Person = {
     Name: string
     Age: int
 }

module OnboardAiStructures =

    /// Higher-level result type that aggregates results and errors from all modules.
    type OnboardingResult<'T> = Result<'T,OnboardingError>

    and OnboardingError =
    | FetchError of FetchError
    | HtmlOutputError of HtmlOutputError

    /// Processes a single pair of structure IDs, attempts to copy volume, and logs the result to the Html
    let processAndLogPair (ss: StructureSet) (aiId: string) (rhId: string) =
        let message =
            StructureOperations.copyStructureVolume ss aiId rhId
            |> Result.either 
                (fun successMessage -> successMessage)  
                (fun errorMsg -> sprintf "%A" errorMsg)
        
        // Log the result of the operation by appending it to the HTML table
        HtmlOutput.appendStructureOperationResultsToHtmlTable htmlFilePath (aiId, rhId, message) 
        |> Result.mapError OnboardingError.HtmlOutputError
    
    /// Runs the main Onboarding Workflow
    let Run(context: ScriptContext) =

        let url = "http://rghrhkfedoc001/radiowiki/api.php?action=parse&format=json&page=DcmCollab_AI_Autosegmentering_af_HH&prop=text"
        let ss = context.StructureSet
        
        let result =
            result {
                // Fetch structure pairs from the specified URL and map any errors to OnboardingError
                let! structurePairs = 
                    HtmlTableFetcher.fetchTableData(url) 
                    |> Result.mapError OnboardingError.FetchError
                    
                // Write the HTML header to start the output file
                do! HtmlOutput.writeHeaderToHtml(htmlFilePath) 
                    |> Result.mapError OnboardingError.HtmlOutputError
                    
                // Process and log each structure pair by copying volumes and updating the HTML
                structurePairs
                |> List.iter (fun (aiId, rhId) ->
                    processAndLogPair ss aiId rhId |> ignore
                )

                // Write the HTML footer to finalize the output file
                do! HtmlOutput.writeFooterToHtml(htmlFilePath) 
                    |> Result.mapError OnboardingError.HtmlOutputError
                
                // Display the HTML output file in the default web browser
                do! HtmlOutput.displayHtml(htmlFilePath) 
                    |> Result.mapError OnboardingError.HtmlOutputError
                    
            }
        
        // Display a message box showing the error if the operation fails
        match result with
        | Ok () -> ()
        | Error err -> MessageBox.Show( err.ToString() ) |> ignore

