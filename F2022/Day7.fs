module Day7

open System
open Xunit
open Xunit.Abstractions

type File = {
    Name: string
    Size: int
}
type Directory =
    { Name: string
      mutable Dirs: Directory list
      mutable Files: File list }
    
    member this.Size() =
        lazy (
            this.Files
            |> List.sumBy (fun f -> f.Size)
        )

    member this.TotalSize() =
        lazy (
            this.Size().Value + (this.Dirs |> List.sumBy (fun d -> d.TotalSize().Value))
        )

let createDirectory name = {Name = name; Dirs=List.empty;Files=List.empty }

let rootDirectory = createDirectory "/"

type Tests(output: ITestOutputHelper) =
    let write obj =
        output.WriteLine (string obj)

    let parseLine (dirStack: Directory list) (line: string) =
        let split = line.Split(" ")
        match split[0] with
        | "$" when split[1] = "ls" -> dirStack
        | "$" when split[1] = "cd" && split[2] = "/" ->
            match List.length dirStack with
            | 0 -> createDirectory "/" :: dirStack
            | _ -> [List.last dirStack]
        | "$" when split[1] = "cd" && split[2] = ".." ->
            List.tail dirStack
        | "$" when split[1] = "cd" ->
            let newDirectory = createDirectory split[2]
            dirStack.Head.Dirs <- newDirectory :: dirStack.Head.Dirs
            newDirectory :: dirStack
        | "dir" -> dirStack
        | _ ->
            let [|size;name|] = split
            dirStack.Head.Files <- {Name=name;Size=(int size)} :: dirStack.Head.Files
            dirStack
            
    let example =
        """$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k"""

    let personalInput = System.IO.File.ReadAllText("../../../7.txt")

    let parseInput (inputType: string) =
        let input =
            match inputType with
            | "example" -> example
            | "input" -> personalInput
        
        input
            .Split('\n', StringSplitOptions.RemoveEmptyEntries ||| StringSplitOptions.TrimEntries)
        |> Array.fold parseLine []
        |> List.last
        
    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 1`` (inputType) =
        let rec SumSizeOver100000 (directory: Directory) =
            let dirSize = directory.TotalSize()
            let size = if dirSize.Value <= 100_000 then dirSize.Value else 0

            let subdirSize =
                directory.Dirs
                |> List.sumBy (fun d -> SumSizeOver100000 d)
            
            size + subdirSize
        
        let rootDir = parseInput inputType
        write (SumSizeOver100000 rootDir)

    [<Theory>]
    [<InlineData("example")>]
    [<InlineData("input")>]
    let ``Part 2`` (inputType) =
        let rootDir = parseInput inputType
        
        let freeSize = 70000000 - rootDir.TotalSize().Value
        let spaceToFree = 30000000 - freeSize
        
        let rec flattenDirectories directory =
            let subDirs =
                directory.Dirs
                |> List.collect flattenDirectories
            directory :: subDirs

        let directories = flattenDirectories rootDir
        let result =
            directories
            |> List.filter (fun d -> d.TotalSize().Value >= spaceToFree)
            |> List.sortBy (fun d -> d.TotalSize().Value)
            |> List.head
            |> fun d -> d.TotalSize().Value
        
        output.WriteLine (string result)
