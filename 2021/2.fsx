
type Direction = Forward | Up | Down

let split (delimiter:char) (theString:string) = theString.Split(delimiter)

let commands =
    System.IO.File.ReadAllLines("2.txt")
    |> Seq.map (split ' ')
    |> Seq.map (fun s -> match s[0] with
                            | "forward" -> (Forward, int(s[1]))
                            | "up" -> (Up, int(s[1]))
                            | "down" -> (Down, int(s[1]))
    )

let (h,d) =
    ((0,0), commands)
    ||> Seq.fold (fun (h,d) (dir,magn) -> match dir with
                                    | Forward -> (h + magn, d)
                                    | Up ->      (h       , d - magn)
                                    | Down ->    (h       , d + magn)
    )

printfn "Part 1: %A" (h*d)

let (h2, d2, a) =
    ((0,0,0), commands)
    ||> Seq.fold (fun (h,d,a) (dir,magn) -> match dir with
                                    | Forward -> (h + magn, d + a * magn, a)
                                    | Up ->      (h       , d           , a - magn)
                                    | Down ->    (h       , d           , a + magn)
    )

printfn "Part 2: %A" (h2*d2)

