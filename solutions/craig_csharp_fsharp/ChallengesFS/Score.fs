module Score

let score_using_english (message:seq<char>) = 
    message 
    |> Seq.sumBy 
        (fun letter -> 
            match letter with
            | t when t >= 'A' && t <= 'Z' -> 1
            | t when t >= 'a' && t <= 'z' -> 1
            | ' ' -> 1
            | _ -> 0)
    |> (fun score -> (score, message))