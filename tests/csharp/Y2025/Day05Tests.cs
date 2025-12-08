using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day05Tests
{
    private readonly Day05 _solver = new();

    private const string Sample = """
    3-5
    10-14
    16-20
    12-18

    1
    5
    8
    11
    17
    32
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("3", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("14", p2);
    }
}
