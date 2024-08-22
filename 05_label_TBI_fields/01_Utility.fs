namespace VMS.TPS

open System.Windows.Forms
open FsToolkit.ErrorHandling

/// Defines types necessary for constructing and representing TBI (Total Body Irradiation) Plan IDs and related entities.
module Utility =

    // Gets the first 5 characters from a string
    let first5Chars (s : string) = 
        s.Substring(0,5)     
    
    // Shows a message box with an error message
    let showErrorMessageBox (error: string) : unit =
        MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore

    // Maps errors to error messages and display them
    let handleResultError (result: Result<'a, string>) : Result<'a, unit> =
        result
        |> Result.mapError showErrorMessageBox
