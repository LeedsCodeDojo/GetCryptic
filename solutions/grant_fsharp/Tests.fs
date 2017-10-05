module Tests

open System
open Xunit
open FsUnit.Xunit
open Crypto

[<Fact>]
let ``Hex to Base 64`` () =
   "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
   |> hexToBase64
   |> should equal "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t"

[<Fact>]
let ``Fixed XOR`` () =
   ("1c0111001f010100061a024b53535009181c" |> hexToBytes) |> xorList ("686974207468652062756c6c277320657965" |> hexToBytes)
   |> bytesToHex
   |> should equal "746865206b696420646f6e277420706c6179"

// [<Fact>]
// let ``Single character XOR cypher`` () =
//    let cyper = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736" |> findXorCypher
//    ("1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736" |> hexToNybles) |> xorAgainstCharacter (int cyper) |> should equal ""