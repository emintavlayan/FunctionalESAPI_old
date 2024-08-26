module WebUI_Events

open WebUI4CSharp_net48
open System

let handleDropdownSelection (e: webui_event_t byref) =
    let lEvent = WebUIEvent(e)

    let ppSelection = lEvent.GetString()
    let tpSelection = lEvent.GetStringAt(UIntPtr(1UL))

    printfn "Selected Plans: Planning Plan = %s, Treatment Plan = %s" ppSelection tpSelection
