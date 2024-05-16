// File: TbiPlanId.fs
namespace VMS.TPS

open TbiTreatmentCodeDb
open TreatmentCodeTypes
open TbiTypes
open Patterns

module TbiPlanId =

    // Parser module for tbi Plan Ids
    module TbiPlanIdParser =

        open System.Text.RegularExpressions
        open TbiTypes
        open TbiTreatmentCode

        

        let tryParseTbiPlanningPlanId (planSetupId: string) : TbiPlanningPlanId option =
            let matchResult = tbiPlanningPlanIdPattern.Match(planSetupId)

            if matchResult.Success then
                let treatmentCodeId = matchResult.Groups.[1].Value
                let versionNumber = matchResult.Groups.[2].Value
            
                match TreatmentCodeId.create treatmentCodeId with
                | Some tcId -> Some { TreatmentCodeId = tcId; VersionNumber = VersionNumber versionNumber }
                | None -> None

            else 
                None

        // Regex pattern to match PlanSetup.Id for Tbi Treatment Plan Id
        let tbiTreatmentPlanIdPattern = Regex(@"^([A-Za-z]{2,3}\d{3})\s*(\d+(?:\.\d+)?)-(\d+(?:\.\d+)?)\s*(v\d+)$")
        // Explanation of Regex Pattern:
        // ^               : Asserts the start of the string.
        // [A-Za-z]{2,3}   : Matches any sequence of 2 to 3 letters.
        //                   This represents the alphabetic part of the treatment plan code.
        // \d{3}           : Matches exactly 3 digits. This represents the numeric part of the treatment plan code.
        // \s*             : Matches zero or more whitespace characters. 
        //                   This allows for flexibility in the format,
        //                   handling cases where there might be space between components.
        // \d+             : Matches one or more digits. This is the starting dose.
        // (?:\.\d+)?      : A non-capturing group that matches a decimal point followed by one or more digits, zero or one time. 
        //                   This allows for the dose to have a fractional part but does not require it.
        // -               : Matches the literal dash character. This separates the starting dose from the total dose.
        // \d+             : Matches one or more digits. This is the total dose.
        // (?:\.\d+)?      : Similar to the starting dose, 
        //                   this non-capturing group allows for an optional fractional part of the total dose.
        // \s*             : Matches zero or more whitespace characters. 
        //                   This allows for some flexibility in formatting before the version number.
        // v               : Matches the literal character 'v', which precedes the version number.
        // \d+             : Matches one or more digits, representing the version number of the plan.
        // $               : Asserts the end of the string. 
        //                   This ensures that the entire string conforms to the expected pattern.

        let tryParseTbiTreatmentPlanId (planSetupId: string) : TbiTreatmentPlanId option =
            let matchResult = tbiTreatmentPlanIdPattern.Match(planSetupId)
    
            if matchResult.Success then
                let treatmentCodeId = matchResult.Groups.[1].Value
                let startingDose = double (matchResult.Groups.[2].Value)
                let totalDose = double (matchResult.Groups.[3].Value)
                let versionNumber = matchResult.Groups.[4].Value

                match TreatmentCodeId.create treatmentCodeId with
                | Some tcId -> 
                    Some { 
                        TreatmentCodeId = tcId; 
                        DoseInformation = { StartingDose = StartingDose startingDose; TotalDose = TotalDose totalDose }; 
                        VersionNumber = VersionNumber versionNumber 
                    }
                | None -> None
            else
                None

    // Active Patterns to match TBI Plan Ids
    module ActivePatternsForTbiPlanIds =

        // DefTbiPlanningPlanIderns
        let (|TbiPlanningPlanId|TbiTreatmentPlanId|InvalidPlanId|) (planId: string) =
            match TbiPlanIdParser.tryParseTbiPlanningPlanId planId with
            | Some tbiPlan -> TbiPlanningPlanId tbiPlan
            | None ->
                match TbiPlanIdParser.tryParseTbiTreatmentPlanId planId with
                | Some tbiTreatmentPlan -> TbiTreatmentPlanId tbiTreatmentPlan
                | None -> InvalidPlanId

