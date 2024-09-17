namespace VMS.TPS

open Url
open ApertureGuess
open Utils
open FsToolkit.ErrorHandling

open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types

module ElectronMobius =
    
    let run(context: ScriptContext) =

        context.Patient.BeginModifications()

        // Mobius Elektron Quick Calc Base Url
        let baseUrl = "http://mobiusfx/quick/electron"

        // Shortcuts wrapped in try-catch to handle potential exceptions
        let pt = tryMe context.Patient "No Patient"
        let ss = tryMe context.StructureSet "No StructureSet"
        let plan = tryMe context.ExternalPlanSetup "No Plan"
        let beam = tryMe (context.ExternalPlanSetup.Beams |> Seq.head) "No Beam"
        let dose = tryMe context.ExternalPlanSetup.Dose "No Dose"

        if not (plan.IsEntireBodyAndBolusesCoveredByCalculationArea()) then   
            warnAndQuit "Calculation Area does not cover entire Body and Boluses"

        let trySelectEnergy 
            (beam: Beam) : Result<string, string> =

            match beam.EnergyModeDisplayName with
            | "6E" -> Ok "6"
            | "9E" -> Ok "9"
            | "12E" -> Ok "12"
            | "16E" -> Ok "16"
            | _ -> Error "Beam Energy did not match 6E / 9E / 12E or 16E"

        let trySelectApplicator 
            (beam: Beam) : Result<string, string> =
            
            match beam.Applicator.Id with
            | "A06" -> Ok "6x6"
            | "A10" -> Ok "10x10"
            | "A15" -> Ok "15x15"
            | "A20" -> Ok "20x20"
            | "A25" -> Ok "25x25"
            | _ -> Error "Applicator is outside of expected values"
        
        // Single Computation Expression to either full working data or the first Error msg
        let urlCE = result {

            let patientName = pt.Name //cannot be null
            
            let patientId = pt.Id //cannot be null
            
            let machineName = beam.TreatmentUnit.Id //"Accelerator08" //cannot be null
            
            let! beamEnergy = trySelectEnergy beam

            let! applicator = trySelectApplicator beam
            
            let! width, length = guessApertureDiametersCE ss beam dose

            let cutoutWidth = (width * 0.1 |> roundTo1).ToString()
            let cutoutLength = (length * 0.1 |> roundTo1).ToString()

            let! ssd =
                match (beam.Boluses |> Seq.toList) with
                | [b] -> 
                    let ssdToBolusInCm = (beam.GetSourceToBolusDistance(b) * 0.1 |> roundTo1).ToString()
                    Ok (ssdToBolusInCm)
                | [] -> 
                    warn "No Bolus found on the beam. Really? \nCalculation will continue without bolus"
                    let ssdInCm = (beam.SSD * 0.1 |> roundTo1).ToString()
                    Ok (ssdInCm)
                | _ -> 
                    Error "We cannot calculate if there are more than one bolus on the beam" 
                
            let depthPct =
            
                let dmaxInPercent =
                    if plan.Dose.DoseMax3D.IsRelativeDoseValue then
                        plan.Dose.DoseMax3D.Dose
                    else
                        (plan.Dose.DoseMax3D.Dose / plan.TotalDose.Dose) * 100.0
            
                ((200.0 - dmaxInPercent) |> roundTo1 ).ToString()
            
            let beamDose =
                (plan.PlannedDosePerFraction.Dose * 100.0 ).ToString() // in cGy
               
            let beamName = beam.Id 
            
            let tpsMu = //cannot be null. dose is calculated
                (beam.Meterset.Value |> roundTo1).ToString()
                        
            // Generate the URL
            let url =
                generateUrl
                    baseUrl
                    patientName
                    patientId
                    machineName
                    beamEnergy
                    applicator
                    cutoutWidth
                    cutoutLength
                    ssd
                    depthPct
                    beamDose
                    beamName
                    tpsMu

            return url
            
        }

        match urlCE with
        | Ok url -> openUrlInBrowser url
        | Error err -> err |> logAndWarn


