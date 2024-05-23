// File: Patterns.fs
namespace VMS.TPS

open System.Text.RegularExpressions

/// Contains regular expression patterns for validating and parsing TBI Plan IDs.
module Patterns =

    /// Regular expression pattern to match PlanSetup.Id for TBI Planning Plan Names.
    /// The pattern enforces a specific format: 
    /// - Begins with 2 to 3 letters representing the alphabetic part of the Treatment Code.
    /// - Followed by exactly 3 digits representing the numeric part.
    /// - Optionally followed by whitespace and then 'v' plus one or more digits indicating the version number.
    let tbiPlanningPlanIdPattern = Regex(@"^([A-Za-z]{2,3}\d{3})\s*(v\d+)$")
    
    /// Regular expression pattern to match PlanSetup.Id for TBI Treatment Plan IDs.
    /// The pattern enforces a format that includes:
    /// - An alphabetic part (2 to 3 letters) followed by a numeric part (3 digits).
    /// - An optional starting dose, which may include decimals, followed by a dash.
    /// - A total dose, which may also include decimals.
    /// - Optionally followed by whitespace and then 'v' plus digits indicating the version number.
    let tbiTreatmentPlanIdPattern = Regex(@"^([A-Za-z]{2,3}\d{3})\s*(\d+(?:\.\d+)?)-(\d+(?:\.\d+)?)\s*(v\d+)$")
    
