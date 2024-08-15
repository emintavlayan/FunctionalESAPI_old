//// File: TbiPlans.Fs
namespace VMS.TPS

////open VMS.TPS.Common.Model.API
////open VMS.TPS.Common.Model.Types

//open TbiPlanId
//open TbiTypes
//open TbiPlanId.TbiPlanIdParser
//open TbiPlanId.ActivePatternsForTbiPlanIds

//module TbiPlans =

//    module TbiFieldId =
       
//        type TbiFieldId = TbiFieldId of string

//    module TbiPlanningPlan =
    
//        type TbiPlanningPlan = {
//            Id: TbiTypes.TbiPlanningPlanId
//            PlanSetup: PlanSetup
//            TreatmentFieldIdList: TbiFieldId.TbiFieldId list
//        }

//    module TbiTreatmentPlan =
    
//        type TbiPlanningPlan = {
//            Id: TbiTypes.TbiPlanningPlanId
//            PlanSetup: PlanSetup
//            TreatmentFieldIdList: TbiFieldId.TbiFieldId list
//        }

//        // Function to validate TbiTreatmentPlan
//    let validateTbiTreatmentPlan (planSetup: PlanSetup) (planId: string) =
//        match TbiPlanIdParser.tryParseTbiTreatmentPlanId planId with
//        | Some tbiTreatmentPlan ->
//            // Rule 1: Starting dose check
//            let startingDose = tbiTreatmentPlan.DoseInformation.StartingDose
//            match startingDose with
//            | StartingDose dose when dose <> 0.0 && dose % planSetup.DosePerFraction.Dose <> 0.0 ->
//                let msg = sprintf "Invalid starting dose: %f. Must be zero or divisible by DosePerFraction: %f." dose planSetup.DosePerFraction.Dose
//                failwith msg
//            | _ -> ()

//            // Rule 2: Total dose check
//            let (TotalDose totalDoseFromPlanName) = tbiTreatmentPlan.DoseInformation.TotalDose
//            let totalDoseFromPlanSetup = planSetup.DosePerFraction.Dose * (float planSetup.NumberOfFractions.Value)
//            if totalDoseFromPlanName <> totalDoseFromPlanSetup then
//                let msg = sprintf "Total dose mismatch. Plan name total dose: %f, Plan setup total dose: %f." totalDoseFromPlanName totalDoseFromPlanSetup
//                failwith msg
//        | None ->
//            failwith "Invalid plan ID format."