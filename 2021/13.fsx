let input = System.IO.File.ReadAllLines "13.txt"

let points =
    input
    |> Seq.takeWhile (fun l -> not (l = ""))
    |> Seq.map (fun s -> s.Split ',' |> Array.map int)
    |> Seq.map (fun a -> (a[0], a[1]))
    |> Set.ofSeq

let instructions =
    input
    |> Seq.skip (1 + Set.count points)

let foldAlongY foldY (points: Set<int * int>) =
    points
    |> Set.map (fun (x, y) ->
        match y with
        | _ when y < foldY -> (x, y)
        | _ ->
            let yy = y - (y - foldY) * 2
            (x, yy)
    )
let foldAlongX foldX (points: Set<int * int>) =
    points
    |> Set.map (fun (x, y) ->
        match x with
        | _ when x < foldX -> (x, y)
        | _ ->
            let xx = x - (x - foldX) * 2
            (xx, y)
    )

let printPoints (points: Set<int * int>) =
    let maxX = points |> Seq.map fst |> Seq.max
    let maxY = points |> Seq.map snd |> Seq.max
    for y in 0..maxY do
        for x in 0..maxX do
            match (Set.contains (x,y) points) with
            | true -> printf "#"
            | false -> printf " "
        printfn ""


let result1 = Set.count (foldAlongX 655 points)
printfn "part 1: %i" result1

printfn "part 2"
instructions
|> Seq.map (fun l ->
    let a = l.Split '='
    (a[0], int a[1])
)
|> Seq.fold (fun points (instr, amount) ->
    match instr with
    | "fold along x" -> foldAlongX amount points
    | "fold along y" -> foldAlongY amount points
) points
|> printPoints
