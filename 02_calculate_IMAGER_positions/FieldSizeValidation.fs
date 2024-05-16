namespace VMS.TPS

open System.Windows.Forms

module FieldSizeValidation =
    
    // Function to check if Setup Field size is 18x18cm
    let CheckFieldSizes (id: string) (x1: float) (x2: float) (y1: float) (y2: float) =
        let showMessage msg = MessageBox.Show(msg) |> ignore
        let isValidSize sumX sumY = ( sumX = 18.0 ) && ( sumY = 18.0 )
        let sumSizes x1 x2 y1 y2 = (x1 + x2, y1 + y2)

        match id.ToLower().Contains("set"), sumSizes x1 x2 y1 y2 with
        | true, (sumX, sumY) when not (isValidSize sumX sumY) -> 
            showMessage $"Expected Field Size for {id} is 18x18cm."
        | _ -> ()
