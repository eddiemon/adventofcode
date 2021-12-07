let input = System.IO.File.ReadAllText("7.txt")

let positions = input.Split(',') |> Seq.map int

let max = Seq.max positions
let min = Seq.min positions

let allFuels =
    [min..max]
    |> Seq.map (fun i ->
        let totalFuel =
            positions
            |> Seq.fold (fun acc v ->
                let steps = abs(v - i)
                let fuel = (steps * (steps + 1)) / 2
                acc + fuel
            ) 0
        (i, totalFuel)
    )

let minFuel =
    allFuels 
    |> Seq.minBy (fun (_, fuel) -> fuel)
    |> snd

printfn "%A" minFuel