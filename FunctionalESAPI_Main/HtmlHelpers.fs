namespace VMS.TPS

open AngleSharp.Dom
open Model

module HtmlHelpers =

    // Helper function to get an element by ID from the body
    let escapeId (id: string) =
        id.Replace(".", "\\.").Replace("#", "\\#").Replace(" ", "\\ ")
        
    let getElementById (id: string) (model: Model) =
        let selector = "#" + escapeId id
        printfn "Querying with selector: %s" selector
        
        model.Body.QuerySelector(selector)

    // Function to get the content (innerHtml, innerText, outerHtml)
    let get (id: string) (what: string) (model: Model)  =
        let element = getElementById id model
        match what with
        | "innerHtml" -> element.InnerHtml
        | "textContent" -> element.TextContent
        | "outerHtml" -> element.OuterHtml
        | _ -> failwith "element or attr not found" 

    // Function to set the content (innerHtml, innerText, outerHtml)
    let set (id: string) (what: string) (value: string) (model: Model) =
        let element = getElementById id model
        match what with
        | "innerHtml" -> element.InnerHtml <- value
        | "textContent" -> element.TextContent <- value
        | "outerHtml" ->
            let parent = element.ParentElement
            if parent <> null then
                let newElement = element.Owner.CreateElement("div")
                newElement.OuterHtml <- value
                parent.ReplaceChild(newElement.FirstChild, element) |> ignore
        | _ -> ()
