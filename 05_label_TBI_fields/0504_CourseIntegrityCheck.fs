namespace VMS.TPS

open System.Windows.Forms
open VMS.TPS.Common.Model.API
open Utility

type IntegrityResult<'T> = Result< 'T, IntegrityError >

and IntegrityError =
    | TreatmentCodesDontMatch of string
    | PlanVersionsDontMatch of string
    | Unex of string


/// gets a pair of plan ids
/// tries to create tbi planpair type
/// chek for integrity
/// - same beams and MU for both plans etc.
module CourseIntegrityCheck =
    
    
    
    let compareFirst5Chars (s1: string) (s2: string) : bool =

        first5Chars s1 = first5Chars s2
