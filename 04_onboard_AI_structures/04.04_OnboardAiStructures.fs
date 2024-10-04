namespace VMS.TPS

open System.Windows.Forms      
open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling
open System.IO

open HtmlOutput

/// Higher-level result type that aggregates results and errors from all modules.
type OnboardingResult<'T> = Result<'T,OnboardingError>

and OnboardingError =
| FetchError of FetchError
| HtmlOutputError of HtmlOutputError

module OnboardingError =
    
    let message =
        function
        | FetchError e -> $"Failed to fetch data: {FetchError.message e}"
        | HtmlOutputError e -> $"Failed to prepare HTML: {e}"
            

module OnboardAiStructures =

       /// Runs the main Onboarding Workflow
    let run(context: ScriptContext) =

        let url = "http://rghrhkfedoc001/radiowiki/api.php?action=parse&format=json&page=DcmCollab_AI_Autosegmentering_af_HH&prop=text"
        let ss = context.StructureSet
        
        let outputPath : string =
            let tempPath : string = Path.GetTempPath()
            let htmlFileName : string = "output.html"
            Path.Combine(tempPath, htmlFileName)
        
        let result =
            result {
                // Fetch structure pairs from the specified URL and map any errors to OnboardingError
                let! structurePairs = 
                    HtmlTableFetcher.fetchTableData(url) 
                    |> Result.mapError OnboardingError.FetchError
                
                // Process and log each structure pair by copying volumes and updating the HTML                
                let structureCopyResults =
                    structurePairs
                    |> List.map (fun (aiId, rhId) ->
                        (aiId, rhId, StructureOperations.copyStructureVolume ss aiId rhId)
                    )

               do! writeHtml outputPath structureCopyResults |> Result.mapError HtmlOutputError
                    
            }
        
        // Display a message box showing the error if the operation fails
        match result with
        | Ok () -> ()
        | Error err -> MessageBox.Show( OnboardingError.message err ) |> ignore