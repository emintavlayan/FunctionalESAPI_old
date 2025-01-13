namespace VMS.TPS

open Model
open WebUI4CSharp_net48
open Elmish
open System
 

// Handle events dispatched from WebUI and return the appropriate message
module Subscriptions =

    let handleEvents (e: byref<webui_event_t>) : Msg = 
        let lEvent = new WebUIEvent(e)
        let eventType = lEvent.GetString()
        let id = lEvent.GetStringAt(unativeint 1)
        printfn "handle event id: %s" id
        //System.Console.ReadLine() |> ignore

        let value = lEvent.GetStringAt(unativeint 2) |> int
        printfn "handle event value: %s" (value.ToString())
        //System.Console.ReadLine() |> ignore
        

        match eventType with
        | "increment" -> 
            Increment (id, value )

        | "decrement" -> 
            Decrement (id, value )

        | _ -> 
            failwith "unknown message"

    // Keep a reference to the BindCallback delegate
    let mutable callbackReference : BindCallback = Unchecked.defaultof<_>

    // Define a WebUI subscription for event handling
    let webUIEventSubscription (window: WebUIWindow) (dispatch: Dispatch<Msg>) : IDisposable =
        callbackReference <- BindCallback(fun e -> 
            let msg = handleEvents (&e)
            dispatch msg )
        
        window.Bind("uiEvent", callbackReference) |> ignore
        window.Show(View.htmlContent) |> ignore
    
        { new IDisposable with
            member _.Dispose() = 
                window.Close() |> ignore
                callbackReference <- Unchecked.defaultof<_>
        }

    // Subscription setup
    let subscribe (window: WebUIWindow) (_model: Model) : (string list * (Dispatch<Msg> -> IDisposable)) list =
        [ [ "counter-sub" ], webUIEventSubscription window ]
