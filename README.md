# FunctionalESAPI

**A suite of ESAPI scripts**, crafted entirely in F#, adhering to Domain-Driven Design principles. This approach is driven by a passion for creating a better, more efficient development process, aiming to enhance collaboration among team members, streamline the workflow, and encourage the creation of innovative solutions.
<br><br>

## Definitions:

- **ESAPI:** Short for the Scripting API of the Varian Eclipse Treatment Planning System (TPS). It is used to automate processes, add new functionalities, and extract data insights.

- **TPS:** A computer system designed to assist in creating optimal Radiotherapy plans. Key inputs include patients' 3D DICOM imaging and Linac commissioning data.

- **Radiotherapy:** A cancer treatment method that targets tumors with radiation, primarily generated and shaped by linear accelerators (**Linac**). Due to the high-energy nature of this radiation, medical physicists are responsible for commissioning, quality assurance, and authorizing treatments using these medical devices.

- **Medical Physicist:** A professional with a university degree in Physics, followed by a master's degree in healthcare, specializing in the application and management of radiation in therapeutic settings.

- **Me:** A medical physicist at Copenhagen University Hospital in Denmark. 
<br><br>

## Why Functional Programming:

- **Physics Background:** Our training is steeped in pure functions, making functional programming intuitive. Pure means, functions don't create mess outside their context; same input causes same output. 

- **Safety and Simplicity:** Provides a safer approach to problem-solving with a lightweight mindset. At the end its just functions and data.

- **First-Class Functions:** Functions can be composed, half-baked, passed as arguments, and manipulated with ease.

- **Pattern Matching and Lists:** Offers a cleaner alternative to traditional control structures. You dont have to write any "if statements" or "for loops". Take your time, let that sink in. 

- **Strong Typing:** Ensures everything is checked before compile time. If I can write it, it works.

- **FP in OOP:** People are already making significant efforts to use functional programming principles in Object Oriented first languages like C#. You maybe already using some of them, its called LINQ. Why not have the whole package then?

- **OOP in FP:** F# goes beyond just being a Functional Programming Language. You can structure your code in a clean, purely functional manner and then incorporate objects if necessary. F# can be more object-oriented than C# can ever be functional!
<br><br>

## Why F#:

- **Elegant and Powerful:** F# is a functional-first language which emphasizes immutability, higher-order functions, and concise syntax.F# combines beauty and power with an expressive syntax. Developed and maintained by Microsoft, so it will be around for a long time.

- **Seamless Integration:** .NET compiles to the same IL, ensuring compatibility with the TPS. So Eclipse won't even notice which language you have used.

- **Broad .NET Ecosystem:** Access to everything .NET offers, including machine learning, user interfaces, and more.
<br><br>

## Why Domain-Driven Design:

- **Expert Alignment:** We are domain experts by definition. Period. There is a strong argument why computer scientist should learn the domain very well in order to help solving problems. we are going to the other direction, from knowing the domain very well to learn code and solve problems. DDD is exactly what we need.

- **Code as Documentation:** The codebase serves as clear documentation, enhancing understanding and maintenance. Here I will try to use clear meaningful function names and seperate concerns in small maintainable modules

- **Enhanced Communication:** Facilitates better collaboration between developers and domain experts. I trust all my collegues, without any coding background, will be able to read and understand what the code does. And I find this very secure way of having feedbacks.
<br><br> 

## How it looks:

- **Less is more:** no paranthesis, no curly braces; indentation is enough. Just let your non-coding friends read this. 

```fsharp
/// Module for searching and validating structures within a StructureSet.
module StructureSearch =

    /// Checks if a structure has significant volume.
    let structureHasVolume structure = 
        not structure.IsEmpty && 
        structure.HasSegment &&
        structure.Volume > 0.001

    /// Finds non-empty PTV structures with a dose level in Id.
    let findNonEmptyPTVs structureSet =
        structureSet.Structures
        |> Seq.filter (fun structure -> structure.Id.Contains("PTV"))
        |> Seq.filter structureHasVolume
```
<br>

- **End Game** is hopefully our codes will look like this ReadMe file, bite-size functions with clear naming, encapsulated in small modules. This way, I believe we can grow into a bigger community of developers by breaking barriers and onboarding friends, even those who don't know programming.
<br> 

## Resources:

- [F# Software Foundation](https://fsharp.org/)

- [F# for Fun and Profit](https://fsharpforfunandprofit.com/) by Scott Wlaschin

- [Domain Modeling Made Functional](https://pragprog.com/titles/swdddf/domain-modeling-made-functional/) by Scott Wlaschin

- [Essential F#](https://leanpub.com/essential-fsharp) by Ian Russell

- [Get Programming with F#: A guide for .NET developers](https://www.manning.com/books/get-programming-with-f-sharp) by Isaac Abraham
