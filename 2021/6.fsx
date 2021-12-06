open System.IO

let input = File.ReadAllText("6.txt")

let mutable fishes = Array.create 9 0L

input.Split(',')
|> Seq.map int
|> Seq.iter (fun i ->
    fishes[i] <- fishes[i] + 1L
)

[1..256]
|> Seq.iter (fun _ ->
    fishes <-
        (Array.create 9 0L, Array.mapi (fun i f -> (i, f)) fishes)
        ||> Array.fold (fun acc value ->
            match fst value with
            | 0 -> acc[6] <- acc[6] + snd value; acc[8] <- snd value
            | 7 -> acc[6] <- acc[6] + snd value
            | i -> acc[i-1] <- snd value
            acc
        )
)

Seq.sum fishes