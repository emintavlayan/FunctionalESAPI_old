# Functional Scripting Design Guide (F# + ESAPI)

This guide defines the design principles for clean, composable, and robust F# scripting — with or without ESAPI. It applies to all scripts within this solution to ensure maintainability, clarity, and safe error handling.

---

## 1. Explicit Data Flow with `Result` and `Validation`

- All operations that can fail return `Result<'T, string>` or `Validation<'T, string list>`
- This enables safe, predictable composition
- Failures are never hidden or thrown unless explicitly handled

```fsharp
let trySetPrescription (dose: float) (plan: PlanSetup) =
    try
        plan.SetPrescription(1, DoseValue(dose, "Gy"), 1)
        Ok plan
    with ex ->
        Error $"Failed to set prescription: {ex.Message}"
```

Avoid:
- Throwing exceptions from core logic
- Returning raw exceptions
- Relying on null or option types for failure signaling

---

## 2. `result {}` Computation Expressions for Clean Error Propagation

- Compose multiple dependent steps clearly
- Short-circuits on first failure
- Makes function flow linear and easy to read

```fsharp
result {
    let! patient = tryGetPatient ctx
    let! course = tryGetCourse ctx
    let! plan = tryFindPlanByIdPattern course "HH"
    return plan
}
```

Avoid:
- Deep match nesting
- Manual branching over Result

---

## 3. Pure Functions Where Possible

- Functions take explicit inputs and return outputs
- Avoid context-bound behavior in inner logic
- All non-IO logic is testable and referentially transparent

```fsharp
let createModifiedPlan course originalPlan imagePlan =
    result {
        let! copied = copyPlanToNewImage course originalPlan imagePlan
        let! modified =
            copied
            |> trySetPrescription 2.0
            |> Result.bind (trySetCalculationModel "AcurosXB")
        return modified
    }
```

Avoid:
- Using mutable state
- Writing to UI or file system in logic functions

---

## 4. Minimal State, No Mutation Unless Necessary

- Use immutable values by default
- Only mutate when the external API (e.g., ESAPI) requires it
- Limit the scope of modifications explicitly

```fsharp
let! patient = tryGetPatient ctx
do patient.BeginModifications()
```

Avoid:
- Mutable bindings unless required
- In-place updates across shared state

---

## 5. Layered Structure

Scripts should follow this logical layering:

Main.fs        // Entry point: binds execution, shows results  
Workflow.fs    // Core workflow logic, composed of smaller units  
Other Modules  // Supporting layers, e.g. Utilities, DomainTypes, PlanOps  

- Main.fs: Only constructs context and calls workflow functions
- Workflow.fs: Defines the domain-specific flow for the script
- Lower modules: Hold reusable operations or helpers

Avoid:
- Putting logic directly in Main.fs
- Mixing context acquisition and processing in the same function

---

## 6. Error Messaging with Context

- Errors are labeled with identifying information (IDs, names)
- Enables direct traceability to source of failure

```fsharp
Error $"[Plan {plan.Id}] Failed to copy to new image: {ex.Message}"
```

Avoid:
- Empty or generic messages like "Error occurred" or "Something failed"

---

## 7. Commented Toggle Points

- Multiple variants (e.g., single vs batch) are kept in the code for demonstration or testing
- Toggled via comments — clear, explicit, and isolated

```fsharp
// --- SINGLE PLAN VERSION ---
// let! plan = createModifiedPlan course originalPlan (List.head imagePlans)
// do showMessageBox $"Created plan: {plan.Id}"

// --- MULTI PLAN VERSION ---
match createModifiedPlansFromDailyImages course originalPlan imagePlans with
| Validation.Ok plans -> ...
```

Avoid:
- Duplicating logic between versions
- Removing working versions used for demos or reference

---

## 8. Naming Reflects Intention

- Function names clearly describe their purpose and behavior
- Prefer explicit prefixes:
  - tryGetX: returns Result
  - createX: builds a new instance
  - checkX: validates or asserts
  - showX: triggers side effects

```fsharp
let tryFindPlanByIdPattern (course: Course) (pattern: string) = ...
```

Avoid:
- Vague names like runStep, processIt, handleX
- Omitting verbs in function names

---
