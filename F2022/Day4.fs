module Day4

open Xunit
open Xunit.Abstractions

type Range =
    { Lo: int
      Hi: int }
    member this.length = this.Hi - this.Lo

type Tests(output: ITestOutputHelper) =
    let input =
        System.IO.File.ReadAllLines("../../../4.txt")
        |> Array.filter (fun s -> System.String.IsNullOrWhiteSpace(s) = false)
        |> Array.map (fun s ->
            s.Split(',')
            |> Array.map (fun s' ->
                s'.Split('-')
                |> Array.map int
                |> fun a -> { Lo = a[0]; Hi = a[1] }
            )
            |> Array.sortByDescending (fun r -> r.length)
        )
        
    [<Fact>]
    let ``Part 1`` () =
        let pairIsFullyContained (ranges: Range[]) =
            if ranges[0].Lo > ranges[1].Lo then false
            elif ranges[0].Hi < ranges[1].Hi then false
            else true

        let fullyContainerPairs =
            input
            |> Seq.filter pairIsFullyContained
            |> Seq.length
        
        output.WriteLine (string fullyContainerPairs)
    
    [<Fact>]
    let ``Part 2`` () =
        let pairOverlaps (ranges: Range[]) =
            if ranges[1].Hi < ranges[0].Lo then false
            elif ranges[0].Hi < ranges[1].Lo then false
            else true

        let overlappingPairs =
            input
            |> Seq.filter pairOverlaps
            |> Seq.length
        
        output.WriteLine (string overlappingPairs)
