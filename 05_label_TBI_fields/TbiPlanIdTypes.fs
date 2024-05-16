// File: TbiPlanIdTypes.fs
namespace VMS.TPS

open TreatmentCodeTypes

/// Defines types necessary for constructing and representing TBI Plan IDs.
module TbiTypes =

    /// Represents detailed dose information, including starting and total doses.
    type DoseInformation = {
        /// The starting dose of the treatment plan.
        StartingDose: StartingDose
        /// The total dose of the treatment plan.
        TotalDose: TotalDose
    }

    /// Represents the starting dose of a treatment plan.
    and StartingDose = StartingDose of double

    /// Represents the total dose of a treatment plan.
    and TotalDose = TotalDose of double

    /// Represents the version number of a treatment plan.
    type VersionNumber = VersionNumber of string
    
    /// Represents the ID for a TBI planning plan, consisting of a treatment code and a version number.
    type TbiPlanningPlanId = {
        /// The treatment code ID associated with the planning plan.
        TreatmentCodeId: TreatmentCodeId
        /// The version number of the planning plan.
        VersionNumber: VersionNumber
    }

    /// Represents the ID for a TBI treatment plan, including treatment code, dose information, and version number.
    type TbiTreatmentPlanId = {
        /// The treatment code ID associated with the treatment plan.
        TreatmentCodeId: TreatmentCodeId
        /// Detailed dose information for the treatment plan.
        DoseInformation: DoseInformation
        /// The version number of the treatment plan.
        VersionNumber: VersionNumber
    }

    type TbiCourse = TbiPlanningPlanId * TbiTreatmentPlanId 


