namespace VMS.TPS

open VMS.TPS.Common.Model.API

// Assembly attribute to indicate the script can write data
[<assembly: ESAPIScript(IsWriteable = true)>]
do()

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    member __.Execute(context: ScriptContext) = 
        
        context.Patient.BeginModifications()

        OnboardAiStructures.Run(context)
       


        
