let input = System.IO.File.ReadAllLines "10.txt"

let points ch =
    match ch with
    | Some ')' -> 3L
    | Some ']' -> 57L
    | Some '}' -> 1197L
    | Some '>' -> 25137L
    | Some _ | None -> 0L

let opposite ch =
    match ch with
    | ')' -> '('
    | ']' -> '['
    | '}' -> '{'
    | '>' -> '<'
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


// printfn "%A" (firstIllegalChar "{([(<{}[<>[]}>{[]{[(<()>") // }
// printfn "%A" (firstIllegalChar "[[<[([]))<([[{}[[()]]]" )// )
// printfn "%A" (firstIllegalChar "[{[{({}]{}}([{[{{{}}([]") // ]
// printfn "%A" (firstIllegalChar "[<(<(<(<{}))><([]([]()" )// )
// printfn "%A" (firstIllegalChar "<{([([[(<>()){}]>(<<{{" )// >

let syntaxErrorScore =
    input
    |> Seq.map firstIllegalChar
    |> Seq.sumBy points

printfn "%A" syntaxErrorScore