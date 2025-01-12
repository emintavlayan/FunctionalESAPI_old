// xdata should be acomplex type that will hold all all relevant info
// the next steps can be none so in js null
// dto will be constructed as linear dictionary

type currentPlan = {
  PatientId : Patient.Id
  CourseId : Course.Id
  PlanId :  Plan.Id
}

// folder will be read for tbi dicom images
type TbiImageFileName = TbiImageFileName of string

// when creating this name we have constarints
// must start with RI and extension dicom
let createTbiImageFileName (s : string) : Result<TbiImageFileName, string> =
  if s.Startswith("RI") and s.Endswith(".dcm")
  then Ok (TbiImageFileName s)
  else Error "filename does not fit"


// step 1 planselection
// url selection with editable?

// step 2 analyzing files
// filter files that blong to this patient

let isTbiImageFileName (s : string) : bool =
  if s.Startswith("RI") and s.Endswith(".dcm")

// so I can use files |> List.Filter isTbiImageFileName
// show as table
// filename / field Id / datetime / patient Id / 

type TbiImageFile = { 
  Filename : TbiImageFileName
  FieldId : string
  AcquisitionDate : DateTime
  PatientId : Patient.Id
}

type TbiImageFileDTO = {
 FileName : string
 FieldId : string
 AcquisitionDate : string
 PatientId : string
}

// Json parser library 
// decode thoth.json encode auto will decode this DTO only

// functions from and to domain
let toTbiImageFileDTO (tif : TbiImageFile) : TbiImageFileDTO =
  let fn = tif.FileName
