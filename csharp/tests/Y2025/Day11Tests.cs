using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day11Tests
{
    private readonly Day11 _solver = new();

    private const string Sample1 = """
    aaa: you hhh
    you: bbb ccc
    bbb: ddd eee
    ccc: ddd eee fff
    ddd: ggg
    eee: out
    fff: out
    ggg: out
    hhh: ccc fff iii
    iii: out
    """;

    private const string Sample2 =
    """
    svr: aaa bbb
    aaa: fft
    fft: ccc
    bbb: tty
    tty: ccc
    ccc: ddd eee
    ddd: hub
    hub: fff
    eee: dac
    dac: fff
    fff: ggg hhh
    ggg: out
    hhh: out
    """;
    
    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample1);
        Assert.Equal("5", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample2);
        Assert.Equal("2", p2);
    }
}
