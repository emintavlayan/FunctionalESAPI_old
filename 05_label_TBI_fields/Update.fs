// Update.fs
namespace VMS.TPS

open NodeTypes
open Model

module Update =

    // Represents possible messages that trigger updates.
    type Msg =
        | Dropdown1Changed of string
        | Dropdown2Changed of string
        | NextClicked

    // Updates the model based on the received message.
    let update msg model =
        let updatedNodes =
            match msg with
            | Dropdown1Changed newSelection ->
                model.Nodes |> List.map (function
                    | Dropdown node when node.Id = "dropdown1" ->
                        Dropdown { node with Selection = newSelection; Changed = true }
                    | other -> other)
        
            | Dropdown2Changed newSelection ->
                model.Nodes |> List.map (function
                    | Dropdown node when node.Id = "dropdown2" ->
                        Dropdown { node with Selection = newSelection; Changed = true }
                    | other -> other)
        
            | NextClicked ->
                model.Nodes |> List.map (function
                    | Section node when node.Id = "section1" ->
                        Section { node with IsVisible = false; Changed = true }
                    | Section node when node.Id = "section2" ->
                        Section { node with IsVisible = true; Changed = true }
                    | TextArea node when node.Id = "reportStep2" ->
                        TextArea { node with Text = "Generated text for step 2"; Changed = true }
                    | other -> other)

        { model with Nodes = updatedNodes; ViewNeedsUpdate = true }

