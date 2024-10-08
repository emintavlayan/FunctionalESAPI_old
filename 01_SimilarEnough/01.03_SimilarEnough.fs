namespace VMS.TPS

open System
open System.Runtime.CompilerServices

open ActivePatterns
open RegexUtils

/// Contains functions and active patterns for comparing and processing identifiers.
module IdComparison = 
    
    /// Determines if two strings are considered similar enough based on a series of checks.
    let similarEnough s1 s2 =

        // Convert strings to lowercase alphanumerics for uniform comparison
        let a = onlyAlphaNumerics(s1).ToLowerInvariant()
        let b = onlyAlphaNumerics(s2).ToLowerInvariant()
        
        // Match strings against defined active patterns to determine similarity
        match a with
        | Equal(b)              -> true
        | BothAreBowel(b)       -> true
        | HasLeftRightSuffix(b) -> true
        // Use the next best active pattern here
        | _ -> false

type StringExtensions() =
    /// Provides a C# friendly way to access the 'similarEnough' functionality from F#.
    /// This extension method allows for a natural usage pattern in C#,
    ///   making it possible to call it on string instances directly.
    /// To make this work:
    ///   - Include the F# DLL in your C# project, 
    ///   - add a 'using StringExtensions;' directive.
    /// Call this function from C# like this:
    ///   - SimilarEnough(string1, string2); 
    /// or
    ///   - string1.SimilarEnough(string2);
    [<Extension>]
    static member SimilarEnough (str1: string, str2: string) : bool =
        // Check for null values first to prevent C# nullq
        match str1, str2 with
        | null, _ -> false
        | _, null -> false
        | _, _ -> IdComparison.similarEnough str1 str2
