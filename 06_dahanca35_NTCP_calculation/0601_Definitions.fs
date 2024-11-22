namespace VMS.TPS

type DysfagiTumorPlacering = 
    | Pharynx
    | LarynxOrOther

type DysfagiBaseline =
    | Grad01
    | Grad2
    | Grad345

type XerostomiBaseline =
    | Nothing
    | ABit
    | QuiteABit

type RisikoOrgan = {
    Id : string
    PhotonMeanDose : float
    ProtonMinimumPossibleMeanDose : float
    Beta : float
}

module Definitions =

    let RisikoOrganerIdList : string list = [
        "extendedOralCavity";
        "pcmUp";
        "pcmMid";
        "pcmLow";

    ]


    

