namespace VMS.TPS

open System
open System.IO
open System.Diagnostics
open System.Text
open System.Windows.Forms
open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types

//test
[<System.Runtime.CompilerServices.CompilerGeneratedAttribute>]
type Script() =
    member __.Execute(context: ScriptContext) = 

        // Get the structure set from the context
        let structureSet: StructureSet = context.StructureSet

        // Filter high-resolution structures which are not empty
        let highResolutionStructures: Structure seq =
            structureSet.Structures
            |> Seq.filter (fun (structure: Structure) ->
                not structure.IsEmpty && 
                structure.HasSegment && 
                structure.IsHighResolution)

        // Create html table rows from data
        let toHtmlTable (structures: Structure seq) =
            let bodyRows =
                structures
                |> Seq.map (fun (structure: Structure) -> sprintf "<tr><td>%s</td></tr>" structure.Id)
                |> List.ofSeq
                |> List.fold (+) ""
            sprintf "<!DOCTYPE html>\n<html>\n<head>\n<style>\nbody {\n  margin-left: 5%%;\n  margin-right: 5%%;\n  font-family: sans-serif;\n}\n\nh1 {\n  display: block;\n  font-size: 2em;\n  margin-block-start: 0.67em;\n  margin-block-end: 0.67em;\n  margin-inline-start: 0px;\n  margin-inline-end: 0px;\n  font-weight: bold;\n  margin-left: -3%%;\n}\n\ntable {\n  font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;\n  border: 2px solid blue;\n  border-collapse: collapse;\n  text-indent: initial;\n  white-space: normal;\n  line-height: normal;\n  font-weight: normal;\n  font-style: normal;\n  text-align: start;\n  border-spacing: 2px;\n  font-variant: normal;\n}\n\ntd, th {\n  font-size: 1.17em;\n  border: 1px solid blue;\n  padding: 3px 7px 2px 7px;\n  text-align: left;\n  padding: 8px;\n  width: 120px;\n}\nth {background-color: lightgray;}\n</style>\n</head>\n<body>\n<h1>\nHigh Resolution Structures\n</h1>\n<table>%s</table>\n</body>\n</html>" bodyRows

        // Write table as html and show 
        let writeHtmlTableToFile (filename: string) (structures: Structure seq) =
            let html = toHtmlTable structures
            let fullPath = Path.Combine(Path.GetTempPath(), filename)
            File.WriteAllText(fullPath, html, Encoding.UTF8) // Create or overwrite the file with the new content
            let psi = new ProcessStartInfo(fullPath)
            psi.UseShellExecute <- true
            Process.Start(psi) |> ignore

        // Write the HTML table to file and open it in the default web browser
        writeHtmlTableToFile "highResStructures.html" highResolutionStructures
