// JsHelpers.fs
namespace VMS.TPS

open NodeTypes

module JsHelpers = 

    // Generates JavaScript to change the visibility of an HTML element.
    let jsChangeElementVisibility (id: string) (isVisible: bool) : string =
        let visibility = if isVisible then "block" else "none"
        sprintf "document.getElementById('%s').style.display = '%s';" id visibility

    // Generates JavaScript to update the content of a textarea.
    let jsUpdateTextArea (id: string) (text: string) : string =
        sprintf "document.getElementById('%s').value = '%s';" id text

    // Generates JavaScript to update a dropdown's options and selection.
    let jsUpdateDropdown (id: string) (selection: string) (options: string list) : string =
        let optionsJs = String.concat "" (List.map (sprintf "<option>%s</option>") options)
        sprintf "document.getElementById('%s').innerHTML = '%s'; document.getElementById('%s').value = '%s';" id optionsJs id selection

    // Combines the appropriate JavaScript generation function based on the node type.
    let generateJsForNode = function
        | Dropdown node -> jsUpdateDropdown node.Id node.Selection node.Options
        | Section node -> jsChangeElementVisibility node.Id node.IsVisible
        | TextArea node -> jsUpdateTextArea node.Id node.Text
       

