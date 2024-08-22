namespace VMS.TPS

open System
open Utility
open FsToolkit.ErrorHandling

/// Represents the ID for a TBI planning plan, consisting of a treatment code and a version number.
type TbiPlanningPlanId = TbiPlanningPlanId of string with
    
    member this.Value = let (TbiPlanningPlanId id) = this in id

    // string -> Result<TbiPlanningPlanId, string>
    static member TryCreate (id : string) =
        let txCode = first5Chars id
        if not (txCode.ToLower().StartsWith("tb")) then
            Error "plan id must start with: TB"
        elif 


/// Represents the ID for a TBI treatment plan, including treatment code, dose information, and version number.
//type TbiTreatmentPlanId = {
    
//}

///// Represents a unique identifier for a TBI beam as a string value.
//type TbiBeamId = TbiBeamId of string

///// Represents a TBI planning plan, including its ID, associated beams, and whether the beams are ordered.
//type TbiPlanningPlan = {
//    /// The ID of the planning plan.
//    PlanId: TbiPlanningPlanId
//    /// The list of beam IDs associated with the planning plan.
//    BeamIds: TbiBeamId list
//}

///// Represents a TBI treatment plan, including its ID, associated beams, and whether the beams are ordered.
//type TbiTreatmentPlan = {
    
//}

///// Represents a course of TBI treatment, consisting of a planning plan and a treatment plan.
//type TbiPlanPair = TbiPlanningPlan * TbiTreatmentPlan

    
