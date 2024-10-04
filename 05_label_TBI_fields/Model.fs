// Model.fs
namespace VMS.TPS

open NodeTypes

module Model =

    // Represents the application's state, holding all UI elements as a list of nodes.
    type Model = {
        Nodes: Node list  // A list of all UI elements
        ViewNeedsUpdate: bool  // Flag to indicate if the view needs to be updated
    }

    // Initializes the model with default values for each UI element.
    let initModel = {
        Nodes = [
            Dropdown { Changed = false; Id = "dropdown1"; Selection = ""; Options = ["Option1"; "Option2"; "Option3"] }
            Dropdown { Changed = false; Id = "dropdown2"; Selection = ""; Options = ["OptionA"; "OptionB"; "OptionC"] }
            Section { Changed = false; Id = "section1"; IsVisible = true }
            Section { Changed = false; Id = "section2"; IsVisible = false }
            TextArea { Changed = false; Id = "reportStep2"; Text = "" }
        ]
        ViewNeedsUpdate = false
    }


