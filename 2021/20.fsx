let input = System.IO.File.ReadAllLines "20.txt"

let charToInt =
    function
    | '#' -> 1
    | '.' -> 0

let window9x9 (x,y) =
    Seq.zip [-1; 0; 1; -1; 0; 1; -1; 0; 1] [-1; -1; -1; 0; 0; 0; 1; 1; 1]
    |> Seq.map (fun (dx, dy) -> (dx + x, dy + y))

let printImage image =
    let intToChar =
        function
        | 1 -> '#'
        | 0 -> '.'

    let keys = Map.keys image
    let minX = (Seq.minBy (fun (x,y) -> x) keys) |> fst
    let maxX = Seq.maxBy (fun (x,y) -> x) keys |> fst
    let minY = Seq.minBy (fun (x,y) -> y) keys |> snd
    let maxY = Seq.maxBy (fun (x,y) -> y) keys |> snd
    for y in minY..maxY do
        for x in minX..maxX do
            match Map.tryFind (x,y) image with
            | Some v -> printf "%c" (intToChar v)
            | None -> printf "."
        printfn ""

let getPixelValueOrZero image pixel =
    match Map.tryFind pixel image with
    | Some p -> p
    | None -> 0

let getDecimal binaryValues =
    // printfn "%A" binaryValues
    let result =
        Seq.foldBack (fun b (acc, multiplier) ->
            (acc + (b*multiplier), multiplier * 2)
        ) binaryValues (0, 1)
    fst result

let enhance algorithm image =
    let toVisit =
        image
        |> Map.keys
        |> Seq.fold (fun toVisit n ->
            let nn = n |> window9x9 //Seq.fold (fun s nn -> Set.add nn s) toVisit
            Seq.fold (fun toVisit nnn -> Set.add nnn toVisit) toVisit nn
        ) Set.empty
        |> List.ofSeq

    Seq.fold (fun image (x,y) ->
        let pixels = window9x9 (x,y)
        // printfn "%A" image
        let algorithmIndex =
            pixels
            |> Seq.map (getPixelValueOrZero image)
            |> getDecimal
        let newValue = Array.get algorithm algorithmIndex
        printfn "Found value <%i> in algorithm index <%i> for point (%i, %i)" newValue algorithmIndex x y
        Map.add (x,y) newValue image
    ) image toVisit

let algorithm = input |> Seq.item 0 |> Seq.map charToInt |> Array.ofSeq

let image =
    let img = input |> Seq.skip 2 |> Seq.map (fun l -> l.ToCharArray() |> Array.map charToInt) |> Array.ofSeq
    let size = Seq.length img
    [for y in 0..size-1 do
        for x in 0..size-1 -> ((x,y), img[y][x])]
    |> Map.ofList

let iimage = enhance algorithm image

printImage image
printImage iimage
// window9x9 (2,2) |> Seq.map (getPixelValueOrZero image) |> getDecimal