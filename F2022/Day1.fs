module Day1

open System
open Xunit
open Xunit.Abstractions

let text = System.IO.File.ReadAllText("../../../1.txt")

let caloriesPerElf =
    text.Split("\n\n")
    |> Seq.map (fun s -> s.Split("\n") |> Seq.map int)

type Tests(output: ITestOutputHelper) =
    [<Fact>]
    let ``Part 1`` () =
        let max =
            caloriesPerElf
            |> Seq.map (fun calories -> Seq.sum calories)
            |> Seq.max
            
        output.WriteLine(string max)

    [<Fact>]
    let ``Part 2`` () =
        let max3 =
            caloriesPerElf
            |> Seq.map (fun calories -> Seq.sum calories)
            |> Seq.sortDescending
            |> Seq.take 3
            |> Seq.sum
            
        output.WriteLine(string max3)