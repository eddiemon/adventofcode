let input = System.IO.File.ReadAllLines "11.txt"

let charToInt c = int ( System.Char.GetNumericValue c)
let stringToIntArray s =
    s |> Seq.map charToInt |> Array.ofSeq

type World =
  { EnergyLevels: int array
    MaxX : int
    MaxY : int
    mutable NoOfFlashes: int}

    member this.getCoordinates =
        seq {
            for y in 0..this.MaxY do
                for x in 0..this.MaxX do
                    yield (x,y)
        }
        
    member this.get x y =
        this.EnergyLevels[y * (this.MaxX + 1) + x]

    member this.inc x y =
        this.EnergyLevels[y * (this.MaxX + 1) + x] <- this.EnergyLevels[y * (this.MaxX + 1) + x] + 1
        
    member this.set x y value =
        this.EnergyLevels[y * (this.MaxX + 1) + x] <- value

let initialWorld = fun () ->
    let maxX = -1 + input[0].Length
    let maxY = -1 + Array.length input
    let result =
        input
        |> Seq.map stringToIntArray
        |> Seq.collect (fun x -> x)
        |> Array.ofSeq
    { EnergyLevels = result; MaxX = maxX; MaxY = maxY; NoOfFlashes = 0 }

let neighbouringPositions maxX maxY (x,y) =
    [(0, -1); (0, 1); (-1, 0); (1, 0); (-1, -1);(1, -1);(-1, 1); (1, 1)]
    |> Seq.map (fun (dx, dy) -> (x+dx, y+dy))
    |> Seq.filter (
        function
        | (x, y) when y < 0 || x < 0 || y > maxY || x > maxX -> false
        | _ -> true
    )

let printWorld world =
    for y in 0 .. world.MaxY do
        for x in 0 .. world.MaxX do
            printf "%i " world.EnergyLevels[y * (world.MaxX+1) + x]
        printfn ""

let flash (world: World) =
    let rec loop (acc:int) (flashed: (int * int) list) (world: World) =
        let shouldFlash =
            world.getCoordinates
            |> Seq.filter (fun (x,y) -> world.get x y >= 10)
            |> Seq.filter (fun (x,y) -> not (Seq.contains (x,y) flashed))
            |> List.ofSeq
        // printfn "Found %i to flash" (Seq.length shouldFlash)
        match Seq.length shouldFlash with
        | 0 -> acc
        | n ->
            let neighbours =
                shouldFlash
                |> Seq.collect (fun x -> neighbouringPositions world.MaxX world.MaxY x)
                // |> Seq.filter (fun (x,y) -> not (Seq.contains (x,y) flashed))
                // |> Seq.distinct
            // printfn "Incrementing %i neighbours" (Seq.length neighbours)
            Seq.iter (fun (x,y) -> world.inc x y) shouldFlash
            Seq.iter (fun (x,y) -> world.inc x y) neighbours

            // printWorld world

            loop (acc + n) (flashed@shouldFlash) world
    loop 0 [] world

let resetFlashed (world: World) =
    world.getCoordinates
    |> Seq.filter (fun (x,y) -> world.get x y >= 10)
    |> Seq.iter (fun (x,y) -> world.set x y 0)

let step (world: World) : World =
    // printfn "Stepping world"
    // printWorld world
    for i in 0..Array.length (world.EnergyLevels) - 1 do
        world.EnergyLevels[i] <- world.EnergyLevels[i] + 1
    // printfn "World after incrementing"
    // printWorld world
    world.NoOfFlashes <- world.NoOfFlashes + flash world
    // printfn "Flashed %i" noOfFlashes
    resetFlashed world
    world

let world = initialWorld()
for i in 1..100 do
    step world
    |> ignore

printWorld world
printfn "flashes %i" world.NoOfFlashes