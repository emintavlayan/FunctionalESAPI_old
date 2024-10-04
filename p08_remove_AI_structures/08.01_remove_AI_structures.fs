namespace VMS.TPS

open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types
open System.Windows.Forms
open System.Text

// Assembly attribute to indicate the script can write data
[<assembly: ESAPIScript(IsWriteable = true)>]
do()

/// This type of Running the code is imposed by Varian Esapi library.
[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    member __.Execute(context: ScriptContext) = 
        
        context.Patient.BeginModifications()

        let ss = context.StructureSet

        // Filter AI structures
        let AI_structures = 
            ss.Structures
            |> Seq.filter (fun s -> s.Id.Contains("_AI_"))
            |> Seq.toList
        
        if AI_structures.Length > 0 then
            // Create a message with the AI structures' IDs
            let sb = StringBuilder()
            sb.AppendLine("The following AI structures are ready to be deleted:") |> ignore
            AI_structures |> List.iter (fun s -> sb.AppendLine(s.Id) |> ignore)
            
            // Show the message box and get user confirmation
            let result = MessageBox.Show(sb.ToString(), "Confirm Deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
            
            match result with
            | DialogResult.OK ->
                // User confirmed, remove the structures
                for s in AI_structures do
                    ss.RemoveStructure(s)
                MessageBox.Show("Structures deleted.", "Operation Completed", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            | DialogResult.Cancel ->
                // User canceled, do nothing
                MessageBox.Show("Deletion canceled.", "Operation Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
            | _ -> ()
        else
            // No AI structures to delete
            MessageBox.Show("No AI structures found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
