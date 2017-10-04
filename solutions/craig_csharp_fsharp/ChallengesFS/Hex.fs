module Hex

open System

let bytes_from_hex hex =
    hex
    |> Seq.map (function
        | '0' -> 0x0
        | '1' -> 0x1
        | '2' -> 0x2
        | '3' -> 0x3
        | '4' -> 0x4
        | '5' -> 0x5
        | '6' -> 0x6
        | '7' -> 0x7
        | '8' -> 0x8
        | '9' -> 0x9
        | 'a' -> 0xA
        | 'b' -> 0xB
        | 'c' -> 0xC
        | 'd' -> 0xD
        | 'e' -> 0xE
        | 'f' -> 0xF
        | _ -> raise (ArgumentException("Value is not a hex string")))
    |> Seq.map (byte)
    |> Seq.chunkBySize 2
    |> Seq.map (fun chunk -> Seq.head(chunk) <<< 4 ||| Seq.last(chunk))
    
let hex_from_bytes (bytes:seq<byte>) =
    let toHex = (function
        | 0x0 -> '0'
        | 0x1 -> '1'
        | 0x2 -> '2'
        | 0x3 -> '3'
        | 0x4 -> '4'
        | 0x5 -> '5'
        | 0x6 -> '6'
        | 0x7 -> '7'
        | 0x8 -> '8'
        | 0x9 -> '9'
        | 0xA -> 'a'
        | 0xB -> 'b'
        | 0xC -> 'c'
        | 0xD -> 'd'
        | 0xE -> 'e'
        | 0xF -> 'f'
        | _ -> raise (ArgumentException("Value must be between 0 and 15")))

    bytes 
    |> Seq.map (int)
    |> Seq.map(fun byte -> seq { 
        yield toHex((byte &&& 240) >>> 4)
        yield toHex(byte &&& 15) })
    |> Seq.concat