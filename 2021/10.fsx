let input = System.IO.File.ReadAllLines "10.txt"

let points ch =
    match ch with
    | ')' -> 3L
    | ']' -> 57L
    | '}' -> 1197L
    | '>' -> 25137L
    | _ -> 0L

let opposite ch =
    match ch with
    | ')' -> '('
    | ']' -> '['
    | '}' -> '{'
    | '>' -> '<'
    | '(' -> ')'
    | '{' -> '}'
    | '[' -> ']'
    | '<' -> '>'
    | _ -> ' '

let processLine line =
    let mutable S = []
    let mutable processed = []
    for c in line do
        match c with
        | '(' | '{' | '[' | '<' ->
            S <- c :: S
            processed <- (c, true) :: processed
        | ')' | '}' | ']' | '>' ->
            processed <- (c, (List.head S = opposite c)) :: processed
            S <- List.tail S
        | _ -> ignore 0
    processed

let firstIllegalChar line =
    let processed = processLine line
    let illegalChar = Seq.tryFind (fun (_, legal) -> not legal) processed
    match illegalChar with
    | Some (c, _) -> Some c
    | None -> None

let maybePoints chOpt =
    match chOpt with
    | Some c -> points c
    | None -> 0

let syntaxErrorScore =
    input
    |> Seq.map firstIllegalChar
    |> Seq.sumBy maybePoints

printfn "%A" syntaxErrorScore

let incompleteInput =
    input
    |> Seq.filter (fun line ->
        None = firstIllegalChar line
    )
    |> List.ofSeq

let incompleteChars line =
    let mutable S = []
    for c in line do
        match c with
        | '(' | '{' | '[' | '<' ->
            S <- c :: S
        | ')' | '}' | ']' | '>' ->
            S <- List.tail S
        | _ -> ignore 0
    S
    
let getCompletion incompleteChars =
    incompleteChars
    |> Seq.map opposite

let points2 ch =
    match ch with
    | ')' -> 1L
    | ']' -> 2L
    | '}' -> 3L
    | '>' -> 4L
    | _ -> 0L

let scoreCompletion completion =
    completion
    |> Seq.fold (fun acc c ->
        acc * 5L + (points2 c)
    ) 0L

let completionScores =
    incompleteInput
    |> Seq.map incompleteChars
    |> Seq.map getCompletion
    |> Seq.map scoreCompletion

completionScores
|> Seq.sort
|> Seq.item ((Seq.length completionScores) / 2)