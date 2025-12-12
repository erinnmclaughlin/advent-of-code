using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day06Tests
{
    private readonly Day06 _solver = new();

    private const string Sample = """
    123 328  51 64 
     45 64  387 23 
      6 98  215 314
    *   +   *   +  
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("4277556", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("3263827", p2);
    }
}
