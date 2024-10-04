namespace VMS.TPS

open VMS.TPS.Common.Model.API
open PlanCollecting
open FsToolkit.ErrorHandling
open WebUI4CSharp_net48
open WebUI
open System.IO

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() = 
    
    member __.Execute(context: ScriptContext) = 
    
        // Initilazation
        let course = context.Course

        let planIdList : string list =
            course.PlanSetups
            |> Seq.map ( fun p -> p.Id )
            |> Seq.toList
        
        // Combined match to assign pp and tp if Ok
        let pp, tp = 
            match PlanCollecting.getRecentPlanPairIds course with
            | Ok dto -> 
                dto.PlanningPlanDto.Id, dto.TreatmentPlanDto.Id
            | Error msg -> 
                printfn "Error: %s" msg 
                "", ""
        
        // Read the HTML content from the file
        let myHtml = File.ReadAllText("C:\Users\etav0001\Documents\FunctionalESAPI\05_label_TBI_fields\index.html")

        // Create the WebUI window
        let window = new WebUIWindow()

        // Bind the JavaScript function to F# functions using a lambda expression
        window.Bind("handleDropdownSelection", BindCallback(fun e -> handleDropdownSelection(&e) |> ignore)) |> ignore

        // Show the WebUI window with the HTML content
        window.Show(myHtml) |> ignore

        // Populate the dropdowns with initial values
        populateDropdowns window planIdList "ppDropdown" "tpDropdown" pp tp  |> ignore

        // Wait for the UI to close
        WebUI.Wait() |> ignore

        // Clean up resources
        WebUI.Clean() |> ignore

        0