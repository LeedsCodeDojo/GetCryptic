open Crypto
open System

module Program = 
    let [<EntryPoint>] main _ = 
        
        [0..255]
        |> List.iter (fun cypher ->
           "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736"
           |> hexToBytes
           |> List.map (fun byte -> byte ^^^ cypher)
           |> List.map char
           |> String.Concat
           |> (printfn "%s"))
        

        0
