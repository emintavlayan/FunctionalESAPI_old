namespace VMS.TPS

open System
open System.Windows
open System.Windows.Media.Media3D
open Utils

open VMS.TPS.Common.Model.API
open VMS.TPS.Common.Model.Types
open FsToolkit.ErrorHandling


module ApertureGuess =

    
    let fiftyPercent : DoseValue =
        new DoseValue(50, DoseValue.DoseUnit.Percent)
        
     
    let guessApertureDiameters (d: Dose) =
        
        let meshOpt = 
            d.Isodoses 
            |> Seq.tryFind (fun iso -> iso.Level = fiftyPercent)
            |> Option.map (fun iso -> iso.MeshGeometry)

        match meshOpt with
        | Some(mesh) ->
            let slicedPositions = 
                mesh.Positions
                |> Seq.filter (fun p -> p.Y = 0.0)

            // Calculate bounding box or further operations on slicedPositions
            // Calculate bounding box
            let minX = slicedPositions |> Seq.minBy (fun p -> p.X)
            let maxX = slicedPositions |> Seq.maxBy (fun p -> p.X)
            let minZ = slicedPositions |> Seq.minBy (fun p -> p.Z)
            let maxZ = slicedPositions |> Seq.maxBy (fun p -> p.Z)
    
            let diameterX = maxX.X - minX.X
            let diameterZ = maxZ.Z - minZ.Z
            Ok (diameterX, diameterZ)

        | None -> 
            Error "no 50% dose mesh for old men"

    