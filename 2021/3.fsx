
let digits = Seq.ofArray (System.IO.File.ReadAllLines("3.txt")) |> Seq.map (fun s -> s.ToCharArray() |> Seq.map System.Char.GetNumericValue |> Seq.map int)

let tdigits =
    digits
    |> Seq.transpose

let mcd digits = // most common digit
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.sortByDescending fst
    |> Seq.maxBy snd
    |> fst

let lcd digits = // least common digit
    digits
    |> Seq.countBy (fun x -> x)
    |> Seq.sortBy fst
    |> Seq.minBy snd
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

let bitPredicate index (filterBit:int) seq = Seq.filter (fun b -> Seq.item index b = filterBit) seq

let rating filterFunc =
    [0..Seq.length tdigits]
    |>  Seq.fold (fun (acc, index) _ ->
        let filterBit = acc |> Seq.transpose |> Seq.item index |> filterFunc
        let filteredDigits = bitPredicate index filterBit acc
        (filteredDigits, index + 1)
    ) (digits, 0)
    |> fst
    |> Seq.head
    |> toDecimal


let oxygenRating = rating mcd
let co2scrubRating = rating lcd

co2scrubRating * oxygenRating