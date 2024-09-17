namespace VMS.TPS

//open Definitions

module BetaValues =
    
    // Dysfagi Beta Values
    let dysfagiIntercept = -4.5710
    
    let oralCavityBeta = 0.0341
    let pcmUpBeta      = 0.0267
    let pcmMidBeta     = 0.0107
    let pcmLowBeta     = 0.0151
    
    // Constants for DysfagiBaseline
    let grad01BaselineBeta = 1.0000
    let grad2BaselineBeta  = 1.0627
    let grad35BaselineBeta = 1.4610

    // Constants for DysfagiTumorPlacering
    let pharynxBetaValue       = -0.7115
    let larynxOrOtherBetaValue = -0.8734

    // Xerostomi Beta Values
    let xerostomiIntercept = -4.3613
    
    let submandibularTotalBeta   = 0.0193
    let parotidsDmeanSumSqrtBeta = 0.1054

    // Constants for XerostomiBaseline
    let Nothing       = 1.0000
    let aBitBeta      = 0.5234
    let quiteABitBeta = 1.2763

    // Accessor functions for tumor placement beta values
    let getTumorPlaceringBeta (tumorPlacering: DysfagiTumorPlacering) =
        match tumorPlacering with
        | Pharynx -> pharynxBetaValue
        | LarynxOrOther -> larynxOrOtherBetaValue

    // Accessor functions for dysfagi baseline beta values
    let getDysfagiBaselineBeta (dysfagiBaseline: DysfagiBaseline) =
        match dysfagiBaseline with
        | Grad01 -> 1.0
        | Grad2 -> grad2BaselineBeta
        | Grad345 -> grad35BaselineBeta

    // Accessor functions for xerostomi baseline beta values
    let getXerostomiBaselineBeta (xerostomiBaseline: XerostomiBaseline) =
        match xerostomiBaseline with
        | Nothing -> 1.0
        | ABit -> aBitBeta
        | QuiteABit -> quiteABitBeta

    // Organ beta values accessor
    let getOrganBeta (organ: string) =
        match organ with
        | "extendedOralCavity" -> oralCavityBeta
        | "pcmUp" -> pcmUpBeta
        | "pcmMid" -> pcmMidBeta
        | "pcmLow" -> pcmLowBeta
        | "submandibularTotal" -> submandibularTotalBeta
        | _ -> failwith "Invalid organ"

    


