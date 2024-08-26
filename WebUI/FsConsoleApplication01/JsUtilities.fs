module JsUtilities

// Utility function to convert an F# list to a JavaScript array string
let createJsArrayString (values: string list) =
    values 
    |> List.map (fun v -> sprintf "'%s'" v) 
    |> String.concat ", "
    // use this like: let values = [%s];
 