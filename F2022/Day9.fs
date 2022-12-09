module Day9

open System
open Xunit
open Xunit.Abstractions

type Direction = Right | Left | Up | Down

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../9.txt")
    let example =
        """R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2"""

    let example2 =
        """R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "example2" -> example2
            | "input" -> personalInput
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.collect (fun line ->
            let s = line.Split(" ")
            let times = int s[1]
            match s[0] with
            | "R" -> Array.replicate times Right
            | "L" -> Array.replicate times Left
            | "U" -> Array.replicate times Up
            | "D" -> Array.replicate times Down
            | _ -> failwith ""
        )
        
    let applyDirection dir (point: int*int) =
        match dir with
        | Right -> (fst point + 1, snd point)
        | Left -> (fst point - 1, snd point)
        | Up -> (fst point, snd point - 1)
        | Down -> (fst point, snd point + 1)
    
    let applyCompensation (head:int*int) (tail: int*int) =
        let dx = (fst head) - (fst tail)
        let dy = (snd head) - (snd tail)
        if abs dx <= 1 && abs dy <= 1 then
            tail
        elif abs dx = 0 then
            let ndy = dy / (abs dy)
            (fst tail, snd tail + ndy)
        elif abs dy = 0 then
            let ndx = dx / (abs dx)
            (fst tail + ndx, snd tail)
        else
            let ndx = dx / (abs dx)
            let ndy = dy / (abs dy)
            (fst tail + ndx, snd tail + ndy)

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let input = parseInput inputType
        let mutable head = (0,0)
        let mutable tail = head
        let mutable visitedPoints = Set.singleton tail
        for direction in input do
            head <- applyDirection direction head
            tail <- applyCompensation head tail
            visitedPoints <- Set.add tail visitedPoints

        write visitedPoints.Count
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("example2")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        let mutable head = (0,0)
        let mutable tails = List.replicate 9 head
        let mutable visitedPoints = Set.singleton head
        for direction in input do
            head <- applyDirection direction head
            tails <-
                tails
                |> List.fold (fun (previousKnot, list) knot ->
                    let newKnot = applyCompensation previousKnot knot
                    (newKnot, newKnot :: list)
                ) (head, [])
                |> snd
                |> List.rev
            let tail = List.last tails
            visitedPoints <- Set.add tail visitedPoints

        write visitedPoints.Count
