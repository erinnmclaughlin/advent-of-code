using AdventOfCode.Y2024;

namespace AdventOfCode.Tests.Y2024;

public sealed class Day01Tests
{
    private readonly Day01 _solver = new();

    private const string Sample = """
    3   4
    4   3
    2   5
    1   3
    3   9
    3   3
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("11", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("31", p2);
    }
}
