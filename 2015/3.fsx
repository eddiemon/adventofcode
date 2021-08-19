let input = System.IO.File.ReadAllText("3.txt")

type Dir = N | W | S | E
let parseDir = function | '^' -> N | '>' -> W | 'v' -> S | '<' -> E | c -> failwithf "Invalid %c" c

let move (x,y) dir =
    match dir with
    | N -> (x, y+1)
    | S -> (x, y-1)
    | W -> (x+1, y)
    | E -> (x-1, y)

let resultA =
    input
    |> Seq.map parseDir
    |> Seq.scan move (0,0)
    |> Seq.distinct
    |> Seq.length

printfn "%A" resultA

let resultB =
    input
    |> Seq.map parseDir
    |> Seq.mapi (fun i x -> i%2, x)
    |> Seq.groupBy fst
    |> Seq.map (fun (_, gr) -> gr |> Seq.map snd)
    |> Seq.collect (Seq.scan move (0,0))
    |> Seq.distinct
    |> Seq.length

printfn "%A" resultB