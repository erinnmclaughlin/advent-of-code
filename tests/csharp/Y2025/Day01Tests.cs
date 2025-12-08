using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day01Tests
{
    private readonly Day01 _solver = new();

    private const string Sample = """
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
        Assert.Equal("6", p2);
    }
}
