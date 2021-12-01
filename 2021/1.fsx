
let depths =
    System.IO.File.ReadAllLines("1.in")
    |> Seq.map int

let noOfIncrements =
    (0, depths, depths |> Seq.tail)
    |||> Seq.fold2 (fun acc x1 x2 -> if x2 > x1 then acc + 1 else acc)

printfn "%A" noOfIncrements