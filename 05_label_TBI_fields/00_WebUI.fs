namespace VMS.TPS

open WebUI4CSharp_net48
open System
open Fs4Js

module WebUI =
    
    
    let handleDropdownSelection (e: webui_event_t byref) =
        let lEvent = WebUIEvent(e)
    
        let ppSelection = lEvent.GetString()
        let tpSelection = lEvent.GetStringAt(unativeint 1)
    
        printfn "Selected Plans: Planning Plan = %s, Treatment Plan = %s" ppSelection tpSelection

    // Function to populate dropdowns from the backend
    let populateDropdowns (window: WebUIWindow) (values : string list) (ppdd:string) (tpdd: string) (pp: string) (tp: string) =
        
        // Use the utility function to create the JavaScript array declaration
        let jsArrayString = createJsArrayString(values)

        let populateScript = Fs4Js.createScript jsArrayString ppdd tpdd pp tp
        
        // Execute the JavaScript in the frontend
        window.Run(populateScript) |> ignore

    

