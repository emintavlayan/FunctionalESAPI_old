namespace VMS.TPS.FunctionalESAPI

open System.Windows.Forms

module FieldSizeValidation =
    
    /// Function to check if the field size is 18x18cm for setup fields.
    /// If not, a warning message is shown, but execution continues.
    /// Just a clinical tradition
    /// - `id`: The identifier of the field.
    /// - `x1`: The X1 position of the field.
    /// - `x2`: The X2 position of the field.
    /// - `y1`: The Y1 position of the field.
    /// - `y2`: The Y2 position of the field.
    let WarnIfSetupFieldSizeNot18cm (id: string) (x1: float) (x2: float) (y1: float) (y2: float) =

        /// Shows a message box with the given message.
        let showMessage msg = MessageBox.Show(msg) |> ignore

        /// Checks if the summed sizes are 18x18cm.
        let isInvalidSize (x1: float) (x2: float) (y1: float) (y2: float) =
            not ((x1 + x2 = 18.0) && (y1 + y2 = 18.0))

        /// Determines if the field ID indicates a setup field.
        let isSetupField (id: string) = id.ToLower().Contains("set")

        if isSetupField id && isInvalidSize x1 x2 y1 y2 
        then showMessage $"Expected Field Size for {id} is 18x18cm."
