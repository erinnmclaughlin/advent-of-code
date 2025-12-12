open System
open AdventOfCode

[<EntryPoint>]
let main argv =
    if argv.Length < 2 then
        eprintfn "Usage: aoc <year> <day>"
        1
    else
        let tryParseInt (s: string) =
            match Int32.TryParse s with
            | true, v -> Some v
            | _ -> None

        match tryParseInt argv[0], tryParseInt argv[1] with
        | Some year, Some day ->
            try
                let solver = SolverRegistry.get year day

                let input = Console.In.ReadToEnd()

                if String.IsNullOrWhiteSpace input then
                    eprintfn "No input provided on stdin."
                    1
                else
                    let part1, part2 = solver.Solve input
                    printfn "Part 1: %s" part1
                    printfn "Part 2: %s" part2
                    0
            with ex ->
                eprintfn "%s" ex.Message
                1

        | _ ->
            eprintfn "Year and day must be integers."
            1
