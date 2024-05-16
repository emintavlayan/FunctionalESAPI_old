namespace VMS.TPS

pen VMS.TPS.Common.Model.API

open TbiTypes
open TbiPlanId.TbiPlanIdParser
open TbiPlanId.ActivePatternsForTbiPlanIds

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    
    member __.Execute(context: ScriptContext) = 
    
        // Initilazation
        let patient = context.Patient
        
        let plansInScope = context.Course.PlanSetups

        // Process each plan ID
        for plan in plansInScope do
            match plan.Id with
            | TbiPlanningPlanId tbiPlan ->
                // Handle TbiPlanningPlan
                printfn "TbiPlanningPlan: %A" tbiPlan
            | TbiTreatmentPlanId tbiTreatmentPlan ->
                // Handle TbiTreatmentPlan
                printfn "TbiTreatmentPlan: %A" tbiTreatmentPlan
            | InvalidPlanId ->
                // Handle invalid plan ID (throw an error or log it)
                printfn "InvalidPlan: %s" plan.Id


        0