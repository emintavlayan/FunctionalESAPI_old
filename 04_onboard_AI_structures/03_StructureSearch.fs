namespace VMS.TPS

open VMS.TPS.Common.Model.API

/// Module for searching and validating structures within a StructureSet.
module structureSearch =

    /// Represents the result of searching for a structure.
    type StructureSearchResult =
    | ExistsAndHasVolume of Structure  // AI structure exists and has volume
    | ExistsAndEmpty of Structure      // RH structure exists and empty
    | StructureNotFound                // 
    | OtherIssue of string             // unforseen exceptions will be redirected

    /// Finds a structure by ID within a StructureSet.
    let getStructureById (id : string) (ss: StructureSet) : Structure option =
        ss.Structures |> Seq.tryFind (fun s -> s.Id = id)

    /// Checks if a structure has significant volume.
    let hasVolume (structure : Structure) = 
        not structure.IsEmpty && 
        structure.HasSegment &&
        structure.Volume > 0.001

    /// Validates a structure by ID, checking its existence and volume.
    let findStructureWithValidation (id : string) (ss : StructureSet) : StructureSearchResult =
        match getStructureById id ss with
        | Some str when hasVolume str -> ExistsAndHasVolume str
        | Some str                    -> ExistsAndEmpty str
        | None                        -> StructureNotFound
        
        