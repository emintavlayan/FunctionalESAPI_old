namespace VMS.TPS

open Model
open Elmish

// Update function to handle messages
module Update =

    let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
        match msg with
        | Increment (id, value ) ->
            
            let currentValue = HtmlHelpers.get id "textContent" model
            let newValue = string ( int currentValue + value)
            
            HtmlHelpers.set id "textContent" (newValue.ToString()) model
            let newList = ( id, "textContent" ) :: model.ElementsToBeUpdated 
            
            { model with ElementsToBeUpdated = newList }, Cmd.ofMsg SetViewUpdateOn

        | Decrement (id, value ) ->

            let currentValue = HtmlHelpers.get id "textContent" model
            let newValue = string ( int currentValue - value)
            
            HtmlHelpers.set id "textContent" (newValue.ToString()) model
            let newList = ( id, "textContent" ) :: model.ElementsToBeUpdated 
            
            { model with ElementsToBeUpdated = newList }, Cmd.ofMsg SetViewUpdateOn

        | SetViewUpdateOn ->
            printfn "set view on called"
            { model with ViewNeedsToBeUpdated = true }, Cmd.none

        | SetViewUpdateOff ->
            printfn "set view off called"
            { model with ViewNeedsToBeUpdated = false; ElementsToBeUpdated = []  }, Cmd.none
