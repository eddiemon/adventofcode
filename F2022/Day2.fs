module Day2

open System
open Xunit
open Xunit.Abstractions

type Play = Rock | Paper | Scissor
    
type Round = {
    OpponentPlay: Play
    YourPlay: Play
}

let LetterToPlay =
    function
    | "A" | "X" -> Rock
    | "B" | "Y" -> Paper
    | "C" | "Z" -> Scissor
    | _ -> failwith "unexpected"

let LineToRound (l: string) =
    let a = l.Split(" ")
    { OpponentPlay = (LetterToPlay a[0]); YourPlay = (LetterToPlay a[1]) }
    
let WinPolicy yourPlay opponentPlay =
    match yourPlay with
    | Rock when opponentPlay = Scissor -> 6
    | Paper when opponentPlay = Rock -> 6
    | Scissor when opponentPlay = Paper -> 6
    | _ -> 0

let DrawPolicy yourPlay opponentPlay =
    match yourPlay = opponentPlay with
    | true -> 3
    | _ -> 0

let PlayPolicy yourPlay _ =
    match yourPlay with
    | Rock -> 1
    | Paper -> 2
    | Scissor -> 3
    
let WinningShape =
    function
    | Rock -> Paper
    | Paper -> Scissor
    | Scissor -> Rock

let DrawingShape =
    function
    | Rock -> Rock
    | Paper -> Paper
    | Scissor -> Scissor
    
let LosingShape =
    function
    | Rock -> Scissor
    | Paper -> Rock
    | Scissor -> Paper

type Tests(output: ITestOutputHelper) =
    let rounds =
        // """A Y
        // B X
        // C Z""".Split "\n"
        // |> Seq.map (fun l -> l.Trim())
        System.IO.File.ReadAllLines("../../../2.txt")
        |> Seq.filter (fun l -> String.IsNullOrWhiteSpace(l) = false)
        |> Seq.map LineToRound
        
    let ScorePolicys = [WinPolicy;DrawPolicy;PlayPolicy]
    
    [<Fact>]
    let ``Part 1`` () =
        let RoundToScore r =
            ScorePolicys
            |> Seq.fold (fun sum policy -> sum + (policy r.YourPlay r.OpponentPlay)) 0
            
        let score =
            rounds
            |> Seq.sumBy (fun r -> RoundToScore r)
        
        output.WriteLine (string score)
    
    [<Fact>]
    let ``Part 2`` () =
        let ShapeToPlay r =
            match r.YourPlay with
            | Rock -> LosingShape r.OpponentPlay
            | Paper -> DrawingShape r.OpponentPlay
            | Scissor -> WinningShape r.OpponentPlay

        let RoundToScore r =
            ScorePolicys
            |> Seq.fold (fun sum policy -> sum + (policy (ShapeToPlay r) r.OpponentPlay)) 0
            
        let score =
            rounds
            |> Seq.sumBy (fun r -> RoundToScore r)
        
        output.WriteLine (string score)