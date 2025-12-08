namespace AdventOfCode.Tests.Y2025;

public class Day03Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2025, 3);

    private const string Sample = """
        987654321111111
        811111111111119
        234234234234278
        818181911112111
        """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("357", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("3121910778619", p2);
    }
}
