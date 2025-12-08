namespace AdventOfCode.Tests.Y2024;

public class Day03Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2024, 3);

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve("xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))");
        Assert.Equal("161", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve("xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))");
        Assert.Equal("48", p2);
    }
}
