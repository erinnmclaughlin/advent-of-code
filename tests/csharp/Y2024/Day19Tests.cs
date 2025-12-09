using AdventOfCode.Y2024;

namespace AdventOfCode.Tests.Y2024;

public class Day19Tests
{
    private readonly Day19 _solver = new();

    private const string Sample = """
    r, wr, b, g, bwu, rb, gb, br

    brwrr
    bggr
    gbbr
    rrbgbr
    ubwu
    bwurrg
    brgr
    bbrgwb
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("6", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("", p2);
    }

    [Theory]
    [InlineData("brwrr", true)]
    [InlineData("bggr", true)]
    [InlineData("gbbr", true)]
    [InlineData("rrbgbr", true)]
    [InlineData("ubwu", false)]
    [InlineData("bwurrg", true)]
    [InlineData("brgr", true)]
    [InlineData("bbrgwb", false)]
    public void CanMake(string request, bool expected)
    {
        var towels = Day19.ParseTowels("r, wr, b, g, bwu, rb, gb, br");
        var result = Day19.CanMake(request, towels);
        Assert.Equal(expected, result);
    }
}
