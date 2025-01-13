namespace VMS.TPS

open System
open System.Reflection
open System.Windows.Forms
open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types

[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    member __.Execute(context: ScriptContext) =
        // Define the main form
        let form = new Form(Text = "Plancheck UI", Width = 300, Height = 150)

        // Define the button
        let button = new Button(Text = "Call Plancheck Script", Dock = DockStyle.Fill)

        // Add click event handler for the button
        button.Click.Add(fun _ ->
            try
                // Path to the external DLL (update this path)
                let dllPath = @"\\rghrhtransfp01\planscript\Fysiker\fysiker_plancheck_test.esapi.dll"

                // Load the external assembly dynamically
                let assembly = Assembly.LoadFrom(dllPath)

                // Get the Script type from the assembly
                let scriptType = assembly.GetType("VMS.TPS.Script")
                if scriptType = null then
                    failwith "Class 'VMS.TPS.Script' not found in DLL."

                // Create an instance of the Script class
                let scriptInstance = Activator.CreateInstance(scriptType)

                // Find the Execute method in the Script class
                let executeMethod = scriptType.GetMethod("Execute")
                if executeMethod = null then
                    failwith "Method 'Execute' not found in 'VMS.TPS.Script'."

                // Invoke the Execute method of the loaded DLL, passing the provided context
                executeMethod.Invoke(scriptInstance, [| context |]) |> ignore

                // Indicate success
                MessageBox.Show("Plancheck script executed successfully.", "Success") |> ignore
            with
            | ex -> MessageBox.Show($"Error: {ex.Message}", "Execution Failed") |> ignore
        )

        // Add the button to the form
        form.Controls.Add(button)

        // Show the form
        Application.Run(form)
