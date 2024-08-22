// File: 
namespace VMS.TPS

open System
open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling

/// these poors will be used before the real action
type PlanDto = 
    { Id: string
      HistoryDateTime: DateTime }

type PlanPairDto = 
    { ShortPlan: PlanDto
      LongPlan: PlanDto }

type Error = 
    | NotEnoughPlanSetups
    | GeneralError of string
    
/// 
module GatheringPlans =

    /// 
    let getRecentPlanPair (course: Course) : Result<PlanPairDto, string> =
        let planSetups = 
            course.PlanSetups
            |> Seq.sortByDescending (fun plan -> plan.HistoryDateTime)
            |> Seq.take 2
            |> Seq.toList
    
        match planSetups with
        | [p1; p2] -> 
            if p1.Id.Length < p2.Id.Length then 
                Ok { ShortPlan = p1; LongPlan = p2 }
            else 
                Ok { ShortPlan = p2; LongPlan = p1 }
        | _ -> Error "Not enough PlanSetups available."
    

    
    