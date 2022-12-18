module Day18

open System
open System.Collections.Generic
open Xunit
open Xunit.Abstractions

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../18.txt")
    let example =
        """2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.map (fun l ->
            let [|x;y;z|] = l.Split(",")
            (int x, int y, int z))
        |> Set.ofArray
    
    let d = [|(-1,0,0);(1,0,0);(0,-1,0);(0,1,0);(0,0,-1);(0,0,1)|]
    let (++) (x1,y1,z1) (x2,y2,z2) = (x1+x2,y1+y2,z1+z2)

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let input = parseInput inputType
        let exposedSides =
            input
            |> Seq.fold (fun acc elem ->
                acc + (d |> Array.map ((++) elem) |> Array.filter (fun p -> input.Contains(p) = false) |> Array.length)
            ) 0
        
        write exposedSides
        
    let isOutside cubes minx maxx miny maxy minz maxz cube =
        if Set.contains cube cubes then
            false
        else
            let visited = HashSet(cubes)
            let mutable found = false
            let q = Queue<int*int*int>()
            q.Enqueue(cube)
            while found = false && q.Count > 0 do
                let c = q.Dequeue()
                if visited.Contains(c) = false then
                    visited.Add(c) |> ignore
                    let (x,y,z) = c
                    if x < minx || x > maxx || y < miny || y > maxy || z < minz || z > maxz then
                        found <- true
                    else
                        d |> Array.iter (fun d -> c ++ d |> q.Enqueue)
            found
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        let (minx, maxx) = input |> Set.map (fun (x,_,_) -> x) |> fun s -> (Set.minElement s, Set.maxElement s)
        let (miny, maxy) = input |> Set.map (fun (_,y,_) -> y) |> fun s -> (Set.minElement s, Set.maxElement s)
        let (minz, maxz) = input |> Set.map (fun (_,_,z) -> z) |> fun s -> (Set.minElement s, Set.maxElement s)
        let exposedSurfaceArea =
            input
            |> Seq.collect (fun elem -> d |> Array.map ((++) elem))
            |> Seq.filter (isOutside input minx maxx miny maxy minz maxz)
            |> Seq.length
            
        write exposedSurfaceArea
