namespace VMS.TPS

open AngleSharp.Dom
open Model

module JavascriptHelpers =

    // Create a single line of JavaScript code to update the UI
    let createSingleJsCode (id: string) (what: string) (model: Model) =
        let newHtml =  (HtmlHelpers.get id what model).Replace("'", "\\'")
        let jsCode = sprintf "document.getElementById('%s').%s = '%s';" id what (newHtml) 
        jsCode

    // Create full JavaScript code for a list of updates
    let createFullJsCode (idAndWhatList: (string * string) list) (model: Model) =
        idAndWhatList
        |> List.distinct // makes unique
        |> List.map (fun (id, what) -> createSingleJsCode id what model) 
        |> String.concat " "
