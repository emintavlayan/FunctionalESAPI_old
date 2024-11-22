namespace VMS.TPS

open VMS.TPS.Common.Model.API
open FunctionalESAPI.ImagerPositionCalculation

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    
    member __.Execute(context: ScriptContext) = 
    
        calculate_IMAGER_positions(context)  