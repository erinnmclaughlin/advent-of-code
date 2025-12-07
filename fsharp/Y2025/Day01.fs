namespace AdventOfCode.FSharp.Y2025

open AdventOfCode.FSharp
open System

type Day01() =

    let parseInstruction (instruction: string) =
        let increment = if instruction[0] = 'L' then -1 else 1
        let value = instruction[1..] |> int
        increment, value

    let parseInput (input: string) =
        input.Split(
            '\n',
            StringSplitOptions.RemoveEmptyEntries |||
            StringSplitOptions.TrimEntries)

    let executeInstruction current p1 p2 (instruction: string) =
        let inc, ticks = parseInstruction instruction

        let next, nextPart2 =
            [0..ticks-1]
            |> Seq.fold(fun (c, zeroCount) _ -> 
                let newC = (c + inc + 100) % 100
                let newZeroCount = if newC = 0 then zeroCount + 1 else zeroCount
                newC, newZeroCount
            )(current, p2)
        
        let nextPart1 = p1 + if next = 0 then 1 else 0

        next, nextPart1, nextPart2

    interface IAdventDay with
        member _.Year = 2025
        member _.Day  = 1

        member _.Solve input =
            input
            |> parseInput
            |> Seq.fold (fun (current, p1, p2) inst -> executeInstruction current p1 p2 inst) (50, 0, 0)
            |> fun (_, p1, p2) -> p1.ToString(), p2.ToString()