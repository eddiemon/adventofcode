module Day13

open System
open Xunit
open Xunit.Abstractions

type Packet =
    | Integer of int
    | Subpacket of Packet list

type PacketPair =
    { Id: int
      Left: Packet
      Right: Packet }

let rec compare left right : int =
    let compareNum a b = sign(a-b)
    let compareSubpackets l r = Seq.zip l r |> Seq.map (fun (l,r) -> compare l r) |> Seq.tryFind ((<>)0)
    
    match left, right with
    | Integer l, Integer r ->  compareNum l r
    | Integer l, Subpacket _ -> compare (Subpacket [left]) right
    | Subpacket _, Integer r -> compare left (Subpacket [right])
    | Subpacket l, Subpacket r ->
        match compareSubpackets l r with
        | None -> compareNum l.Length r.Length
        | Some c -> c
let comparePair pair = compare pair.Left pair.Right

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let personalInput = System.IO.File.ReadAllText("../../../13.txt")
    let example =
        """[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]"""
        
    let parseInput inputType =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        input
            .Split("\n\n", StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.map (fun l ->
            l.Split("\n")
            |> Array.map (fun l ->
                let rec parsePacket idx : int*Packet =
                    let mutable packets = []
                    let mutable idx' = idx
                    let mutable integer = ""
                    let mutable endOfPacket = false
                    while idx' < l.Length && not endOfPacket do 
                        match l[idx'] with
                        | '[' ->
                            let (idx'', subpacket) = parsePacket (idx'+1)
                            packets <- subpacket :: packets
                            idx' <- idx''
                        | ']' ->
                            if integer.Length > 0 then
                                let value = int integer
                                packets <- Integer(value) :: packets
                            endOfPacket <- true
                            idx' <- idx' + 1
                        | ',' ->
                            if integer.Length > 0 then
                                let value = int integer
                                packets <- Integer(value) :: packets
                                integer <- ""
                            idx' <- idx' + 1
                        | value ->
                            integer <- integer + (string value)
                            idx' <- idx' + 1
                    (idx' + 1, Subpacket(packets |> List.rev))
                
                let (_, packet) = parsePacket 1
                packet
            ))
        |> Array.mapi (fun id [|first;second|] -> {Id=id+1; Left=first;Right=second})

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let packetPairs = parseInput inputType
        let score =
            packetPairs
            |> Array.filter (fun p -> -1 = comparePair p)
            |> Array.sumBy (fun p -> p.Id)
        
        write score
    
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let input = parseInput inputType
        
        let decoderKey =
            input
            |> Array.append [|{Id = -1; Left=Subpacket [Subpacket [Integer 2]];Right=Subpacket [Subpacket [Integer 6]]}|]
            |> Array.collect (fun p -> [|p.Left;p.Right|])
            |> Array.sortWith compare
            |> Array.mapi (fun i x -> (i+1,x))
            |> Array.filter (fun (_,x) -> (x = Subpacket [Subpacket [Integer 2]]) || (x = Subpacket [Subpacket [Integer 6]]))
            |> Array.map (fun (i,_) -> i)
            |> Array.reduce (*)
        write decoderKey
