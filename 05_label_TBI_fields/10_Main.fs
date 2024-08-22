namespace VMS.TPS

open VMS.TPS.Common.Model.API

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    
    member __.Execute(context: ScriptContext) = 
    
        // Initilazation
        let course = context.Course
        
        0