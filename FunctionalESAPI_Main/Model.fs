namespace VMS.TPS

open System.IO
open AngleSharp
open AngleSharp.Dom

// Define the model
module Model =

    type Model = {
        Body: IElement  // Representing the parsed HTML body as an AngleSharp node
        ViewNeedsToBeUpdated: bool
        ElementsToBeUpdated: (string * string) list // (id, what) for updates
    }

    // Define the messages
    type Msg = 
    | Increment of ( string * int )
    | Decrement of ( string * int )
    | SetViewUpdateOn
    | SetViewUpdateOff

    let initialModel htmlContent =
        // Parse the initial HTML content
        let config = Configuration.Default
        let context = BrowsingContext.New(config)
        let documentTask = context.OpenAsync(fun req -> req.Content(htmlContent) |> ignore)
        let document = documentTask.Result
        let body = document.Body

        { Body = body; ViewNeedsToBeUpdated = false; ElementsToBeUpdated = [] }

    // Function to read the HTML file and initialize the app
    let initializeApp htmlContent =
        
        // Initialize the model with the parsed HTML
        let model = initialModel htmlContent

        // Return the initial model
        model// Initial state of the model

