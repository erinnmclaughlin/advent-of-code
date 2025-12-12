namespace AdventOfCode.Tests.Y2025

open Xunit
open AdventOfCode

type Day02Tests() =

    let solver : IAdventDay = SolverRegistry.get 2025 2

    let sample = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124"

    [<Fact>]
    member _.Sample_matches_part_one_problem_statement() =
        let p1, _ = solver.Solve sample
        Assert.Equal("1227775554", p1)

    [<Fact>]
    member _.Sample_matches_part_two_problem_statement() =
        let _, p2 = solver.Solve sample
        Assert.Equal("4174379265", p2)
