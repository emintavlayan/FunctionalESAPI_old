// View.fs
namespace VMS.TPS

open NodeTypes
open Model
open JsHelpers

module View = 

    // Generates the view by processing only the nodes that have changed.
    let view model =
        if model.ViewNeedsUpdate then
            let jsCommands =
                model.Nodes
                |> List.filter (function
                    | Dropdown node -> node.Changed
                    | Section node -> node.Changed
                    | TextArea node -> node.Changed)
                |> List.map generateJsForNode

            let jsCode = String.concat " " jsCommands
           // WebUI.RunJs(jsCode)

            // Reset the Changed flags
            let resetNodes =
                model.Nodes |> List.map (function
                    | Dropdown node -> Dropdown { node with Changed = false }
                    | Section node -> Section { node with Changed = false }
                    | TextArea node -> TextArea { node with Changed = false })

            { model with Nodes = resetNodes; ViewNeedsUpdate = false }
        else
            model

