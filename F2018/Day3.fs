module Day3

open Xunit
open Xunit.Abstractions

type Rectangle =
    {
      x: int; width: int
      y: int; height : int
    }
    member this.xmax = this.x + this.width - 1
    member this.ymax = this.y + this.height - 1

type Claim =
    { id: string
      r: Rectangle }

let parseClaim s =
    let m = System.Text.RegularExpressions.Regex.Match(s, @"^#(?<id>[^@]+)@(?<x>[^,]+),(?<y>[^:]+):(?<width>[^x]+)x(?<height>.+)")
    {
        id = m.Groups["id"].Value.Trim()
        r = {
            x = int m.Groups["x"].Value
            y = int m.Groups["y"].Value
            width = int m.Groups["width"].Value
            height = int m.Groups["height"].Value
        }
    }


type Tests(output: ITestOutputHelper) =
    let write message = output.WriteLine message
    
    let claims =
        System.IO.File.ReadAllLines "../../../3.in"
//         """#1 @ 1,3: 4x4
// #2 @ 3,1: 4x4
// #3 @ 5,5: 2x2""".Split "\n"
        |> Seq.map parseClaim
        |> Seq.toList
    
    [<Fact>]
    let ``Part 1`` () =
        let upsertAddOne x =
            match x with
            | Some i -> Some (i + 1)
            | None -> Some 1

        let overlappingArea =
            claims
            |> Seq.collect (fun c -> Seq.allPairs (seq { c.r.x .. c.r.xmax }) (seq { c.r.y .. c.r.ymax }))
            |> Seq.fold (fun d p -> Map.change p upsertAddOne d) Map.empty<int*int, int>
            |> Map.fold (fun sum k v -> match v with
                                        | 0 | 1 -> sum
                                        | _ -> sum + 1) 0
        write (string overlappingArea)
 
    
    [<Fact>]
    let ``Part 2`` () =
        let upsertAddOne (v:string) (x:Set<string> option) =
            match x with
            | Some i -> Some (Set.add v i)
            | None -> Some (Set.singleton<string> v)
 
        let nonOverlappingId =
            claims
            |> Seq.collect (fun c -> Seq.allPairs (seq { c.r.x .. c.r.xmax }) (seq { c.r.y .. c.r.ymax }) |> Seq.map (fun p -> (p,c.id)))
            |> Seq.fold (fun d p -> Map.change (fst p) (upsertAddOne (snd p) ) d) Map.empty<int*int, Set<string>>
            |> Map.values
            |> Seq.fold (fun s ids ->
                match Set.count ids with
                | 1 -> s
                | _ -> Set.union s ids) Set.empty<string>
            |> Set.difference (claims |> Seq.map (fun c -> c.id) |> Set.ofSeq)

        write (string nonOverlappingId)
 