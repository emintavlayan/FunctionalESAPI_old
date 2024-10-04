namespace VMS.TPS

open System
open System.Windows
open Utils

open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types
open FsToolkit.ErrorHandling


module ApertureGuess =

    // Function to calculate the distance between two points
    let distance 
        (p1: Point) 
        (p2: Point) : float =

        Math.Sqrt((p2.X - p1.X) ** 2.0 + (p2.Y - p1.Y) ** 2.0)

    // Function to estimate the semi-major and semi-minor axes from an array of points
    let estimateCenter 
        (points: Point[]) : Point =

        let centerX = 
            points 
            |> Array.averageBy (fun p -> p.X)

        let centerY = 
            points 
            |> Array.averageBy (fun p -> p.Y)

        Point(centerX, centerY)

    // Estimate semi-major and semi-minor axes by taking max and min distances from the center
    let estimateDiameters 
        (points: Point[]) : float * float =

        let center = estimateCenter points

        let distancesFromCenter = 
            points 
            |> Array.map (fun p -> distance center p)

        let majorDiameter = 
            distancesFromCenter 
            |> Array.max
            |> (*) 2.0

        let minorDiameter = 
            distancesFromCenter 
            |> Array.min
            |> (*) 2.0

        majorDiameter, minorDiameter

    let tryAddStructure 
        (ss: StructureSet)  
        (id: string) : Result<unit, string> = 

        try
            ss.AddStructure("PTV", id) |> ignore
            Ok ()
        with
        | ex -> 
            Error $"Could not add structure to ss: {ex.Message}"

    let trySelectStructure 
        (ss: StructureSet)  
        (id: string) : Result<Structure, string> =

        try
            let s =
                ss.Structures 
                |> Seq.find (fun s -> s.Id = id)
            Ok s
        with
        | ex -> 
            Error $"Could not select structure: {ex.Message}"

    let tryConvertDoseToStructure
        (str: Structure) 
        (dose: Dose) 
        (doseValue: DoseValue) : Result<unit, string> =

        try
            str.ConvertDoseLevelToStructure(dose, doseValue)
            |> ignore
            Ok ()
        with
        | ex -> 
            Error $"Could not convert dose level into structure: {ex.Message}"

    let tryGetStructureOutlines
        (beam: Beam)  
        (str: Structure) : Result<Point array, string> =

        try
            let points =
                beam.GetStructureOutlines(str, false)
            Ok points.[0]
        with
        | ex -> 
            Error $"Could not get structure outlines: {ex.Message}"

    let tryRemoveStructure
        (ss: StructureSet)
        (str: Structure) : unit =
        
        try 
            ss.RemoveStructure(str)
        with
        | _ -> 
            warnAndQuit "Not able to delete dose structure, but will continue..."
            ()
        
    
    let guessApertureDiametersCE 
        (ss: StructureSet) 
        (beam: Beam) 
        (dose: Dose) : Result<float * float, string> = 
        
        result {

            // Tries to add a structure or ruins the whole process
            do! tryAddStructure ss "dose50"
            
            // Selects the newly created empty structure
            let! dose50 = trySelectStructure ss "dose50"
            
            // Dose value of 50 percent
            let fiftyPercent : DoseValue =
                new DoseValue(50, DoseValue.DoseUnit.Percent)

            // Tries to ...
            do! tryConvertDoseToStructure dose50 dose fiftyPercent

            let! points = tryGetStructureOutlines beam dose50

            let diameters = estimateDiameters points

            return diameters
        }
