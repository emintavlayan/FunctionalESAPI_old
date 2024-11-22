namespace VMS.TPS

open VMS.TPS.Common.Model.API

//// Assembly attribute to indicate the script can write data
//[<assembly: ESAPIScript(IsWriteable = true)>]
//do()

/// This type of Running the code is imposed by Varian Esapi library.
[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    member __.Execute(context: ScriptContext) = 
        
        context.Patient.BeginModifications()
        
        ElectronMobius.run(context)