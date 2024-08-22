namespace VMS.TPS

open VMS.TPS.Common.Model.API
open FsToolkit.ErrorHandling
open System
open System.Windows.Forms
open System.Text.RegularExpressions

/// Unified result type for the module
type StructureOperationResult<'T> = Result<'T, StructureOperationError>

and StructureOperationError =
    | StructureNotFound of string
    | AiStructureIsEmpty of string
    | RhStructureIsNotEmpty of string
    | CopyVolumeFailed of string
    | RemoveStructureFailed of string
    | UnexpectedError of string

/// 
module StructureOperations =

    /// Strips non-alphanumeric characters from a string and converts it to lowercase.
    let normalizeString (input: string) : string =
        let alphanumericOnly = Regex.Replace(input, "[^a-zA-Z0-9]", "")
        alphanumericOnly.ToLower()
    
    /// Compares two strings after normalizing them by stripping non-alphanumeric characters.
    let areStringsEqual (str1: string) (str2: string) : bool =
        let normalizedStr1 = normalizeString str1
        let normalizedStr2 = normalizeString str2
        normalizedStr1 = normalizedStr2
        
    /// Finds a structure by a normalized ID within a StructureSet.
    let findStructureByIdNormalized (id: string) (ss: StructureSet) : StructureOperationResult<Structure> =
        match ss.Structures |> Seq.tryFind (fun s -> areStringsEqual s.Id id) with
        | Some structure -> Ok structure
        | None -> Error (StructureNotFound $"Structure with ID {id} not found.")

    /// Checks if a structure has volume and segment.
    let hasVolume (structure: Structure) : bool =
        not structure.IsEmpty && 
        structure.HasSegment  

    /// Checks if AI structure has significant volume.
    let validateAiStrHasVolume (structure: Structure) : StructureOperationResult<Structure> =
        if hasVolume structure 
        then Ok structure
        else Error (AiStructureIsEmpty $"Structure {structure.Id} does not have significant volume.")

    /// Checks if RH structure is empty.
    let validateRhStrIsEmpty (structure: Structure) : StructureOperationResult<Structure> =
        if not (hasVolume structure) 
        then Ok structure
        else Error (RhStructureIsNotEmpty $"Structure {structure.Id} is not empty.")

    /// Safely copies volume from one structure to another.
    let safeCopyVolume (source: Structure) (target: Structure) : Result<unit, StructureOperationError> =
        try 
            target.SegmentVolume <- source.SegmentVolume
            Ok ()
        with
        | ex -> 
            Error (CopyVolumeFailed $"Copy Volume operation failed:{ ex.Message}" ) 
    
    /// Removes a structure from a StructureSet if its DicomType is not empty.
    let safeRemoveStructure (ss: StructureSet) (structure: Structure) : unit =
        if not (String.IsNullOrWhiteSpace(structure.DicomType)) then
            ss.RemoveStructure(structure)

    ///// Main function to search, validate, and copy structure volume.
    let copyStructureVolume (ss: StructureSet) (aiId: string) (rhId: string) : StructureOperationResult<string> =

        result {
            // Find the AI structure by its ID and ensure it has significant volume
            let! aiStructure = 
                findStructureByIdNormalized aiId ss 
                |> Result.bind validateAiStrHasVolume
                
            // Find the RH structure by its ID and ensure it is empty
            let! rhStructure = 
                findStructureByIdNormalized  rhId ss 
                |> Result.bind validateRhStrIsEmpty
               
            // Safely copy the volume from the AI structure to the RH structure
            do! safeCopyVolume aiStructure rhStructure 
        
            // Attempt to remove aiStructure, but ignore any errors since copying is our main concern
            safeRemoveStructure ss aiStructure |> ignore
        
            return $"Volume has been copied successfully."
        } 
    