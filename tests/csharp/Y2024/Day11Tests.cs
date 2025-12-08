namespace AdventOfCode.Tests.Y2024;

public sealed class Day11Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2024, 11);

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve("125 17");
        Assert.Equal("55312", p1);
    }
}
