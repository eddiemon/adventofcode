module Day3

open Xunit
open Xunit.Abstractions
    
type Rucksack = {
    LeftCompartment: seq<char>
    RightCompartment: seq<char>
}

type Tests(output: ITestOutputHelper) =
    let input =
        System.IO.File.ReadAllLines("../../../3.txt")
//         """vJrwpWtwJgWrhcsFMMfFFhFp
// jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
// PmmdzqPrVvPwwTWBwg
// wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
// ttgJtRGJQctTZtZT
// CrZsJsPPZsGzwwsLwLmpwMDw""".Split("\n") |> Seq.map (fun l -> l.Trim())
        |> Seq.map (fun l ->
            let left = l.Substring(0, l.Length / 2)
            let right = l.Substring(l.Length / 2)
            { LeftCompartment = left; RightCompartment = right }
        )
        
    let getPriority =
        seq {
            for c in 'a' .. 'z' do
                (c, (int c) - (int 'a') + 1)
            for c in 'A' .. 'Z' do
                (c, (int c) - (int 'A') + 27)
        } |> dict
        
    [<Fact>]
    let ``Part 1`` () =
        let score =
            input
            |> Seq.map (fun rucksack -> Set.intersect (Set.ofSeq rucksack.LeftCompartment) (Set.ofSeq rucksack.RightCompartment) |> Seq.head)
            |> Seq.sumBy (fun c -> getPriority[c])

        output.WriteLine (string score)
    
    [<Fact>]
    let ``Part 2`` () =
        let score =
            input
            |> Seq.splitInto (Seq.length input / 3)
            |> Seq.map (fun g ->
                g
                |> Seq.map (fun r -> Set.union (Set.ofSeq r.LeftCompartment) (Set.ofSeq r.RightCompartment))
                |> Set.intersectMany
                |> Seq.head
            )
            |> Seq.sumBy (fun c -> getPriority[c])
            
        output.WriteLine (string score)
