open System.Collections.Generic

let input = System.IO.File.ReadAllLines "15.txt"

let cost =
    input
    |> Seq.mapi (fun y i ->
        i
        |> Seq.mapi (fun x risk ->
            ((x, y), int (System.Char.GetNumericValue risk))
        )
    )
    |> Seq.collect (fun x -> x)
    |> Map.ofSeq

let coordinates = cost.Keys

let q = new HashSet<int*int>(coordinates)
let dist =
    System.Linq.Enumerable.ToDictionary(coordinates, (fun x -> x), (fun _ -> System.Int32.MaxValue))
let prev : Dictionary<int*int, (int*int) option> =
    System.Linq.Enumerable.ToDictionary(coordinates, (fun x -> x), (fun _ -> None))

let start = (0,0)
let target = (99,99)

dist[start] <- 0

let allNeighbours (c: int*int) =
    Seq.zip [-1; 1; 0; 0] [0; 0; -1; 1]
    |> Seq.map (fun (y, x) -> (x + (fst c), y + (snd c)))
    |> Seq.filter (fun (x, y) -> x >= 0 && x <= fst target && y >= 0 && y <= snd target)

while q.Count > 0 do
    let u =
        q
        |> Seq.minBy (fun u -> dist[u])
    q.Remove(u) |> ignore

    // printfn "%A has min dist (%i)" u dist[u]
    System.Console.ReadLine |> ignore

    for v in (allNeighbours u) do
        let alt = dist[u] + cost[v]
        match alt < dist[v] with
        | true ->
            dist[v] <- alt
            prev[v] <- Some u
        | false -> ignore 0

printfn "%i" dist[target]