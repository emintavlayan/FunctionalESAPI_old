namespace VMS.TPS.FunctionalESAPI

open System
open System.IO
open System.Diagnostics
open System.Text
open VMS.TPS.Common.Model.API
open FieldSizeValidation

/// Represents a setup beam with imager positions.
type SetupBeamWithImagerPosition = {
    Id : string
    X1 : float
    X2 : float
    Y1 : float
    Y2 : float
    ImagerVrt : float
    ImagerLng : float
    ImagerLat : float  
}

module ImagerPositionCalculation =
    
    /// Main function to calculate imager positions for setup beams.
    /// This function is intended to be used from both Eclipse and C# environments.
    /// - `context`: The script context containing the plan setup.
    let calculate_IMAGER_positions (context : ScriptContext) = 
        
        // Get the plan setup from the context
        let plan = context.PlanSetup
        
        /// Function to calculate lateral imager position based on jaw positions.
        /// - `x1`: The X1 jaw position.
        /// - `x2`: The X2 jaw position.
        let calculateImagerLat x1 x2 =
            // Calculate the absolute shift needed
            let shiftLat = (abs (x1 - x2) / 2.0) * 1.6 // Because the imager is at 60.0cm

            // Apply constraints to the shift
            let constrainedShiftLat =
                match shiftLat with
                | a when a < 1.0 -> 0.0    // If the shift is less than 1cm, don't apply any shift
                | a when a > 13.0 -> 13.0  // The maximum shift we can apply is 13cm
                | _ -> shiftLat

            // Calculate the imager lateral position based on direction
            let imagerLat =
                if ( constrainedShiftLat > 0.0 ) && ( x1 > x2 ) then
                    1000.0 - constrainedShiftLat 
                else
                    constrainedShiftLat

            // Return the imager lateral position
            imagerLat

        /// Function to calculate longitudinal imager position based on jaw positions.
        /// - `y1`: The Y1 jaw position.
        /// - `y2`: The Y2 jaw position.
        let calculateImagerLng y1 y2 =
            // Calculate the absolute shift needed
            let shiftLng = (abs (y1 - y2) / 2.0) * 1.6

            // Apply constraints to the shift
            let constrainedShiftLng =
                match shiftLng with
                | a when a < 1.0 -> 0.0    // If the shift is less than 1cm, don't apply any shift
                | a when a > 13.0 -> 13.0  // The maximum shift we can apply is 13cm
                | _ -> shiftLng

            // Calculate the imager longitudinal position based on direction
            let imagerLng =
                if ( constrainedShiftLng > 0.0 ) && ( y2 > y1 ) then 
                    1000.0 - constrainedShiftLng
                else    
                    constrainedShiftLng

            // Return the imager longitudinal position
            imagerLng
        
        // Filter the beams that are SetupFields ( but not CBCT ) or the first field
        // because we also take an MV image for the 1st beam
        let setupBeams = 
            plan.Beams
            |> Seq.filter (fun beam -> 
                ( beam.IsSetupField && not (beam.Id.ToLower().Contains("cbct")) )
                ||
                    ( 
                    beam.Id.StartsWith("1 ") || // Possible variations of naming the 1st beam
                    beam.Id.StartsWith("1m") || 
                    beam.Id.StartsWith("01") 
                    )  
                )

        // Calculate the imager positions for each setup beam
        // and output as a list of the record type
        let setupBeamsWithImagerPositions : SetupBeamWithImagerPosition list =
            setupBeams
            |> Seq.map (fun beam ->
                // Get the Beam Id
                let id = beam.Id

                // Get the jaw positions for the current beam
                let x1 = beam.ControlPoints.[0].JawPositions.X1 / (-10.0) // this comes negative from the system
                let x2 = beam.ControlPoints.[0].JawPositions.X2 /   10.0  // 
                let y1 = beam.ControlPoints.[0].JawPositions.Y1 / (-10.0) // and also this
                let y2 = beam.ControlPoints.[0].JawPositions.Y2 /   10.0  // divide by ten to convert from mm to cm

                // Check if input values are valid
                WarnIfSetupFieldSizeNot18cm beam.Id x1 x2 y1 y2
        
                // Calculate the imager lateral and longitudinal positions
                let imagerLng = calculateImagerLng y1 y2
                let imagerLat = calculateImagerLat x1 x2
                
                // Create a setup beam record with the input values and calculated imager positions
                { Id = id; X1 = x1; X2 = x2; Y1 = y1; Y2 = y2; ImagerVrt = 60.0; ImagerLng = imagerLng; ImagerLat = imagerLat })
            |> List.ofSeq // Convert seq<setupBeam> to setupBeam list

        /// Helper function to check if a string contains all specified substrings.
        /// - `s`: The string to check.
        /// - `substrings`: The list of substrings to check for.
        let containsThese (s: string) (substrings: string list) =
            substrings |> List.forall (fun x -> s.ToLower().Contains(x))

        /// Helper function to check if a string does not contain any of the specified substrings.
        /// - `s`: The string to check.
        /// - `substrings`: The list of substrings to check for.
        let doesNotContainThose (s: string) (substrings: string list) =
            substrings |> List.forall (fun x -> not (s.ToLower().Contains(x)))

        /// Function to determine the sorting order of a setup beam based on its ID.
        /// - `sb`: The setup beam to sort.
        /// Result not guaranteed because I cannot get field order from ECL 
        let sortByOrder (sb: SetupBeamWithImagerPosition) =
            let id = sb.Id
            match id with
            | _ when containsThese id ["set"; "up"; "0"  ] && doesNotContainThose id ["boost"; "180"; "270"] -> 0
            | _ when containsThese id ["set"; "up"; "180"] && doesNotContainThose id ["boost"] -> 1
            | _ when containsThese id ["set"; "up"; "270"] && doesNotContainThose id ["boost"] -> 2
            | _ when id.StartsWith("1 ") || 
                        id.StartsWith("1m") || 
                        id.StartsWith("01") -> 3
            | _ when containsThese id ["set"; "up"; "0";   "boost"] && doesNotContainThose id ["180"; "270"] -> 4
            | _ when containsThese id ["set"; "up"; "180"; "boost"] -> 5
            | _ when containsThese id ["set"; "up"; "270"; "boost"] -> 6
            | _ -> 7

        /// Function to sort a list of setup beams.
        /// - `lst`: The list of setup beams to sort.
        let sortList (lst: SetupBeamWithImagerPosition list) =
            lst |> List.sortBy sortByOrder  

        // Ordered Setup beams with imager
        let orderedSetupBeamsWithImager = sortList setupBeamsWithImagerPositions

        /// Function to get a specific property value from a setup beam.
        /// - `property`: The property name.
        /// - `beam`: The setup beam.
        let getValue (property: string) (beam: SetupBeamWithImagerPosition) =
            match property with
            | "X1" -> beam.X1
            | "X2" -> beam.X2
            | "Y1" -> beam.Y1
            | "Y2" -> beam.Y2
            | "ImagerVrt" -> beam.ImagerVrt
            | "ImagerLng" -> beam.ImagerLng
            | "ImagerLat" -> beam.ImagerLat
            | _ -> invalidArg "property" "Invalid property"

        /// Function to create an HTML table from a list of setup beams.
        /// - `beams`: The list of setup beams.
        let toHtmlTable (beams: SetupBeamWithImagerPosition list) =
            let filteredBeams, medBeam = beams |> List.partition (fun beam -> beam.Id.ToLower().Contains("med"))
            let sortedBeams = medBeam @ filteredBeams
                
            let headerRow = sprintf "<tr><th></th>%s</tr>" (String.concat "" [for beam in sortedBeams -> sprintf "<th>%s</th>" beam.Id])
            let bodyRows =
                [
                    yield! List.map (fun prop ->
                        sprintf "<tr><td style=\"width: 100px;\">%s</td>%s</tr>" prop (String.concat "" [for beam in sortedBeams -> sprintf "<td>%.1f</td>" (getValue prop beam)])
                    ) ["X1"; "X2"; "Y1"; "Y2"; "ImagerVrt"; "ImagerLng"; "ImagerLat"]
                ]
                |> List.fold (+) ""
            sprintf "<!DOCTYPE html>\n<html>\n<head>\n<style>\nbody {\n  margin-left: 5%%;\n  margin-right: 5%%;\n  font-family: sans-serif;\n}\n\nh1 {\n  display: block;\n  font-size: 2em;\n  margin-block-start: 0.67em;\n  margin-block-end: 0.67em;\n  margin-inline-start: 0px;\n  margin-inline-end: 0px;\n  font-weight: bold;\n  margin-left: -3%%;\n}\n\ntable {\n  font-family: 'Trebuchet MS', Arial, Helvetica, sans-serif;\n  border: 2px solid blue;\n  border-collapse: collapse;\n  text-indent: initial;\n  white-space: normal;\n  line-height: normal;\n  font-weight: normal;\n  font-style: normal;\n  text-align: start;\n  border-spacing: 2px;\n  font-variant: normal;\n}\n\ntd, th {\n  font-size: 1.17em;\n  border: 1px solid blue;\n  padding: 3px 7px 2px 7px;\n  text-align: left;\n  padding: 8px;\n width: 120px;\n}\nth {background-color: lightgray;}\n</style>\n</head>\n<body>\n<h1>\nImager Positions\n</h1>\n<table>%s%s</table>\n</body>\n</html>" headerRow bodyRows

        /// Function to write the HTML table to a file and open it.
        /// - `filename`: The name of the file.
        /// - `beams`: The list of setup beams.
        let writeHtmlTableToFile (filename: string) (beams: SetupBeamWithImagerPosition list) =
            let html = toHtmlTable beams
            let fullPath = Path.Combine(Path.GetTempPath(), filename)
            File.WriteAllText(fullPath, html, Encoding.UTF8) // Create or overwrite the file with the new content
            let psi = new System.Diagnostics.ProcessStartInfo(fullPath)
            psi.UseShellExecute <- true
            Process.Start(psi) |> ignore

        // Write
        let filename = String.Format("setupBeams{0}.html", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"))

        writeHtmlTableToFile filename orderedSetupBeamsWithImager


