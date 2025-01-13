namespace VMS.TPS

open Elmish
open WebUI4CSharp_net48
open Model
open Update
open Subscriptions
open View

// Main program setup
module Main =

    [<EntryPoint>]
    let main argv =

        let window = new WebUIWindow()
        
        // Initialize Elmish program
        Program.mkProgram (fun () -> (initializeApp View.htmlContent), Cmd.none ) update (view window)
        |> Program.withSubscription (subscribe window)
        |> Program.run

        // Wait for UI to close
        WebUI.Wait() |> ignore

        // Clean up resources
        WebUI.Clean() |> ignore

        0
