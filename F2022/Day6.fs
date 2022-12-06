module Day6

open System
open Xunit
open Xunit.Abstractions

type Tests(output: ITestOutputHelper) =
    let input =
        // "mjqjpqmgbljsphdztnvjfqwrcgsmlb"
        System.IO.File.ReadAllText("../../../6.txt")
    
    let startIdx count input =
        input
        |> Seq.windowed count
        |> Seq.takeWhile (fun w -> (Set.ofSeq w).Count <> count)
        |> Seq.length
        |> (+) count
        
    [<Fact>]
    let ``Part 1`` () =
        let startMarkerIdx =
            input
            |> startIdx 4
        output.WriteLine (string startMarkerIdx)
    
    [<Fact>]
    let ``Part 2`` () =
        let startMarkerIdx =
            input
            |> startIdx 14
        output.WriteLine (string startMarkerIdx)
