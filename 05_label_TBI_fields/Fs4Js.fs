namespace VMS.TPS


module Fs4Js =

    // Utility function to convert an F# list to a JavaScript array string
    let createJsArrayString (values: string list) =
        values 
        |> List.map (fun v -> sprintf "'%s'" v) 
        |> String.concat ", "
        // use this like: let values = [%s];
     
    // Generate JavaScript to populate both dropdowns and set initial values
    let createScript list ppDropdown tpDropdown pp tp= 
        sprintf """
        let values = [%s];
        let dropdown1 = document.getElementById('%s');
        let dropdown2 = document.getElementById('%s');
        dropdown1.innerHTML = '';
        dropdown2.innerHTML = '';
        for (let i = 0; i < values.length; i++) {
            let option1 = document.createElement('option');
            let option2 = document.createElement('option');
            option1.value = option2.value = values[i];
            option1.text = option2.text = values[i];
            dropdown1.appendChild(option1);
            dropdown2.appendChild(option2);
        }
        dropdown1.value = '%s';
        dropdown2.value = '%s';
        """ list ppDropdown tpDropdown pp tp

        

