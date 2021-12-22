let input = System.IO.File.ReadAllLines "20.txt"

type State = {
    Image : Map<int*int, int>
    Algorithm: int array
    InfinitePixelValue: int
}

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
    let minX = Seq.minBy (fun (x,y) -> x) keys |> fst
    let maxX = Seq.maxBy (fun (x,y) -> x) keys |> fst
    let minY = Seq.minBy (fun (x,y) -> y) keys |> snd
    let maxY = Seq.maxBy (fun (x,y) -> y) keys |> snd
    for y in minY..maxY do
        for x in minX..maxX do
            match Map.tryFind (x,y) image with
            | Some v -> printf "%c" (intToChar v)
            | None -> printf "."
        printfn ""

let getPixelValueOrZero image pixelDefault pixel =
    match Map.tryFind pixel image with
    | Some p -> p
    | None -> pixelDefault

let getDecimal binaryValues =
    let result =
        Seq.foldBack (fun b (acc, multiplier) ->
            (acc + (b*multiplier), multiplier * 2)
        ) binaryValues (0, 1)
    fst result

let nextInfinitePixelValue = function | 1 -> 0 | 0 -> 1

let enhance state: State =
    let toVisit =
        state.Image
        |> Map.keys
        |> Seq.fold (fun toVisit n ->
            let nn = n |> window9x9
            Seq.fold (fun toVisit nnn -> Set.add nnn toVisit) toVisit nn
        ) Set.empty
        |> List.ofSeq

    let newImage =
        Seq.fold (fun updatedImage (x,y) ->
            let pixels = window9x9 (x,y)
            let algorithmIndex =
                pixels
                |> Seq.map (getPixelValueOrZero state.Image state.InfinitePixelValue)
                |> getDecimal
            let newValue = Array.get state.Algorithm algorithmIndex
            Map.add (x,y) newValue updatedImage
        ) Map.empty toVisit
    { state with
        Image = newImage
        InfinitePixelValue = nextInfinitePixelValue state.InfinitePixelValue
    }


let enhanceNTimes state times =
    Seq.fold (fun s _ -> enhance s) state [1..times]

let algorithm = input |> Seq.item 0 |> Seq.map charToInt |> Array.ofSeq

let image =
    let img = input |> Seq.skip 2 |> Seq.map (fun l -> l.ToCharArray() |> Array.map charToInt) |> Array.ofSeq
    let size = Seq.length img
    [for y in 0..size-1 do
        for x in 0..size-1 -> ((x,y), img[y][x])]
    |> Map.ofList

let state = {
    Image = image
    Algorithm = algorithm
    InfinitePixelValue = 0
}

let result = enhanceNTimes state 50

result.Image.Values |> Seq.filter (fun v -> v = 1) |> Seq.length