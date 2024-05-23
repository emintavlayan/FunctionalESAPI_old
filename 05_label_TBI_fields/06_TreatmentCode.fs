// File: TreatmentCode.fs
namespace VMS.TPS

open TreatmentCodeTypes
open TbiTreatmentCodeDb

module TbiTreatmentCode = 

    // ex: TB005. with pattern matching
    module TreatmentCodeId =

        let create (code: string) : Result<TreatmentCodeId, string> =
            let existsInDatabase = 
                tbiTreatmentCodes
                |> List.exists (fun tc -> tc.TreatmentCodeId = TreatmentCodeId code)
        
            match existsInDatabase with
            | true -> Ok (TreatmentCodeId code)
            | false -> Error (sprintf "We don't recognize this TBI code: '%s'." code)

    // ex: Mini helkropsbestråling
    module Description = 

        let create description =
            Some (Description description)

    // ex: Retransplanteret med ubeslægtet donor
    module Comments = 

        let create comments =
            Some (Comments comments)

    // ex: 3 Gy x 1 - 1 F/W
    module Fractionation = 

        let create fractionation =
            Some (Fractionation fractionation)

    // ex: None
    module AdditionalNote = 

        let create note =
            Some (AdditionalNote note)