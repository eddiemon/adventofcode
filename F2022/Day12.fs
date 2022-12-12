module Day12

open System
open System.Collections.Generic
open Xunit
open Xunit.Abstractions

type Node = (int*int)*char
let height c =
        match c with
        | 'S' -> 0
        | 'E' -> int 'z' - int 'a'
        | _ -> int c - int 'a'

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../12.txt")
    let example =
        """Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.mapi (fun y l -> l.ToCharArray() |> Array.mapi (fun x c -> Node((x,y), c)))
        |> Array.collect id
        |> dict
        
    let shortestPath startNode endNode (nodes:IDictionary<(int*int),char>) =
        let q = PriorityQueue<int*int, int64>()
        let distances = Dictionary<int*int, int64>()
        distances.Add(startNode, 0)
        q.Enqueue(startNode, 0)
        
        while q.Count > 0 do
            let currentNode = q.Dequeue()
            let currentDistance = distances[currentNode] + 1L
            
            let neighbours =
                [|(-1, 0); (1, 0); (0, -1); (0, 1)|]
                |> Array.map (fun (dx,dy) -> (fst currentNode + dx, snd currentNode + dy))
                |> Array.filter (fun p -> nodes.ContainsKey(p) && ((height nodes[p]) - (height nodes[currentNode])) <= 1)

            for n in neighbours do
                let distance = if distances.ContainsKey(n) then distances[n] else Int64.MaxValue
                if currentDistance < distance then
                    distances[n] <- currentDistance
                    q.Enqueue(n, currentDistance)
                else
                    ()
        if distances.ContainsKey(endNode) then distances[endNode] else Int64.MaxValue

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let nodes = parseInput inputType
        
        let startNode = nodes |> Seq.filter (fun kvp -> kvp.Value = 'S') |> Seq.exactlyOne |> fun kvp -> kvp.Key
        let endNode = nodes |> Seq.filter (fun kvp -> kvp.Value = 'E') |> Seq.exactlyOne |> fun kvp -> kvp.Key
        
        let endNodeDistance = shortestPath startNode endNode nodes
        write endNodeDistance
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let nodes = parseInput inputType
        
        let startNodes = nodes |> Seq.filter (fun kvp -> kvp.Value = 'a') |> Seq.map (fun kvp -> kvp.Key)
        let endNode = nodes |> Seq.filter (fun kvp -> kvp.Value = 'E') |> Seq.exactlyOne |> fun kvp -> kvp.Key
        
        let shortestScenicPath =
            startNodes
            |> Seq.map (fun startNode -> shortestPath startNode endNode nodes)
            |> Seq.min
        
        write shortestScenicPath
