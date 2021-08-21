let applyRules input rules =
    Seq.forall (fun rule -> rule input) rules

let applyRulesToInputs inputs rules =
    Seq.filter (fun input -> applyRules input rules) inputs

let threeVowelsRule (input: string) =
    Seq.countBy id input
    |> Seq.where (fun (c, _) -> match c with | 'a' | 'e' | 'i' |'o' | 'u' -> true | _ -> false)
    |> Seq.sumBy snd
    |> fun n -> n >= 3

let oneLetterTwiceInRowRule (input:string) =
    input
    |> Seq.windowed 2
    |> Seq.exists (fun [|a; b|] -> a = b)

let noneOfTheseRule (lst: string list) (input: string) =
    lst
    |> List.exists input.Contains
    |> not

let rules = [ threeVowelsRule; oneLetterTwiceInRowRule; noneOfTheseRule ["ab"; "cd"; "pq"; "xy"] ]

let inputs = System.IO.File.ReadAllLines("5.txt")
printfn "%A" (applyRulesToInputs inputs rules |> Seq.length)