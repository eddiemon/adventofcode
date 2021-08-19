
let chars = System.IO.File.ReadAllText("1.in")

let charToDirection c =
    match c with
    | '(' -> 1
    | ')' -> -1
    | _ -> failwithf "invalid char %c" c

let floor =
    chars
    |> Seq.map charToDirection
    |> Seq.scan (+) 0
    |> Seq.findIndex (fun l -> l < 0)

printfn "%d" floor