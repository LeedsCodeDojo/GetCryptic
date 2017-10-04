module Xor

let xor_by_sequence left right =
    let paddedRight = Seq.initInfinite (fun _ -> right) |> Seq.concat

    left
    |> Seq.zip paddedRight
    |> Seq.map (fun (l, r) -> l ^^^ r)

let xor_with_every_byte bytes = 
    let keyAsSequence key = (seq { yield ((byte)key) })

    seq { yield! [0..255] }
    |> Seq.map (fun key -> ( xor_by_sequence  bytes  (keyAsSequence key) ))