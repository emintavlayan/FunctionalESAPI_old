namespace VMS.TPS

open System.IO
open Model
open WebUI4CSharp_net48
open Elmish

// View function to update the HTML based on the model
module View =

    // Read the index.html file content
    let htmlContent = File.ReadAllText("index4.html")
     
    
    let view (window: WebUIWindow) (model: Model) (dispatch: Dispatch<Msg>) =
        if model.ViewNeedsToBeUpdated then

            // THIS needs to be uniqe list ------------------------------------------------------------------------------
            let jsCode = JavascriptHelpers.createFullJsCode model.ElementsToBeUpdated model

            // Execute jsCode in the browser (you might need to pass this to your JavaScript runtime)
            printfn "JavaScript to run: %s" jsCode

            dispatch SetViewUpdateOff // Turn off the update flag after the view is updated
            window.Run(jsCode) |> ignore
            
        else
            // No need 
            printfn "No view update needed" |> ignore

     