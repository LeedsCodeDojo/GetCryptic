module Text

let bytes_from_text (text:seq<char>) = 
    text 
    |> Seq.map (byte)

let text_from_bytes (bytes:seq<byte>) = 
    bytes 
    |> Seq.map (char)