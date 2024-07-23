namespace VMS.TPS

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

/// Contains functions and active patterns for comparing and processing identifiers.
module RegexUtils = 
    
    /// <summary>
    /// Strips non-alphanumeric characters from the input string using regular expressions for improved performance.
    /// This method avoids the overhead of manually iterating through each character and using StringBuilder.
    /// </summary>
    /// <param name="input">The string to process.</param>
    /// <returns>A string containing only alphanumeric characters.</returns>
    let onlyAlphaNumerics (input: string) =

        Regex.Replace(input.Trim(), "[^a-zA-Z0-9]", "")

    