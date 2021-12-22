let input = System.IO.File.ReadAllLines "22.txt"

type CubeState =
    | On
    | Off

type Cuboid = {
    SetState: CubeState
    X: seq<int>
    Y: seq<int>
    Z: seq<int>
}

type Cube = int*int*int
type World = System.Collections.Generic.Dictionary<Cube, CubeState> //Map<Cube, CubeState>

let parse line: Cuboid =
    let matches = System.Text.RegularExpressions.Regex("^(?<state>on|off) x=(?<xstart>-?\\d+)..(?<xend>-?\\d+),y=(?<ystart>-?\\d+)..(?<yend>-?\\d+),z=(?<zstart>-?\\d+)..(?<zend>-?\\d+)$").Match(line)
    let state = matches.Groups["state"].Value |> function | "on" -> On | "off" -> Off
    let xstart = matches.Groups["xstart"].Value |> int
    let xend = matches.Groups["xend"].Value |> int
    let ystart = matches.Groups["ystart"].Value |> int
    let yend = matches.Groups["yend"].Value |> int
    let zstart = matches.Groups["zstart"].Value |> int
    let zend = matches.Groups["zend"].Value |> int
    {
        SetState = state
        X = seq { for x in xstart..xend do yield x }
        Y = seq { for y in ystart..yend do yield y }
        Z = seq { for z in zstart..zend do yield z}
    }

let applyInstruction (world: World) instruction =
    [for z in instruction.Z do
        for y in instruction.Y do
            for x in instruction.X -> (x,y,z)]
    |> Seq.fold (fun (world: World) coord ->
        match instruction.SetState with
        | On -> world.[coord] <- instruction.SetState; world
        | Off -> world.Remove(coord) |> ignore; world
    ) world

let filterInRegion (x: seq<int>) (y: seq<int>) (z: seq<int>) instruction =
    (instruction.X |> Seq.head) >= (x |> Seq.head) &&
    (instruction.X |> Seq.last) <= (x |> Seq.last) &&
    (instruction.Y |> Seq.head) >= (y |> Seq.head) &&
    (instruction.Y |> Seq.last) <= (y |> Seq.last) &&
    (instruction.Z |> Seq.head) >= (z |> Seq.head) &&
    (instruction.Z |> Seq.last) <= (z |> Seq.last)

let filter = Seq.filter (filterInRegion [-50..50] [-50..50] [-50..50])

let instructions = input |> Seq.map parse

let world = instructions |> filter |> Seq.fold applyInstruction (World())

world.Count