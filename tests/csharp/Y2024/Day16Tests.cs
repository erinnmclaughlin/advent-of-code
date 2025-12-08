namespace AdventOfCode.Tests.Y2024;

public sealed class Day16Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2024, 16);

    private const string Sample1 = """
    ###############
    #.......#....E#
    #.#.###.#.###.#
    #.....#.#...#.#
    #.###.#####.#.#
    #.#.#.......#.#
    #.#.#####.###.#
    #...........#.#
    ###.#.#####.#.#
    #...#.....#.#.#
    #.#.#.###.#.#.#
    #.....#...#.#.#
    #.###.#.#.#.#.#
    #S..#.....#...#
    ###############
    """;

    private const string Sample2 = """
    #################
    #...#...#...#..E#
    #.#.#.#.#.#.#.#.#
    #.#.#.#...#...#.#
    #.#.#.#.###.#.#.#
    #...#.#.#.....#.#
    #.#.#.#.#.#####.#
    #.#...#.#.#.....#
    #.#.#####.#.###.#
    #.#.#.......#...#
    #.#.###.#####.###
    #.#.#...#.....#.#
    #.#.#.#####.###.#
    #.#.#.........#.#
    #.#.#.#########.#
    #S#.............#
    #################
    """;

    [Theory]
    [InlineData(Sample1, "7036")]
    [InlineData(Sample2, "11048")]
    public void Sample_matches_part_one_problem_statement(string sample, string expected)
    {
        var (p1, _) = _solver.Solve(sample);
        Assert.Equal(expected, p1);
    }

    [Theory]
    [InlineData(Sample1, "45")]
    [InlineData(Sample2, "64")]
    public void Sample_matches_part_two_problem_statement(string sample, string expected)
    {
        var (_, p2) = _solver.Solve(sample);
        Assert.Equal(expected, p2);
    }
}
