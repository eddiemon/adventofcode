open System.Security.Cryptography
open System.Text

let md5 = MD5.Create()
    
let solve (input : byte array) filter =
    Seq.unfold (fun x -> Some (x+1, x+1)) 0
    |> Seq.map (fun n ->
        n, (md5.ComputeHash (Array.append input (string n |>  System.Text.Encoding.UTF8.GetBytes))))
    |> Seq.filter filter
    |> Seq.head
    |> fst

printfn "%A" (solve "ckczppom"B (fun (_, b) ->
        b.[0] = 0uy && b.[1] = 0uy && b.[2] < 16uy))
        
printfn "%A" (solve "ckczppom"B (fun (_, b) ->
        b.[0] = 0uy && b.[1] = 0uy && b.[2] = 0uy))