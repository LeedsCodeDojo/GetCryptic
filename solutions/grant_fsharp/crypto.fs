module Crypto

open System

let sixBitNumberToBase64Character = function
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
    | 23 -> 'Z'
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
    | x -> failwith (sprintf "Invalid Base 64 number: '%d'" x)

let hexDigitToNyble = function
    | '0' -> 0
    | '1' -> 1
    | '2' -> 2
    | '3' -> 3
    | '4' -> 4
    | '5' -> 5
    | '6' -> 6
    | '7' -> 7
    | '8' -> 8
    | '9' -> 9
    | 'a' -> 10
    | 'b' -> 11
    | 'c' -> 12
    | 'd' -> 13
    | 'e' -> 14
    | 'f' -> 15
    | x -> failwith (sprintf "Invalid Hex Character: '%c'" x)

let nybleToHexDigit = function
    | 0 -> '0'
    | 1 -> '1'
    | 2 -> '2'
    | 3 -> '3'
    | 4 -> '4'
    | 5 -> '5'
    | 6 -> '6'
    | 7 -> '7'
    | 8 -> '8'
    | 9 -> '9'
    | 10 -> 'a'
    | 11 -> 'b'
    | 12 -> 'c'
    | 13 -> 'd'
    | 14 -> 'e'
    | 15 -> 'f'
    | x -> failwith (sprintf "Invalid Nyble: %d" x)

let hexToNybles (hex:string) =
    hex 
    |> Seq.toList
    |> List.map hexDigitToNyble

let nyblesToHex nybles =
    nybles
    |> List.map nybleToHexDigit
    |> Array.ofList
    |> String

let xorAgainstCharacter nyble nybles = 
    nybles 
    |> List.map (fun n -> n ^^^ nyble)

let xorList nybles1 nybles2 =
    List.zip nybles1 nybles2
    |> List.map (fun (n1, n2) -> n1 ^^^ n2)

let fillMissingNyblesWithZeroes = function
    | [n1;n2] -> [n1;n2;0]
    | [n1] -> [n1;0;0]
    | threeFullNybles -> threeFullNybles

let concatenateNybles (nybles: int list) =
     (nybles.[0] <<< 8) + (nybles.[1] <<< 4) + nybles.[2]

let threeNyblesToTwoSixBitNumbers (threeNybles: int) =
    let SIX_BIT_MASK = 63 // 00111111
    [ (threeNybles >>> 6) &&& SIX_BIT_MASK; threeNybles &&& SIX_BIT_MASK]

let hexToBase64 (hex:string) =
    hex 
    |> hexToNybles
    |> List.chunkBySize 3
    |> List.map fillMissingNyblesWithZeroes
    |> List.map concatenateNybles
    |> List.map threeNyblesToTwoSixBitNumbers
    |> List.concat
    |> List.map sixBitNumberToBase64Character
    |> List.toArray
    |> String

let findXorCypher (hex:string) =

    let encryptedNybles = hex |> hexToNybles

    //[0..63]
    //|> List.map (fun cypher -> encryptedNybles |> xor cypher)

    'a'