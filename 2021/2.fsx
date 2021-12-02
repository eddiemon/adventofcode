
type Direction = Forward | Up | Down
type Command = { Direction: Direction; Magnitude: int }

let split (delimiter:char) (theString:string) = theString.Split(delimiter)

let commands =
    System.IO.File.ReadAllLines("2.txt")
    |> Seq.map (split ' ')
    |> Seq.map (fun s -> match s[0] with
                            | "forward" -> { Direction = Forward; Magnitude = int(s[1])}
                            | "up" -> { Direction = Up; Magnitude = int(s[1])}
                            | "down" -> { Direction = Down; Magnitude = int(s[1])}
    )

let depths =
    ([|0;0|], commands)
    ||> Seq.fold (fun acc x -> match x.Direction with
                                | Forward -> [|acc[0] + x.Magnitude; acc.[1]|]
                                | Up ->      [|acc[0]; acc.[1] - x.Magnitude|]
                                | Down ->    [|acc[0]; acc.[1] + x.Magnitude|]
    )

printfn "Part 1: %A" (depths[0]*depths[1])

