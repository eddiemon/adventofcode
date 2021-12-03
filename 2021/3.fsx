
let digits = System.IO.File.ReadAllLines("3.txt")

let tdigits =
    digits
    |> Seq.transpose
    |> Seq.map (fun s -> s |> Seq.map System.Char.GetNumericValue |> Seq.map int)

let mcd digits =
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.maxBy (fun x -> snd(x))
    |> fst

let lcd digits =
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.minBy (fun x -> snd(x))
    |> fst

let toDecimal digits =
    let length = Seq.length digits
    digits
    |> Seq.map float
    |> Seq.mapi (fun i x -> System.Math.Pow(2, float(length - i - 1)) * x)
    |> Seq.sum
    |> int

let gamma =
    tdigits
    |> Seq.map mcd
    |> toDecimal
    
let epsilon =
    tdigits
    |> Seq.map lcd
    |> toDecimal

printfn "Part 1: %A" (gamma * epsilon)