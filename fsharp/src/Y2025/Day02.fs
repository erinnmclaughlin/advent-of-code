namespace AdventOfCode.Y2025

open AdventOfCode

type Day02() =

    let parseRange (line: string) =
        line.Split '-' 
        |> fun parts -> [parts[0] |> int64 .. parts[1] |> int64]

    let getRepeatedString part count =
        Seq.replicate count part |> String.concat ""

    let getPart (idString: string) numParts =
        idString.Length / numParts
        |> fun len -> idString[..len-1]

    let isPartRepeated (idString: string) numParts : bool =
        getPart idString numParts
        |> fun part -> getRepeatedString part numParts 
        |> idString.Equals

    let mustBeValid (idString: string) numParts =
        idString.Length = 1 || idString.Length % numParts <> 0 

    let isValid id numParts : bool =
        id
        |> string
        |> fun idString ->
            mustBeValid idString numParts ||
            isPartRepeated idString numParts |> not

    let parseIdsFromString (content: string) =
        content.Split ','
        |> Seq.collect parseRange
        |> Seq.toArray

    interface IAdventDay with
        member _.Year = 2025
        member _.Day  = 2

        member _.Solve input =
            let ids = input |> parseIdsFromString
            let part1 =
                ids
                |> Seq.filter (fun id -> isValid id 2 |> not)
                |> Seq.sum
            let part2 =
                ids
                |> Seq.filter (fun id -> (
                    let idString = id |> string
                    [2..idString.Length] |> Seq.exists(fun s -> isValid id s |> not)
                ))
                |> Seq.sum

            part1.ToString(), part2.ToString()