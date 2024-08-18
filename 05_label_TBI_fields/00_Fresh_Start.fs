// File: TbiPlanIdTypes.fs
namespace VMS.TPS

/// Defines types necessary for constructing and representing TBI (Total Body Irradiation) Plan IDs and related entities.
module TbiTypes =

    /// Represents a TBI treatment code as a string value.
    type TreatmentCode = TreatmentCode of string
    
    /// Represents dose information as a string value.
    type DoseInformation = DoseInformation of string

    /// Represents the version number of a treatment plan as a string value.
    type VersionNumber = VersionNumber of string
    
    /// Represents the ID for a TBI planning plan, consisting of a treatment code and a version number.
    type TbiPlanningPlanId = {
        /// The treatment code ID associated with the planning plan.
        TreatmentCodeId: TreatmentCode
        /// The version number of the planning plan.
        VersionNumber: VersionNumber
    }

    /// Represents the ID for a TBI treatment plan, including treatment code, dose information, and version number.
    type TbiTreatmentPlanId = {
        /// The treatment code ID associated with the treatment plan.
        TreatmentCodeId: TreatmentCode
        /// Detailed dose information for the treatment plan.
        DoseInformation: DoseInformation
        /// The version number of the treatment plan.
        VersionNumber: VersionNumber
    }

    /// Represents a unique identifier for a TBI beam as a string value.
    type TbiBeamId = TbiBeamId of string

    /// Represents a TBI planning plan, including its ID, associated beams, and whether the beams are ordered.
    type TbiPlanningPlan = {
        /// The ID of the planning plan.
        PlanId: TbiPlanningPlanId
        /// The list of beam IDs associated with the planning plan.
        BeamIds: TbiBeamId list
        /// A boolean value indicating whether the beams are ordered.
        BeamsAreOrdered: bool
    }

    /// Represents a TBI treatment plan, including its ID, associated beams, and whether the beams are ordered.
    type TbiTreatmentPlan = {
        /// The ID of the treatment plan.
        PlanId: TbiTreatmentPlanId
        /// The list of beam IDs associated with the treatment plan.
        BeamIds: TbiBeamId list
        /// A boolean value indicating whether the beams are ordered.
        BeamsAreOrdered: bool
    }

    /// Represents a course of TBI treatment, consisting of a planning plan and a treatment plan.
    type TbiCourse = TbiPlanningPlan * TbiTreatmentPlan
