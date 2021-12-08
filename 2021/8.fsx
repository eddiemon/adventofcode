let input = System.IO.File.ReadAllLines "8.txt"

let mutable digits: string array list = []
let mutable outputs: string array list = []

input
|> Seq.iter (fun s -> 
    let split = s.Split('|', System.StringSplitOptions.RemoveEmptyEntries ||| System.StringSplitOptions.TrimEntries)
    // printfn "%i" (Seq.length split)
    digits <- digits @ [split[0].Split ' ']
    outputs <- outputs @ [split[1].Split ' ']
)

let result1 =
    outputs
    |> Seq.collect (fun array ->
        array |> Seq.map Seq.length
    )
    |> Seq.filter (
        function 
        | 2 | 4 | 3 | 7 -> true
        | _ -> false
    )
    |> Seq.length

printfn "Part 1: %i" result1