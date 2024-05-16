// File: DomainErrors.fs
namespace VMS.TPS

open TreatmentCodeTypes

/// Represents various errors that can occur within the treatment domain.
module DomainErrors =

    /// Defines errors related to Treatment Codes operations.
    type TreatmentCodeError =
        /// Represents the error when a specified Treatment Code cannot be found.
        | NotFound of treatmentCodeId: TreatmentCodeId
