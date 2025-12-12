namespace AdventOfCode.Tests.Y2025

open Xunit
open AdventOfCode

type Day01Tests() =

    let solver : IAdventDay = SolverRegistry.get 2025 1

    let sample = """
        L68
        L30
        R48
        L5
        R60
        L55
        L1
        L99
        R14
        L82
        """

    [<Fact>]
    member _.Sample_matches_part_one_problem_statement() =
        let p1, _ = solver.Solve sample
        Assert.Equal("3", p1)

    [<Fact>]
    member _.Sample_matches_part_two_problem_statement() =
        let _, p2 = solver.Solve sample
        Assert.Equal("6", p2)
