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
