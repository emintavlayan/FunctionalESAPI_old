namespace VMS.TPS

open System.Windows.Forms

open VMS.TPS.Common.Model.API

/// Will be used before the real action
type PlanPairDto = { 
    PlanningPlanDto: PlanSetup
    TreatmentPlanDto: PlanSetup }

type PlanGatheringResult = Result< PlanPairDto, string>
    
/// 
module PlanCollecting =

    /// Gets the 2 most recent plans in the course and returns ( shortId, LongId )
    let getRecentPlanPairIds (course: Course) =
        let planSetups = 
            course.PlanSetups
            |> Seq.filter ( fun plan -> plan.Id.ToLowerInvariant().StartsWith("tb"))
            |> Seq.sortByDescending (fun plan -> plan.HistoryDateTime)
            |> Seq.take 2
            |> Seq.toList
    
        match planSetups with
        | [p1; p2] -> 
            if p1.Id.Length < p2.Id.Length then 
                Ok { PlanningPlanDto = p1; TreatmentPlanDto = p2 }
            else 
                Ok { PlanningPlanDto = p2; TreatmentPlanDto = p1 }
        | _ -> Error "Not enough PlanSetups available." 
    
    /// Creates and shows a form for user confirmation or adjustment
    let getApprovalForPlanSelection (course: Course) (defaultPlanPair: PlanPairDto) =
        
        // Love the simplicity of WinForms
        let form = new Form(Text = "TBI Plan Selection", Width = 400, Height = 200)
        let planningLabel = new Label(Text = "TBI Planning Plan:", Top = 20, Left = 10, Width = 150)
        let planningDropdown = new ComboBox(Top = 20, Left = 170, Width = 200)
        let treatmentLabel = new Label(Text = "TBI Treatment Plan:", Top = 60, Left = 10, Width = 150)
        let treatmentDropdown = new ComboBox(Top = 60, Left = 170, Width = 200)
        let okButton = new Button(Text = "OK", Top = 100, Left = 150, Width = 100)
    
        // Populate dropdowns with PlanSetup IDs
        let planIds = 
            course.PlanSetups 
            |> Seq.map (fun p -> p.Id)        // create id seq instead of real plans
            |> Seq.toArray                                     // convert into an array for UI purposes
            |> Array.map (fun id -> id :> obj)  // upcast strings to objects ?!?!

        planningDropdown.Items.AddRange(planIds)
        treatmentDropdown.Items.AddRange(planIds)
    
        // Set default selections
        planningDropdown.SelectedItem <- defaultPlanPair.PlanningPlanDto :> obj
        treatmentDropdown.SelectedItem <- defaultPlanPair.TreatmentPlanDto :> obj
    
        // Add controls to the form
        form.Controls.Add(planningLabel)
        form.Controls.Add(planningDropdown)
        form.Controls.Add(treatmentLabel)
        form.Controls.Add(treatmentDropdown)
        form.Controls.Add(okButton)
    
        // Handle the OK button click
        let result = ref None
        okButton.Click.Add (fun _ ->
            
            let selectedPlanningPlan =
                course.PlanSetups
                |> Seq.find ( fun plan -> plan.Id = planningDropdown.SelectedItem.ToString() )

            let selectedTreatmentPlan =
                course.PlanSetups
                |> Seq.find ( fun plan -> plan.Id = treatmentDropdown.SelectedItem.ToString() )

            result.Value <- Some (Ok { PlanningPlanDto = selectedPlanningPlan; TreatmentPlanDto = selectedTreatmentPlan })
            form.Close()
        )
    
        // Show the form as a dialog
        form.ShowDialog() |> ignore
    
        // Return the result or an error if the user cancels the form
        match result.Value with
        | Some r -> r
        | None -> Error "User canceled the selection."
        
    
    