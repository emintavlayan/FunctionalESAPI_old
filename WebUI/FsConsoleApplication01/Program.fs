open System
open System.IO
open WebUI4CSharp_net48
open WebUI_Events
open JsUtilities

module WebUIExample =

    // Function to populate dropdowns from the backend
    let populateDropdowns (window: WebUIWindow) =
        // Create the list of values to populate
        let values = ["a"; "b"; "c"; "d"; "e"]

        // Use the utility function to create the JavaScript array declaration
        let jsArrayString = createJsArrayString(values)

        // Generate JavaScript to populate both dropdowns and set initial values
        let createScript list ppDropdown tpDropdown pp tp= 
            sprintf """
            let values = [%s];
            let dropdown1 = document.getElementById('%s');
            let dropdown2 = document.getElementById('%s');
            dropdown1.innerHTML = '';
            dropdown2.innerHTML = '';
            for (let i = 0; i < values.length; i++) {
                let option1 = document.createElement('option');
                let option2 = document.createElement('option');
                option1.value = option2.value = values[i];
                option1.text = option2.text = values[i];
                dropdown1.appendChild(option1);
                dropdown2.appendChild(option2);
            }
            dropdown1.value = '%s';
            dropdown2.value = '%s';
            """ list ppDropdown tpDropdown pp tp

        let populateScript = createScript jsArrayString "ppDropdown" "tpDropdown"  "b" "d"
        // get plansetup as list
        //let planList =

        //let planListJs = createJsArrayString planList


        // get tbi plan pair dto
        //let pp =
        //let tp =


        // Execute the JavaScript in the frontend
        window.Run(populateScript) |> ignore

    
    [<EntryPoint>]
    let main argv =
        // Read the HTML content from the file
        let myHtml = File.ReadAllText("..\\..\\..\\index.html")

        // Create the WebUI window
        let window = new WebUIWindow()

        // Bind the JavaScript function to F# functions using a lambda expression
        window.Bind("handleDropdownSelection", BindCallback(fun e -> handleDropdownSelection(&e) |> ignore)) |> ignore

        // Show the WebUI window with the HTML content
        window.Show(myHtml) |> ignore

        // Populate the dropdowns with initial values
        populateDropdowns(window)

        // Wait for the UI to close
        WebUI.Wait() |> ignore

        // Clean up resources
        WebUI.Clean() |> ignore

        0 // Return an integer exit code
