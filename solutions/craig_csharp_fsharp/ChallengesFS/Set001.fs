module Challenges

open NUnit.Framework
open Base64
open Hex
open Text
open Xor
open Score
open System.IO

type Challenge01() = 
    [<Test>]
    member this.Solution() = 
        let expectedBase64 = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t"
        let hex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        let base64 = hex |> bytes_from_hex |> base64_from_bytes

        Assert.AreEqual(expectedBase64, base64)


    [<Test>]
    member this.ConvertHex() = 
        let expectedHex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d"
        let hex = expectedHex |> bytes_from_hex |> hex_from_bytes
        CollectionAssert.AreEqual(expectedHex, hex)

    [<Test>]
    member this.ConvertText() = 
        let expectedText = "Man"
        let text = expectedText |> bytes_from_text |> text_from_bytes
        CollectionAssert.AreEqual(expectedText, text)


    [<TestCase("Man", "TWFu")>]
    [<TestCase("M", "TQ==")>]
    [<TestCase("Ma", "TWE=")>]
    member this.ConvertBase64(input, expected) = 
        let textBytes = input |> bytes_from_text 

        let base64 = textBytes |> base64_from_bytes
        CollectionAssert.AreEqual(base64, expected)

        let toBytes = base64 |> bytes_from_base64
        CollectionAssert.AreEqual(toBytes, textBytes)
        
type Challenge02() = 
    [<Test>]
    member this.Solution() =
        let bytes1 = "1c0111001f010100061a024b53535009181c" |> bytes_from_hex
        let bytes2 = "686974207468652062756c6c277320657965" |> bytes_from_hex
        
        Assert.AreEqual("746865206b696420646f6e277420706c6179", (xor_by_sequence bytes1 bytes2) |> hex_from_bytes)

type Challenge03() = 
    [<Test>]
    member this.Solution() =
        let bytes = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736" |> bytes_from_hex
        
        let answer = 
            (xor_with_every_byte bytes)
            |> Seq.map text_from_bytes
            |> Seq.map score_using_english
            |> Seq.maxBy (fun (score, _) -> score)
            |> (fun (_, found) -> found)
            
        Assert.AreEqual(answer, "Cooking MC's like a pound of bacon")
    
//type Challenge04() = 
//    [<Test>]
//    member this.Solution() =
//        let bestText = 
//            File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Set001Challenge004.txt"))
//            |> Seq.map bytes_from_base64
//            |> Seq.map xor_with_every_byte
//            |> Seq.concat
//            |> Seq.map text_from_bytes
//            |> Seq.map score_using_english
//            |> Seq.maxBy (fun (score, _) -> score)
//            |> (fun (_, text) -> text)
//        System.Console.WriteLine(bestText |> System.String.Concat)
//        Assert.AreEqual("qwewqe", bestText)

type Challenge05() = 
    [<Test>]
    member this.Solution() =
        let xor_ed = 
            (xor_by_sequence ("Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal" |> bytes_from_text) ("ICE" |> bytes_from_text)) 
            |> hex_from_bytes
        
        let expected = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f"
        Assert.AreEqual(expected, xor_ed)