# Similar Enough

Pure F# script to check if the structure identifiers are similar enough (instead of exact match), designed to be integrated into a big C# project.

### Situation

We have a giant C# script to check Plan Quality Metrics of every treatment plan before delivery. 

Besides dose, fractionation, beam naming etc. this script also checks if dose-volume cosntraints of sturcturea are met by comparing the values gathered from the plan to the values in an SQL database table of desired values.

### Problem

When searching the database current script was using Equality operator on Structure ids, ie. Structure_Id_1.Equals(Structure_Id_2). Even if we put great effort in standardizing the names, in the clinical practice it is likely we have typos, forgotten suffixes or somebody used "underscore" instead of "dash" kind of mismatches. 

### Solution

We have developed a small F# script to be more flexible than absolute match. Since F# is still .Net environment interoperability is not a problem at all. And this was an oppurtunity to use matching capabilities of Functional Programming.

### What we get

While deveoping this script i had the chance to learn **Option Type**, **Active Patterns** and **C# Interoperability**.

**Option Type** is great to deal with the possibility of Null. It is basically a box wrapper around normal types which have 2 compartments, Some and None. You can carry this box around but you need to open it to see if it carries a value ( Some value) or empty (None). 

**Active Patterns** are at the core of Pattern Matching for me because they let us define very complicated logic seperately. Later on we can use them together to create simple modular flow, easy to read and reason about. In this script I have used partial active patterns to catch different versions of similar structure names.