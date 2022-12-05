module Day5

open System
open System.Collections.Generic
open Xunit
open Xunit.Abstractions

type Tests(output: ITestOutputHelper) =
    
    let pushItemToStack (stackNo: int) (item: string) (stack: Map<int, Stack<string>>) =
        (stack.Item stackNo).Push item
        stack

    let parseStacks (s: string) =
        let lines = s.Split("\n")
                    |> Array.rev
        let noOfStacks = lines[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Length
        let stacks = seq { for i in 1 .. noOfStacks do (i, Stack<string>()) } |> Map.ofSeq
        
        lines
        |> Array.skip 1
        |> Array.fold (fun stacks s ->
            seq { for i in 0 .. noOfStacks-1 do i }
            |> Seq.fold (fun stacks index ->
                 match s[1+index*4] with
                 | ' ' -> stacks
                 | c -> pushItemToStack (index+1) (string c) stacks) stacks
        ) stacks
    
    let parseMoves (s:string) =
        s.Split("\n", StringSplitOptions.RemoveEmptyEntries)
        |> Array.map (fun l ->
            let p = l.Split(" ")
            (int p[1], int p[3], int p[5])
        )
        
    let input =
        let parts =
//             """    [D]    
// [N] [C]    
// [Z] [M] [P]
//  1   2   3 
//
// move 1 from 2 to 1
// move 3 from 1 to 3
// move 2 from 2 to 1
// move 1 from 1 to 2"""
            System.IO.File.ReadAllText("../../../5.txt")
                .Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
        (parseStacks parts[0], parseMoves parts[1])
        
    [<Fact>]
    let ``Part 1`` () =
        let (stacks, moves) = input
        
        // Apply moves
        moves
        |> Array.iter (fun (count, fromIdx, toIdx) ->
            seq {0..count-1}
            |> Seq.iter (fun _ ->
                let item = (stacks.Item fromIdx).Pop()
                (stacks.Item toIdx).Push(item)
            )
        )
        
        let topCrates =
            stacks
            |> Seq.map (fun kvp -> kvp.Value.Pop())
            |> String.concat ""
        
        output.WriteLine (topCrates)
    
    [<Fact>]
    let ``Part 2`` () =
        let (stacks, moves) = input
        
        // Apply moves
        moves
        |> Array.iter (fun (count, fromIdx, toIdx) ->
            seq {0..count-1}
            |> Seq.fold (fun list _ ->
                List.append [(stacks.Item fromIdx).Pop()] list
            ) List.empty<string>
            |> List.iter (fun item -> (stacks.Item toIdx).Push(item))
        )
        
        let topCrates =
            stacks
            |> Seq.map (fun kvp -> kvp.Value.Pop())
            |> String.concat ""
        
        output.WriteLine (topCrates)
