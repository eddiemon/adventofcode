module Day5

open Xunit
open Xunit.Abstractions

type Tests(output: ITestOutputHelper) =
    let input =
        // """dabAcCaCBAcCcaDA"""
        System.IO.File.ReadAllText("../../../5.txt")
        |> List.ofSeq
    
    let reduceOne input =
        input
        |> List.fold (fun (result, previous) current ->
            match abs((int current) - (int previous)) with
            | 32 -> (List.tail result, ' ')
            | _ -> ((List.append [current] result), current)
        ) (List.empty<char>, ' ')
        |> fst
        |> List.rev
        
    let reduceMax input =
        input
        |> List.unfold (fun s ->
            let reduced = s |> reduceOne
            match (List.length s = List.length reduced) with
            | false -> Some (reduced, reduced)
            | true -> None
        )
        |> List.last

    [<Fact>]
    let ``Part 1`` () =
        let reduced = reduceMax input
        output.WriteLine (string (List.length reduced))
    
    [<Fact>]
    let ``Part 2`` () =
        let shortest =
            seq { for c in 'a'..'z' do [|c; char ((int c) - 32)|] }
            |> List.ofSeq
            |> List.map (fun removeChars -> List.filter (fun c -> Array.contains c removeChars |> not) input)   
            |> Seq.map reduceMax
            |> Seq.map Seq.length
            |> Seq.min
        
        output.WriteLine (string shortest)
