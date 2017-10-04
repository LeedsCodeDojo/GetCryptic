module Base64

open System

let bytes_from_base64 (base64:seq<char>) = 
    let fromBase64 = (function
        | 'A' ->  Some ((byte)0)
        | 'B' ->  Some ((byte)1)
        | 'C' ->  Some ((byte)2)
        | 'D' ->  Some ((byte)3)
        | 'E' ->  Some ((byte)4)
        | 'F' ->  Some ((byte)5)
        | 'G' ->  Some ((byte)6)
        | 'H' ->  Some ((byte)7)
        | 'I' ->  Some ((byte)8)
        | 'J' ->  Some ((byte)9)
        | 'K' ->  Some ((byte)10)
        | 'L' ->  Some ((byte)11)
        | 'M' ->  Some ((byte)12)
        | 'N' ->  Some ((byte)13)
        | 'O' ->  Some ((byte)14)
        | 'P' ->  Some ((byte)15)
        | 'Q' ->  Some ((byte)16)
        | 'R' ->  Some ((byte)17)
        | 'S' ->  Some ((byte)18)
        | 'T' ->  Some ((byte)19)
        | 'U' ->  Some ((byte)20)
        | 'V' ->  Some ((byte)21)
        | 'W' ->  Some ((byte)22)
        | 'X' ->  Some ((byte)23)
        | 'Y' ->  Some ((byte)24)
        | 'Z' ->  Some ((byte)25)
        | 'a' ->  Some ((byte)26)
        | 'b' ->  Some ((byte)27)
        | 'c' ->  Some ((byte)28)
        | 'd' ->  Some ((byte)29)
        | 'e' ->  Some ((byte)30)
        | 'f' ->  Some ((byte)31)
        | 'g' ->  Some ((byte)32)
        | 'h' ->  Some ((byte)33)
        | 'i' ->  Some ((byte)34)
        | 'j' ->  Some ((byte)35)
        | 'k' ->  Some ((byte)36)
        | 'l' ->  Some ((byte)37)
        | 'm' ->  Some ((byte)38)
        | 'n' ->  Some ((byte)39)
        | 'o' ->  Some ((byte)40)
        | 'p' ->  Some ((byte)41)
        | 'q' ->  Some ((byte)42)
        | 'r' ->  Some ((byte)43)
        | 's' ->  Some ((byte)44)
        | 't' ->  Some ((byte)45)
        | 'u' ->  Some ((byte)46)
        | 'v' ->  Some ((byte)47)
        | 'w' ->  Some ((byte)48)
        | 'x' ->  Some ((byte)49)
        | 'y' ->  Some ((byte)50)
        | 'z' ->  Some ((byte)51)
        | '0' ->  Some ((byte)52)
        | '1' ->  Some ((byte)53)
        | '2' ->  Some ((byte)54)
        | '3' ->  Some ((byte)55)
        | '4' ->  Some ((byte)56)
        | '5' ->  Some ((byte)57)
        | '6' ->  Some ((byte)58)
        | '7' ->  Some ((byte)59)
        | '8' ->  Some ((byte)60)
        | '9' ->  Some ((byte)61)
        | '+' ->  Some ((byte)62)
        | '/' ->  Some ((byte)63)
        | '=' -> None
        | _ -> raise (System.ArgumentException("Not a valid Base64 character")))
        
    base64
    |> Seq.map fromBase64
    |> Seq.chunkBySize 4
    |> Seq.map (fun chunk -> 
            let padding = 4 - (chunk |> Seq.choose id |> Seq.length)

            let paddingRemoved = 
                chunk
                |> Seq.map (function
                    | Some x -> x
                    | None -> ((byte)0))
            
            let encoded = Seq.fold(fun (acc:int) (elem:byte) ->  ((acc <<< 6) ||| (int)elem)   ) 0 paddingRemoved

            let first = (byte)((encoded &&& (255 <<< 16)) >>> 16)
            let second = (byte)((encoded &&& (255 <<< 8)) >>> 8)
            let third = (byte)((encoded &&& (255 <<< 0)) >>> 0)

            match padding with
            | 1 -> seq { 
                yield first
                yield second
                }
            | 2 -> seq {
                yield first
                }
            | _ -> seq {
                yield first
                yield second
                yield third
                }
        )
    |> Seq.concat
    
    

let base64_from_bytes (bytes:seq<byte>) =
    let toBase64 = (function
        | 0 -> 'A'
        | 1 -> 'B'
        | 2 -> 'C'
        | 3 -> 'D'
        | 4 -> 'E'
        | 5 -> 'F'
        | 6 -> 'G'
        | 7 -> 'H'
        | 8 -> 'I'
        | 9 -> 'J'
        | 10 -> 'K'
        | 11 -> 'L'
        | 12 -> 'M'
        | 13 -> 'N'
        | 14 -> 'O'
        | 15 -> 'P'
        | 16 -> 'Q'
        | 17 -> 'R'
        | 18 -> 'S'
        | 19 -> 'T'
        | 20 -> 'U'
        | 21 -> 'V'
        | 22 -> 'W'
        | 23 -> 'X'
        | 24 -> 'Y'
        | 25 -> 'Z'
        | 26 -> 'a'
        | 27 -> 'b'
        | 28 -> 'c'
        | 29 -> 'd'
        | 30 -> 'e'
        | 31 -> 'f'
        | 32 -> 'g'
        | 33 -> 'h'
        | 34 -> 'i'
        | 35 -> 'j'
        | 36 -> 'k'
        | 37 -> 'l'
        | 38 -> 'm'
        | 39 -> 'n'
        | 40 -> 'o'
        | 41 -> 'p'
        | 42 -> 'q'
        | 43 -> 'r'
        | 44 -> 's'
        | 45 -> 't'
        | 46 -> 'u'
        | 47 -> 'v'
        | 48 -> 'w'
        | 49 -> 'x'
        | 50 -> 'y'
        | 51 -> 'z'
        | 52 -> '0'
        | 53 -> '1'
        | 54 -> '2'
        | 55 -> '3'
        | 56 -> '4'
        | 57 -> '5'
        | 58 -> '6'
        | 59 -> '7'
        | 60 -> '8'
        | 61 -> '9'
        | 62 -> '+'
        | 63 -> '/'
        | _ -> raise (System.ArgumentException("Value must be between 0 and 63")))
        
    bytes
    |> Seq.chunkBySize 3
    |> Seq.map (fun chunk -> 
        let padding = 3 - Seq.length(chunk)
        let encoded = Seq.fold(fun (acc:int) elem -> ((acc <<< 8) ||| elem)) 0 (chunk |> Seq.map (int)) 
        let padded = encoded <<< (padding * 8)
        ( padding, padded ))
    |> Seq.map(fun (padding, toEncode) -> 
        let first = (toEncode &&& (63 <<< 18)) >>> 18
        let second = (toEncode &&& (63 <<< 12)) >>> 12
        let third = (toEncode &&& (63 <<< 6)) >>> 6
        let fourth = (toEncode &&& (63 <<< 0)) >>> 0
    
        match padding with
            | 1 -> 
                seq {
                    yield Some first
                    yield Some second
                    yield Some third
                    yield None
                }
            | 2 ->
                seq {
                    yield Some first
                    yield Some second
                    yield None
                    yield None
                } 
            | _ ->
                seq {
                    yield Some first
                    yield Some second
                    yield Some third
                    yield Some fourth
                }
    )
    |> Seq.concat
    |> Seq.map (function
        | Some x -> x |> toBase64
        | None -> '='
        )