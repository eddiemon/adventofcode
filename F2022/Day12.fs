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
        let unvisited = HashSet(nodes.Keys)
        let tentativeDistance =
            nodes.Keys |> Seq.map (fun n -> KeyValuePair(n, Int32.MaxValue)) |> fun vs -> Dictionary(vs)
        
        tentativeDistance[startNode] <- 0
        
        while unvisited.Contains(endNode) do
            let nextNode =
                tentativeDistance
                |> Seq.filter (fun kvp -> unvisited.Contains(kvp.Key))
                |> Seq.sortBy (fun kvp -> kvp.Value)
                |> Seq.head
            let currentNode = nextNode.Key
            let currentDistance = nextNode.Value + 1
            
            let neighbours =
                [|(-1, 0); (1, 0); (0, -1); (0, 1)|]
                |> Array.map (fun (dx,dy) -> (fst currentNode + dx, snd currentNode + dy))
                |> Array.filter (fun p -> unvisited.Contains(p) && ((height nodes[p]) - (height nodes[currentNode])) <= 1)

            for n in neighbours do
                let distance = tentativeDistance[n]
                if currentDistance < distance then
                    tentativeDistance[n] <- currentDistance
                else
                    ()
            unvisited.Remove(currentNode) |> ignore
        tentativeDistance.Item endNode

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
