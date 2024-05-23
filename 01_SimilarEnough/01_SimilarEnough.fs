namespace VMS.TPS

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

/// Contains functions and active patterns for comparing and processing identifiers.
module IdComparison = 
    
    /// <summary>
    /// Strips non-alphanumeric characters from the input string using regular expressions for improved performance.
    /// This method avoids the overhead of manually iterating through each character and using StringBuilder.
    /// </summary>
    /// <param name="input">The string to process.</param>
    /// <returns>A string containing only alphanumeric characters.</returns>
    let onlyAlphaNumerics (input: string) =

        Regex.Replace(input.Trim(), "[^a-zA-Z0-9]", "")

    /// <summary>
    /// Active pattern for checking if two strings are equal when stripped of non-alphanumeric characters.
    /// </summary>
    let (|EqualAlphaNumeric|_|) s1 s2 =
        
        if s1 = s2 
        then Some () 
        else None

    /// <summary>
    /// Active pattern for checking if both strings are variations of 'bowel', including 'BowelBag' or 'SmallBowel'.
    /// </summary>
    let (|IsBowel|_|) (s1:string) (s2:string) =
        
        if  (s1.Contains("bowel")) && (s2.Contains("bowel"))
        then Some () 
        else None
    
    /// <summary>
    /// Active pattern to check if two strings are the same but with a direction suffix, such as 'left' or 'right'.
    /// This performance optimized version checks for suffix presence and compares the base of the string.
    /// </summary>
    /// <param name="s1">First string to compare.</param>
    /// <param name="s2">Second string to compare.</param>
    /// <returns>Some() if matched with a suffix, None otherwise.</returns>
    let (|HasLeftRightSuffix|_|) (s1:string) (s2:string) = 
        
        // Checks if a string ends with 'l' or 'r'
        let hasSuffix (input: string) = input.EndsWith("l") || input.EndsWith("r")

        // Determines if 'a' is the base of 'b', where 'b' should have the suffix 'l' or 'r'
        // Uses short-circuit evaluation to skip substring operation if 'b' does not have the expected suffix
        let isBaseOf a b = hasSuffix(b) && a = b.Substring(0, b.Length - 1)
        
        match s1, s2 with
        | _ when isBaseOf s2 s1 -> Some ()
        | _ when isBaseOf s1 s2 -> Some ()
        | _ -> None 
   

    /// <summary>
    /// Determines if two strings are considered similar enough based on a series of checks.
    /// This function includes null checks at the beginning to handle potential null values,
    /// which is particularly important for interoperability with C# where nulls are more common.
    /// </summary>
    /// <param name="stringA">First string to compare.</param>
    /// <param name="stringB">Second string to compare.</param>
    /// <returns>Boolean indicating if the strings are similar enough.</returns>
    let similarEnough (stringA: string) (stringB: string) =
        
        // Check for null values first to prevent C# people from frustration
        // Because C# exercises "null reference exceptions" ?!?!
        match stringA, stringB with
        | null, _ -> false
        | _, null -> false
        | _, _ ->

            // Convert strings to lowercase and trim for uniform comparison
            let s1 = onlyAlphaNumerics(stringA).ToLowerInvariant()
            let s2 = onlyAlphaNumerics(stringB).ToLowerInvariant()
        
            // Match strings against various patterns to determine similarity
            match s1 with
            | EqualAlphaNumeric(s2)  -> true
            | IsBowel (s2)           -> true
            | HasLeftRightSuffix(s2) -> true
            | _ -> false

type StringExtensions() =
    /// <summary>
    /// Provides a C# friendly way to access the 'similarEnough' functionality from F#.
    /// This extension method allows for a natural usage pattern in C#,
    /// making it possible to call it on string instances directly.
    /// 
    /// - Include the F# DLL in your C# project, 
    /// - add a 'using StringExtensions;' directive.
    /// </summary>
    /// <example>
    /// Call this function from C# like this:
    /// - SimilarEnough("string1", "string2"); 
    /// or
    /// - "string1".SimilarEnough("string2");
    /// </example>
    [<Extension>]
    static member SimilarEnough (str1: string, str2: string) : bool =
        IdComparison.similarEnough str1 str2

