namespace VMS.TPS

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

/// Contains functions and active patterns for comparing and processing identifiers.
/// note: all input strings will be converted to lowercase and only alphanumerics
///       before being consumed by Active Patterns.
module ActivePatterns = 
    
    /// Checks if two strings are equal.
    let (|Equal|_|) s1 s2 =
        if s1 = s2 
        then Some ()  // Option type 
        else None     // valubale stuff

    /// Checks if both strings are variations of 'bowel'
    /// example: 'Bowel', 'BowelBag' or 'SmallBowel'.
    let (|BothAreBowel|_|) s1 s2 =
        
        if  (s1.Contains("bowel")) && (s2.Contains("bowel"))
        then Some () 
        else None
    
    /// Checks if two strings are the same but
    ///   one of them have a direction suffix, such as 'L' or 'Right'.
    ///   example: ( Parotis, Parotis-L) (Lung_Right, Lung)
    let (|HasLeftRightSuffix|_|) s1 s2 = 
        
        // Checks if a string ends with a direction suffix
        let hasDirectionSuffix s = 
            s.EndsWith("l")    || 
            s.EndsWith("left") || 
            s.EndsWith("r")    || 
            s.EndsWith("right")

        // Strips the string off the direction suffix
        let baseOf b =

            if b.EndsWith("left") 
                then b.Substring(0, b.Length - 4)  // remove last 4 chars

            elif b.EndsWith("right") 
                then b.Substring(0, b.Length - 5)  // or 5
             
            else b.Substring(0, b.Length - 1)      // or just one for 'l' , 'r'
        
        // All the action starts here
        if (hasDirectionSuffix(s1)) &&    // s1 has a suffix and
            (s2 = baseOf s1)              // s2 equals the base of s2
            then Some ()  

        elif (hasDirectionSuffix(s2)) &&  // reverted version
            (s1 = baseOf s2)
            then Some ()  

        else None  
        