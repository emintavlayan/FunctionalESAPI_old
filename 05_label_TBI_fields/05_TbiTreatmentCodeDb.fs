// File: TbiTreatmentCodeDb.fs
namespace VMS.TPS

open TreatmentCodeTypes
open DomainErrors

/// This module represents an in-memory database for TBI Treatment Codes.
/// It also provides a basic FIND operation.
module TbiTreatmentCodeDb = 

    // Initializes an in-memory list to store treatment codes
    let tbiTreatmentCodes : TreatmentCode list = [
        { 
            TreatmentCodeId = TreatmentCodeId "TB002";
            Description = Description "Mini helkropsbestråling";
            Comments = CommentsOption (Some (Comments "Mini TBI"));
            Fractionation = Fractionation "2 Gy x 1 - 1 F/W";
            AdditionalNote = AdditionalNoteOption (None)
        };
        { 
            TreatmentCodeId = TreatmentCodeId "TB005";
            Description = Description "Mini helkropsbestråling";
            Comments = CommentsOption (Some (Comments "Retransplanteret med ubeslægtet donor"));
            Fractionation = Fractionation "3 Gy x 1 - 1 F/W";
            AdditionalNote = AdditionalNoteOption (None)
        };
        {
            TreatmentCodeId = TreatmentCodeId "TB006";
            Description = Description "Mini helkropsbestråling";
            Comments = CommentsOption (Some (Comments "transplanterede med stor risiko for komplikationer eller retransplanterede med beslægtet donor"));
            Fractionation = Fractionation "2 Gy x 2 - 5 F/W";
            AdditionalNote = AdditionalNoteOption (None)
        };
        {
            TreatmentCodeId = TreatmentCodeId "TB011";
            Description = Description "Helkropsbestråling";
            Comments = CommentsOption (None);
            Fractionation = Fractionation "2 Gy x 6 - 10 F/W";
            AdditionalNote = AdditionalNoteOption (None)
        };
        {
            TreatmentCodeId = TreatmentCodeId "TB017";
            Description = Description "Helkropsbestråling + testis";
            Comments = CommentsOption (None);
            Fractionation = Fractionation "2 Gy x 6 - 10 F/W";
            AdditionalNote = AdditionalNoteOption (Some (AdditionalNote "Testesbestråling 4 Gy x 1, dag 2 eller 3"))
        }
    ]

    /// Attempts to find a treatment code by its ID.
    /// <param name="id">The ID of the treatment code to find.</param>
    /// <returns>
    /// An option type that contains the treatment code if found, or None otherwise.
    /// </returns>
    let findTreatmentCodeById (id: TreatmentCodeId) : Result<TreatmentCode, TreatmentCodeError> =
        match tbiTreatmentCodes |> List.tryFind (fun tc -> tc.TreatmentCodeId = id) with
        | Some treatmentCode -> Ok treatmentCode
        | None -> Error (NotFound id)

