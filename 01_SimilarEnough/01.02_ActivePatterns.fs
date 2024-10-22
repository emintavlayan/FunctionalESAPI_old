namespace VMS.TPS

open System
open System.Text.RegularExpressions
open System.Runtime.CompilerServices

/// Contains active patterns for comparing strings.
/// note: all input strings will be converted to lowercase alphanumerics,
///       before being consumed by Active Patterns.
module ActivePatterns = 
    
    /// Checks if two strings are equal.
    let (|Equal|_|) (s1:string) (s2: string) =
        if s1.Equals(s2) 
        then Some ()  // Option type 
        else None     // valubale stuff

    /// Checks if both strings are variations of 'bowel'
    /// example: 'Bowel', 'BowelBag' or 'SmallBowel'.
    let (|BothAreBowel|_|) (s1:string) (s2: string) =
        if  (s1.Contains("bowel")) && (s2.Contains("bowel"))
        then Some () 
        else None
    
    /// Checks if two strings are the same except one have a direction suffix.
    /// example: ( Parotis, Parotis-L) (Lung_Right, Lung)
    let (|HasLeftRightSuffix|_|) (s1:string) (s2: string) = 
        
        // Checks if a string ends with a direction suffix
        let hasDirectionSuffix (s: string)= 
            s.EndsWith("l")    || 
            s.EndsWith("left") || 
            s.EndsWith("r")    || 
            s.EndsWith("right")

        // Strips the string off the direction suffix
        let baseOf (b: string)=
            if b.EndsWith("left") 
            then b.Substring(0, b.Length - 4)  // remove last 4 chars
            elif b.EndsWith("right") 
            then b.Substring(0, b.Length - 5)  // or 5
            else b.Substring(0, b.Length - 1)  // or just one for 'l' , 'r'
        
        // All the action starts here
        if (hasDirectionSuffix(s1)) && (s2 = baseOf s1)  // s1 equals the base of s2
        then Some ()  
        elif (hasDirectionSuffix(s2)) && (s1 = baseOf s2)
        then Some ()  
        else None

    /// Define the next best active pattern here
    /// ---
        
