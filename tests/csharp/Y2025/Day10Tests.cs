using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day10Tests
{
    private readonly Day10 _solver = new();

    private const string Sample = """
    [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
    [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("7", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("", p2);
    }

    [Fact]
    public void Get_calculate_lowest_cost()
    {
        var lineToParse = InputHelper.GetLines(Sample)[0];
        var parsed = Day10.Instruction.Parse(lineToParse);
        var cost = parsed.CountFewestSteps();
        Assert.Equal(2, cost);
    }

    [Fact]
    public void Can_parse_initial_state()
    {
        var lineToParse = InputHelper.GetLines(Sample)[0];
        var parsed = Day10.Instruction.Parse(lineToParse);
        
        Assert.Equal(".##.", parsed.TargetState);
        Assert.Equivalent(new[] { 3,5,4,7 }, parsed.JoltageRequirements);

        Assert.Equal(6, parsed.Buttons.Count);
        Assert.Equal(new [] { 3 }, parsed.Buttons["3"]);
        Assert.Equal(new [] { 1, 3 }, parsed.Buttons["1,3"]);
    }
}
