let input = System.IO.File.ReadAllLines "9.txt"

let heightmap =
    input
    |> Seq.map (fun s -> s |> Seq.map (fun c -> System.Char.GetNumericValue c |> int) |> Array.ofSeq)
    |> Array.ofSeq

let neighbouringPositions (x,y) =
    [(0, -1); (0, 1); (-1, 0); (1, 0)]
    |> Seq.map (fun (dx, dy) ->
        (x+dx, y+dy)
    )
    |> Seq.filter (
        function
        | (x, y) when y < 0 || x < 0 || y >= Seq.length heightmap || x >= Seq.length heightmap[y] -> false
        | _ -> true
    )

let lowest (x, y) =
    let height = heightmap[y][x]
    neighbouringPositions (x, y)
    |> Seq.map (fun (xx, yy) ->
        heightmap[yy][xx]
    )
    |> Seq.exists (fun adjecentHeight -> adjecentHeight <= height)
    |> not

let lowestHeights =
    let xy = Seq.allPairs [0..Seq.length heightmap - 1] [0..Seq.length heightmap[0] - 1]
    
    Seq.fold (fun list (y, x) ->
        if lowest (x, y) then
            list@[((heightmap[y][x]), (x, y))]
        else
            list
    ) [] xy

let result1 =
    lowestHeights
    |> List.map (fun x -> fst x + 1)
    |> List.sum

printfn "%A" result1

let dequeue (l:byref<_>) =
    let r = List.head l
    l <- List.skip 1 l
    r

let enqueue (l:byref<_>) value =
    l <- l@[value]

let getBasinPositions (x, y) =
    let mutable discovered = []
    let mutable S = []
    S <- S@[(x,y)]
    while not (List.isEmpty S) do
        let (x, y) = dequeue &S
        if not (Seq.contains (x, y) discovered) then
            enqueue &discovered (x, y)
            let neighbours = neighbouringPositions (x, y)
            let asd = Seq.filter (fun (xx,yy) -> heightmap[yy][xx] < 9) neighbours
            S <- S@(List.ofSeq asd)
    discovered


let getBasinSize (x, y) : int =
    getBasinPositions (x,y)
    |> List.length

let largestBasins =
    lowestHeights
    |> Seq.map snd
    |> Seq.map getBasinSize
    |> Seq.sortDescending
    |> Seq.take 3

let result2 =
    largestBasins
    |> Seq.fold (fun acc b -> acc * b) 1

printfn "%A" result2