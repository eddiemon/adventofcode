let dims = System.IO.File.ReadAllLines("2.txt")

let dimsToTuple (s: string) = s.Split 'x' |> fun x -> (int x.[0], int x.[1], int x.[2])
let surfaceArea (l, w, h) = 2 * l * w + 2 * w * h + 2 * h * l
let smallestArea (l, w, h) = Seq.min [l * w; l * h; w * h]
let cubicFeet (l, w, h) = l * w * h
let shortestSides (l, w, h) =
    let dims = [|l; w; h|]
    2 * (Seq.sum dims - Seq.max dims)

let totalFeetPaper = 
    dims
    |> Seq.sumBy (dimsToTuple >> (fun x -> surfaceArea x + smallestArea x))
printfn "%d" totalFeetPaper

let totalFeetRibbon =
    dims
    |> Seq.sumBy (dimsToTuple >> (fun x -> shortestSides x + cubicFeet x))
printfn "%d" totalFeetRibbon