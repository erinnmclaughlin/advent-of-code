using AdventOfCode.Y2025;
using Day08 = AdventOfCode.Y2025.Day08;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day08Tests
{
    private readonly Day08 _solver = new();

    private const string Sample = """
    162,817,812
    57,618,57
    906,360,560
    592,479,940
    352,342,300
    466,668,158
    542,29,236
    431,825,988
    739,650,466
    52,470,668
    216,146,977
    819,987,18
    117,168,530
    805,96,715
    346,949,466
    970,615,88
    941,993,340
    862,61,35
    984,92,344
    425,690,689
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = Day08.Solve(Sample, 10);
        Assert.Equal("40", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("25272", p2);
    }
    
    [Fact]
    public void Test_sort_order()
    {
        var ordered = Day08.ParseInput(Sample).EnumerateOrderedPairs().Take(4).ToList();
        
        Assert.Equal((162, 817, 812), ordered[0].Box1.Position);
        Assert.Equal((425, 690, 689), ordered[0].Box2.Position);
        
        Assert.Equal((162, 817, 812), ordered[1].Box1.Position);
        Assert.Equal((431, 825, 988), ordered[1].Box2.Position);
        
        Assert.Equal((906, 360, 560), ordered[2].Box1.Position);
        Assert.Equal((805, 096, 715), ordered[2].Box2.Position);
        
        Assert.Equal((431, 825, 988), ordered[3].Box1.Position);
        Assert.Equal((425, 690, 689), ordered[3].Box2.Position);
    }
}
