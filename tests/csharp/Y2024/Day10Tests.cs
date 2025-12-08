namespace AdventOfCode.Tests.Y2024;

public sealed class Day10Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2024, 10);

    private const string Sample = """
    89010123
    78121874
    87430965
    96549874
    45678903
    32019012
    01329801
    10456732
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("36", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("81", p2);
    }
}
