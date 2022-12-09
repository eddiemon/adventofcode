module Day8

open System
open Xunit
open Xunit.Abstractions

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../8.txt")
    
    let example =
        """30373
25512
65332
33549
35390"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input.Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.mapi (fun y l ->
            l
            |> Seq.mapi (fun x c ->
                ((x,y), int c - int '0')
            )
            |> Array.ofSeq
        )

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let input = parseInput inputType
        let size = Array.length input
        
        let interiorTrees =
            input
            |> Array.fold (fun (y, visibleTrees) row ->
                if y = 0 || y = size - 1 then
                    (y+1, (row |> List.ofArray) @ visibleTrees)
                else
                    let visibleTreesFromLeft =
                        row
                        |> Array.fold (fun ((x, maxHeight), visibleTrees) tree ->
                            if x = 0 || x = size - 1 then
                                ((x + 1, snd tree), tree :: visibleTrees)
                            else
                                match (snd tree) > maxHeight with
                                | true -> ((x+1, snd tree), tree :: visibleTrees)
                                | false -> ((x+1, maxHeight), visibleTrees)
                        ) ((0, 0), [])
                        |> snd
                        
                    let visibleTreesFromRight =
                        row
                        |> Array.rev
                        |> Array.fold (fun ((x, maxHeight), visibleTrees) tree ->
                            if x = 0 || x = size - 1 then
                                ((x + 1, snd tree), tree :: visibleTrees)
                            else
                                match (snd tree) > maxHeight with
                                | true -> ((x+1, snd tree), tree :: visibleTrees)
                                | false -> ((x+1, maxHeight), visibleTrees)
                        ) ((0, 0), [])
                        |> snd
                        
                    (y+1, visibleTreesFromLeft @ visibleTreesFromRight @ visibleTrees)
            ) (0, [])
            |> snd
            
        let interiorTrees' =
            input
            |> Array.transpose
            |> Array.fold (fun (y, visibleTrees) row ->
                if y = 0 || y = size - 1 then
                    (y+1, (row |> List.ofArray) @ visibleTrees)
                else
                    let visibleTreesFromLeft =
                        row
                        |> Array.fold (fun ((x, maxHeight), visibleTrees) tree ->
                            if x = 0 || x = size - 1 then
                                ((x + 1, snd tree), tree :: visibleTrees)
                            else
                                match (snd tree) > maxHeight with
                                | true -> ((x+1, snd tree), tree :: visibleTrees)
                                | false -> ((x+1, maxHeight), visibleTrees)
                        ) ((0, 0), [])
                        |> snd
                        
                    let visibleTreesFromRight =
                        row
                        |> Array.rev
                        |> Array.fold (fun ((x, maxHeight), visibleTrees) tree ->
                            if x = 0 || x = size - 1 then
                                ((x + 1, snd tree), tree :: visibleTrees)
                            else
                                match (snd tree) > maxHeight with
                                | true -> ((x+1, snd tree), tree :: visibleTrees)
                                | false -> ((x+1, maxHeight), visibleTrees)
                        ) ((0, 0), [])
                        |> snd
                        
                    (y+1, visibleTreesFromLeft @ visibleTreesFromRight @ visibleTrees)
            ) (0, [])
            |> snd
        
        let result =
            interiorTrees
            |> Set.ofList
            |> Set.union (Set.ofList interiorTrees')
            |> Set.count
        
        write result
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        let size = Array.length input
        let treeHeights =
            input
            |> Array.collect id
            |> dict
            
        let getHorizontalScenicScore height y xs =
            xs
            |> Seq.fold (fun (maxHeight, list) x ->
                let height' = treeHeights[(x, y)]
                if maxHeight < height then
                    (height', x :: list)
                else
                    (height, list)
            ) (0, [])
            |> snd
            |> Seq.length
            
        let getVerticalScenicScore height x ys =
            ys
            |> Seq.fold (fun (maxHeight, list) y ->
                let height' = treeHeights[(x, y)]
                if maxHeight < height then
                    (height', y :: list)
                else
                    (height, list)
            ) (0, [])
            |> snd
            |> Seq.length

        let highestScenicScore =
            treeHeights
            |> Seq.map (fun kvp ->
                let (x,y) = kvp.Key
                let height = kvp.Value
                let leftScore =
                    seq { x - 1 .. -1 .. 0}
                    |> getHorizontalScenicScore height y

                let rightScore =
                    seq { x + 1 .. 1 .. size - 1}
                    |> getHorizontalScenicScore height y
                    
                let upScore =
                    seq { y - 1 .. -1 .. 0}
                    |> getVerticalScenicScore height x
                    
                let downScore =
                    seq { y + 1 .. 1 .. size - 1}
                    |> getVerticalScenicScore height x
                leftScore * rightScore * upScore * downScore
            )
            |> Seq.max
        
        
        write highestScenicScore
