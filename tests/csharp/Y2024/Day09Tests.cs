namespace AdventOfCode.Tests.Y2024;

public sealed class Day09Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2024, 9);

    private const string Sample = "2333133121414131402";

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("1928", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("2858", p2);
    }
}
