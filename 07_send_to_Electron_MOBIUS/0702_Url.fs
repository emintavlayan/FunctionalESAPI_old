namespace VMS.TPS

open System
open System.Diagnostics
open System.Web // This provides HttpUtility for URL encoding in .NET Framework

module Url =

    // Function to URL-encode a string (for special characters in the query parameters)
    let urlEncode (input: string) = HttpUtility.UrlEncode(input)

    // Function to build the URL with query parameters based on inputs
    let generateUrl
        (baseUrl: string)
        (patientName: string)
        (patientId: string)
        (machineName: string)
        (beamEnergy: string)
        (applicator: string)
        (cutoutWidth: string)
        (cutoutLength: string)
        (ssd: string)
        (depthPct: string)
        (beamDose: string)
        (beamName: string)
        (tpsMu: string)
        : string =

        // Query parameters for all fields (including patient name and id)
        let queryParams =
            [ "patientName_str", urlEncode patientName
              "patientId_str", urlEncode patientId
              "machineName_str0", urlEncode machineName
              "electronEnergy_int0", beamEnergy
              "conesize_cm0", urlEncode applicator
              "cutoutWidth_cm0", cutoutWidth
              "cutoutLength_cm0", cutoutLength
              "electronSsd_cm0", ssd
              "electronDepth_pct0", depthPct
              "electronBeamDose_cgy0", beamDose
              "beamName_str0", urlEncode beamName
              "tpsMu_int0", tpsMu ]

        // Joins the parameters to create the query string
        let queryString =
            queryParams
            |> List.map (fun (key, value) -> sprintf "%s=%s" key value)
            |> String.concat "&"

        // Returns the full URL
        url + "?" + queryString

    // Opens a URL in the default browser
    let openUrlInBrowser (url: string) =
        let psi = new ProcessStartInfo(url)
        psi.UseShellExecute <- true
        Process.Start(psi) |> ignore
