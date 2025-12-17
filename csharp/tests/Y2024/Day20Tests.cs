using AdventOfCode.Y2024;

namespace AdventOfCode.Tests.Y2024;

public sealed class Day20Tests
{
    private readonly Day20 _solver = new();

    private const string Sample = """
    ###############
    #...#...#.....#
    #.#.#.#.#.###.#
    #S#...#.#.#...#
    #######.#.#.###
    #######.#.#...#
    #######.#.###.#
    ###..E#...#...#
    ###.#######.###
    #...###...#...#
    #.#####.#.###.#
    #.#...#.#.#...#
    #.#.#.#.#.#.###
    #...#...#...###
    ###############
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("0", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("", p2);
    }
    
    // There are 14 cheats that save 2 picoseconds.
    // There are 14 cheats that save 4 picoseconds.
    // There are 2 cheats that save 6 picoseconds.
    // There are 4 cheats that save 8 picoseconds.
    // There are 2 cheats that save 10 picoseconds.
    // There are 3 cheats that save 12 picoseconds.
    // There is one cheat that saves 20 picoseconds.
    // There is one cheat that saves 36 picoseconds.
    // There is one cheat that saves 38 picoseconds.
    // There is one cheat that saves 40 picoseconds.
    // There is one cheat that saves 64 picoseconds.
    [Theory]
    [InlineData(1, 3, 14)]
    [InlineData(3, 5, 14)]
    [InlineData(5, 7, 2)]
    [InlineData(7, 9, 4)]
    [InlineData(9, 11, 2)]
    [InlineData(11, 13, 3)]
    [InlineData(19, 21, 1)]
    [InlineData(35, 37, 1)]
    [InlineData(37, 39, 1)]
    [InlineData(39, 41, 1)]
    [InlineData(63, 65, 1)]
    public void Can_get_saved_step_count(int min, int max, int expected)
    {
        var lines = InputHelper.GetLines(Sample);
        var (maze, start, end) = Day20.ParseLines(lines);
        Assert.Equal(expected, Day20.GetSavedStepCountAlt(maze, start, end, min, max));
    }
}
