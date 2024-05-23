// File: TreatmentCodeTypes.fs
namespace VMS.TPS

/// Contains domain types specifically for treatment code functionality.
/// This file is intended for defining all types related to treatment codes.
module TreatmentCodeTypes =

    /// Represents the unique identifier for a treatment code.
    type TreatmentCodeId = TreatmentCodeId of string

    /// Represents the description associated with a treatment code.
    type Description = Description of string

    /// Represents any comments associated with a treatment code.
    type Comments = Comments of string

    /// Represents the fractionation information of a treatment code.
    type Fractionation = Fractionation of string

    /// Represents any additional notes associated with a treatment code.
    type AdditionalNote = AdditionalNote of string

    /// Represents an optional type for comments to handle the presence or absence of comments.
    /// Not every treatment code might have comments associated with it.
    type CommentsOption = CommentsOption of Comments option

    /// Represents an optional type for additional notes to handle the presence or absence of additional notes.
    /// Not every treatment code might have additional notes.
    type AdditionalNoteOption = AdditionalNoteOption of AdditionalNote option

    /// Represents the core domain entity for a treatment code, aggregating all associated details.
    /// This type is used to represent and manipulate treatment codes within the domain.
    /// VOLTRAN
    type TreatmentCode = {
        TreatmentCodeId: TreatmentCodeId
        Description: Description
        Comments: CommentsOption
        Fractionation: Fractionation
        AdditionalNote: AdditionalNoteOption
    }

    