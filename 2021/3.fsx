
let digits = System.IO.File.ReadAllLines("3.txt")

let tdigits =
    digits
    |> Seq.transpose
    |> Seq.map (fun s -> s |> Seq.map System.Char.GetNumericValue |> Seq.map int)

let mcd digits = // most common digit
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.maxBy (fun x -> snd(x))
    |> fst

let lcd digits = // least common digit
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.minBy (fun x -> snd(x))
    |> fst

let toDecimal digits =
    let length = Seq.length digits
    digits
    |> Seq.mapi (fun i x -> (pown 2 (length-i-1)) * x)
    |> Seq.sum

let gamma =
    tdigits
    |> Seq.map mcd
    |> toDecimal
    
let epsilon =
    tdigits
    |> Seq.map lcd
    |> toDecimal

printfn "Part 1: %A" (gamma * epsilon)