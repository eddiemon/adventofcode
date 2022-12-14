module Day14

open System
open Xunit
open Xunit.Abstractions


type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../14.txt")
    let example =
        """498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.collect (fun l ->
            let coords = l.Split(" -> ") |> Array.map (fun l -> l.Split(",") |> Array.map int |> fun [|a;b|] -> (a,b))
            coords
            |> Array.pairwise
            |> Array.fold (fun list ((x1,y1),(x2,y2)) ->
                let xmin = min x1 x2
                let xmax = max x1 x2
                let ymin = min y1 y2
                let ymax = max y1 y2
                match xmin = xmax with
                | true -> [for y in ymin .. ymax do (x1, y)] @ list
                | false -> [for x in xmin .. xmax do (x, y1)] @ list
            ) []
            |> Array.ofList
        ) |> List.ofArray |> List.distinct

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let input = parseInput inputType
        let mutable grid = input
        let maxY =
            input |> List.maxBy (fun (x,y) -> y) |> snd

        let mutable current = (500,0)
        while snd current < maxY do
            let (currX, currY) = current
            if not (List.contains (currX,currY+1) grid) then
                current <- (currX, currY+1)
            elif not (List.contains (currX-1,currY+1) grid) then
                current <- (currX-1, currY+1)
            elif not (List.contains (currX+1,currY+1) grid) then
                current <- (currX+1, currY+1)
            else
                grid <- current :: grid
                current <- (500,0)
        let sands = List.except input grid |> List.length
        
        write sands
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        let grid = System.Collections.Generic.HashSet(input)
        let maxY =
            input |> List.maxBy (fun (x,y) -> y) |> snd |> (+) 1

        let mutable current = (500,0)
        while not (grid.Contains(500,0)) do
            let (currX, currY) = current
            if (currY = maxY) then
                grid.Add(current) |> ignore
                current <- (500,0)
            elif not (grid.Contains(currX,currY+1) ) then
                current <- (currX, currY+1)
            elif not (grid.Contains(currX-1,currY+1) ) then
                current <- (currX-1, currY+1)
            elif not (grid.Contains(currX+1,currY+1) ) then
                current <- (currX+1, currY+1)
            else
                grid.Add(current) |> ignore
                current <- (500,0)
        let sands = System.Linq.Enumerable.Except(grid, input) |> Seq.length
        
        write sands
