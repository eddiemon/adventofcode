module Day1

open System
open Xunit
open Xunit.Abstractions

let input = System.IO.File.ReadAllLines("../../../1.txt")
let example1 = [|
    "1abc2"
    "pqr3stu8vwx"
    "a1b2c3d4e5f"
    "treb7uchet"
|]

let example2 = [|
    "two1nine"
    "eightwothree"
    "abcone2threexyz"
    "xtwone3four"
    "4nineeightseven2"
    "zoneight234"
    "7pqrstsixteen"
|]

type Tests(output: ITestOutputHelper) =
    [<Fact>]
    let ``Part 1`` () =
        let calval (s: char array) =
            let l = Array.find Char.IsNumber s |> int |> fun i -> i - 48
            let r = Array.findBack Char.IsNumber s |> int |> fun i -> i - 48
            l * 10 + r

        let calibrationValue =
            input
            |> Array.map (fun s -> s.ToCharArray())
            |> Array.map calval
            |> Array.sum

        output.WriteLine(calibrationValue.ToString())

    [<Fact>]
    let ``Part 2`` () =
        let digits =
            [|"1";"one";"2";"two";"3";"three";"4";"four";"5";"five";"6";"six";"7";"seven";"8";"eight";"9";"nine"|]
            |> Array.mapi (fun i d -> (i / 2 + 1, d))

        let calval (s: string) =
            let l =
                digits
                |> Array.choose (fun (i, d) -> match s.IndexOf(d) with | -1 -> None  | idx -> Some (i, idx))
                |> Array.minBy snd
                |> fst
            let r =
                digits
                |> Array.choose (fun (i, d) -> match s.LastIndexOf(d) with | -1 -> None  | idx -> Some (i, idx))
                |> Array.maxBy snd
                |> fst
            l * 10 + r

        let calibrationValue =
            input
            |> Array.map calval
            |> Array.sum
        output.WriteLine(calibrationValue.ToString())