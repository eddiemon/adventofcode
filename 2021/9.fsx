let input = System.IO.File.ReadAllLines "9.txt"

let heightmap =
    input
    |> Seq.map (fun s -> s |> Seq.map (fun c -> System.Char.GetNumericValue c |> int) |> Array.ofSeq)
    |> Array.ofSeq


let lowest y x =
    let height = heightmap[y][x]
    Seq.zip [-1;1;0;0] [0;0;-1;1]
    |> Seq.map (fun (dy, dx) ->
        // printfn "(%i, %i)" (x+dx) (y+dy)
        (y+dy, x+dx)
    )
    |> Seq.filter (
        function
        | (y, x) when y < 0 || x < 0 || y >= Seq.length heightmap || x >= Seq.length heightmap[0] -> false
        | _ -> true
    )
    |> Seq.map (fun (yy, xx) -> 
        // printfn "%i (%i,%i) is adjecent to (%i,%i)" (heightmap[yy][xx]) xx yy x y
        heightmap[yy][xx]
    )
    |> Seq.exists (fun adjecentHeight -> adjecentHeight <= height)
    |> not

let lowestHeights =
    let xy = Seq.allPairs [0..Seq.length heightmap - 1] [0..Seq.length heightmap[0] - 1]
    
    Seq.fold (fun list (y, x) ->
        if lowest y x then
            // printfn "%i (%i,%i) is lower than adjecents" (heightmap[y][x]) x y
            list@[((heightmap[y][x]), (x, y))]
        else
            list
    ) [] xy

let result1 =
    lowestHeights
    |> List.map (fun x -> fst x + 1)
    |> List.sum

let getBasinSize (x, y) : int =
    0

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