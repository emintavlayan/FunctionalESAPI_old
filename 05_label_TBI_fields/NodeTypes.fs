// NodeTypes.fs
namespace VMS.TPS

module NodeTypes =

    // Represents a dropdown UI element with a selection and options.
    type DropdownNode = {
        Changed: bool  // Tracks if the node has been changed
        Id: string  // The HTML ID of the dropdown
        Selection: string  // The current selection
        Options: string list  // The list of options available
    }

    // Represents a section UI element that can be shown or hidden.
    type SectionNode = {
        Changed: bool  // Tracks if the node has been changed
        Id: string  // The HTML ID of the section
        IsVisible: bool  // Whether the section is visible or not
    }

    // Represents a textarea UI element that displays text.
    type TextAreaNode = {
        Changed: bool  // Tracks if the node has been changed
        Id: string  // The HTML ID of the textarea
        Text: string  // The current text displayed in the textarea
    }

    // A discriminated union that combines all possible node types.
    type Node =
        | Dropdown of DropdownNode
        | Section of SectionNode
        | TextArea of TextAreaNode
