open Crypto

module Program = 
    let [<EntryPoint>] main _ = 
        printfn "%s" ("abc123" |> hexToBase64)
        0
