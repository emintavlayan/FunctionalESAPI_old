module VMS.TPS.Util

module Result =

    let ofOption errorValue value =
        match value with
        | Some value -> Ok value
        | None -> Error errorValue
