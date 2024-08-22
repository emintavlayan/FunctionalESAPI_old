// File: TbiPlanTypes.fs
namespace VMS.TPS

open System.Windows.Forms
open FsToolkit.ErrorHandling

/// Defines types necessary for constructing and representing TBI (Total Body Irradiation) Plan IDs and related entities.
module Utility =

    // Function to show a message box with an error message
    let showErrorMessageBox (error: string) : unit =
        MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

    // Function to map errors to error messages and display them
    let handleResultError (result: Result<'a, string>) : Result<'a, unit> =
        result
        |> Result.mapError showErrorMessageBox
