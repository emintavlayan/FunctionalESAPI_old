namespace VMS.TPS

open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling

/// Unified result type for the module
type StructureOperationResult<'T> = Result<'T, StructureOperationError>

and StructureOperationError =
    | StructureNotFound of string
    | StructureEmpty of string
    | CopyVolumeFailed of string
    | UnexpectedError of string

module StructureOperations =

    /// Finds a structure by ID within a StructureSet.
    let findStructureById (id: string) (ss: StructureSet) : StructureOperationResult<Structure> =
        match ss.Structures |> Seq.tryFind (fun s -> s.Id = id) with
        | Some structure -> Ok structure
        | None -> Error (StructureNotFound $"Structure with ID {id} not found.")

    /// Checks if a structure has significant volume.
    let validateStructureVolume (structure: Structure) : StructureOperationResult<Structure> =
        if not structure.IsEmpty && structure.HasSegment && structure.Volume > 0.001 then
            Ok structure
        else
            Error (StructureEmpty $"Structure {structure.Id} does not have significant volume.")

    /// Main function to search, validate, and copy structure volume.
    let copyStructureVolume (ss: StructureSet) (sourceId: string) (targetId: string) : StructureOperationResult<string> =
        result {
            let! sourceStructure = findStructureById sourceId ss |> Result.bind validateStructureVolume
            let! targetStructure = findStructureById targetId ss

            if targetStructure.IsEmpty then
                targetStructure.SegmentVolume <- sourceStructure.SegmentVolume
                ss.RemoveStructure(sourceStructure)
                return $"Volume from {sourceId} has been copied to {targetId} successfully."
            else
                return! Error (CopyVolumeFailed $"Target structure {targetId} already has volume.")
        }
