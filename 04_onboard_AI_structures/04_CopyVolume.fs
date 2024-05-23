namespace VMS.TPS

open VMS.TPS.Common.Model.API
open structureSearch

module CopyVolume =
    
    /// Represents the result of copying structure segment volume.
    type VolumeOperationResult =
        | VolumeCopiedSuccessfully of string
        | VolumeCopyFailed of string

    let copyVolume (ss : StructureSet) (aiId : string) (rhId : string) : VolumeOperationResult =
        let aiStr = findStructureWithValidation aiId ss 
        let rhStr = findStructureWithValidation rhId ss 

        match (aiStr, rhStr) with
        | (ExistsAndHasVolume aiStr, ExistsAndEmpty rhStr) -> 
            rhStr.SegmentVolume <- aiStr.SegmentVolume
            ss.RemoveStructure(aiStr)
            VolumeCopiedSuccessfully (sprintf "Volume from %s has been copied to %s successfully." aiId rhId)
        
        | (StructureNotFound, _) -> 
            VolumeCopyFailed "AI structure not present in StructureSet"
        
        | (ExistsAndEmpty _, _) -> 
            VolumeCopyFailed "AI structure does not have volume"
        
        | (OtherIssue msg, _) -> 
            VolumeCopyFailed (sprintf "AI encountered an error: %s" msg)
        
        | (_, StructureNotFound) -> 
            VolumeCopyFailed "RH structure not present in StructureSet"
        
        | (_, ExistsAndHasVolume _) -> 
            VolumeCopyFailed "RH structure already has some volume"
        
        | (_, OtherIssue msg) -> 
            VolumeCopyFailed (sprintf "RH encountered an error: %s" msg)


        
        