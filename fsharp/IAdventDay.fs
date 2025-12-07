namespace AdventOfCode.FSharp

type IAdventDay =
    abstract member Year : int
    abstract member Day  : int
    abstract member Solve : string -> string * string
