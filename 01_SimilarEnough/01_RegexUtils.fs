namespace VMS.TPS

open System
open System.Text.RegularExpressions

/// Contains functions and active patterns for comparing and processing identifiers.
module RegexUtils = 
    
    /// Strips non-alphanumeric characters from the input string using regular expressions.
    let onlyAlphaNumerics string =

        Regex.Replace(string, "[^a-zA-Z0-9]", "")
