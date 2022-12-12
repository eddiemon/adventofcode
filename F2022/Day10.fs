module Day10

open System
open System.Text
open Xunit
open Xunit.Abstractions

type Instruction =
| Add of int
| Noop

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../10.txt")
    let example =
        """addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop
"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.map (fun line ->
            let split = line.Split(" ")
            match split[0] with
            | "noop" -> Noop
            | "addx" -> Add (int split[1])
        )
        |> Array.fold (fun (x, cycles) instr ->
            match instr with
            | Noop -> (x, x::cycles)
            | Add dx -> (x + dx, [x + dx; x] @ cycles)
        ) (1, [])
        |> snd
        |> List.rev

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let cycles = parseInput inputType
        let signalStrength =
            [20; 60; 100; 140; 180; 220]
            |> List.map (fun iter -> iter * cycles.Item (iter - 2))
            |> List.sum
        
        write signalStrength
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let cycles = parseInput inputType
        let sb = StringBuilder()
        1::cycles
        |> List.fold (fun h i ->
            let x = h % 40
            if List.contains x [i-1;i;i+1] then
                sb.Append("#") |> ignore
            else
                sb.Append(" ") |> ignore
            
            if x = 39 then
                sb.AppendLine() |> ignore
            h+1
        ) (0)
        |> ignore
        write (sb.ToString())
