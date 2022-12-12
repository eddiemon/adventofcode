module Day11

open System
open Xunit
open Xunit.Abstractions

type WorryLevel = bigint
type MonkeyId = int
type Monkey = {
    Id: int
    mutable Items: WorryLevel list
    Operation: WorryLevel -> WorryLevel
    Test: WorryLevel -> MonkeyId
    TestDivisor: WorryLevel
}

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../11.txt")
    let example =
        """Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.map (fun monkeyDescription ->
            let lines = monkeyDescription.Split("\n", StringSplitOptions.TrimEntries)
            let id = (lines[0].Split(":")[0]).Split(" ")[1] |> int
            let startingItems = lines[1].Split(": ") |> Array.skip 1 |> Array.exactlyOne |> fun l -> l.Split(", ") |> Array.map (fun s -> WorryLevel.Parse s) |> List.ofArray
            let operation = lines[2].Split("Operation: new = old ") |> Array.skip 1 |> Array.exactlyOne |> fun l ->
                let l' = l.Split(" ")
                let op = l'[0]
                let term = l'[1]
                match op with
                | "+" -> fun old -> old + (WorryLevel.Parse term)
                | "-" -> fun old -> old - (WorryLevel.Parse term)
                | "*" when term = "old" -> fun old -> old * old
                | "*" -> fun old -> old * (WorryLevel.Parse term)
            let trueMonkey = lines[4].Split("If true: throw to monkey ", StringSplitOptions.RemoveEmptyEntries) |> Array.exactlyOne |> int
            let falseMonkey = lines[5].Split("If false: throw to monkey ", StringSplitOptions.RemoveEmptyEntries) |> Array.exactlyOne |> int
            let divisibleBy = lines[3].Split("Test: divisible by ", StringSplitOptions.RemoveEmptyEntries) |> Array.exactlyOne |> WorryLevel.Parse
            let test = fun (item: WorryLevel) ->
                match (item % divisibleBy) = WorryLevel.Zero with
                | true -> trueMonkey
                | false -> falseMonkey
            { Id = id; Items = startingItems; Operation = operation; Test = test; TestDivisor = divisibleBy }
        )
        

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let input = parseInput inputType
        let monkeys =
            input
            |> Array.map (fun m -> (m.Id, m))
            |> dict
        let mutable monkeyBusiness =
            input
            |> Array.map (fun m -> (m.Id, 0))
            |> Map.ofArray
            
        let doTurn monkeyId =
            let monkey = monkeys[monkeyId]
            monkey.Items
            |> List.iter (fun item ->
                let mutable newWorryLevel = monkey.Operation item
                newWorryLevel <- newWorryLevel / (bigint 3)
                let throwMonkeyId = monkey.Test newWorryLevel
                monkeys[throwMonkeyId].Items <- monkeys[throwMonkeyId].Items @ [newWorryLevel]
                monkeyBusiness <- Map.change monkeyId (fun v ->
                    match v with
                    | Some value -> Some (value + 1)
                    | None -> Some (1)) monkeyBusiness
            )
            monkey.Items <- List.empty
        
        let doRound() =
            monkeys.Values
            |> Seq.iter (fun m -> doTurn m.Id)
        
        seq {0 .. 19}
        |> List.ofSeq
        |> List.iter (fun _ -> doRound())
        
        let monkeyBusinessScore =
            monkeyBusiness.Values
            |> Seq.sortDescending
            |> Seq.take 2
            |> Seq.reduce (*)
            
        write monkeyBusinessScore
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        let monkeys =
            input
            |> Array.map (fun m -> (m.Id, m))
            |> dict
            
        let divisor =
            input
            |> Array.map (fun m -> m.TestDivisor)
            |> Array.reduce (*)

        let mutable monkeyBusiness =
            input
            |> Array.map (fun m -> (m.Id, 0L))
            |> Map.ofArray
            
        let doTurn monkeyId =
            let monkey = monkeys[monkeyId]
            monkey.Items
            |> List.iter (fun item ->
                let mutable newWorryLevel = monkey.Operation item
                newWorryLevel <- newWorryLevel % divisor
                let throwMonkeyId = monkey.Test newWorryLevel
                monkeys[throwMonkeyId].Items <- monkeys[throwMonkeyId].Items @ [newWorryLevel]
                monkeyBusiness <- Map.change monkeyId (fun v ->
                    match v with
                    | Some value -> Some (value + 1L)
                    | None -> Some (1)) monkeyBusiness
            )
            monkey.Items <- List.empty
        
        let doRound() =
            monkeys.Values
            |> Seq.iter (fun m -> doTurn m.Id)
        
        seq {1 .. 10000}
        |> List.ofSeq
        |> List.iter (fun _ -> doRound())
        
        let monkeyBusinessScore =
            monkeyBusiness.Values
            |> Seq.sortDescending
            |> Seq.take 2
            |> Seq.reduce (*)
            
        write monkeyBusinessScore
