using Day08 = AdventOfCode.Y2025.Day08;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day08Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get(2025, 8);

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
        var circuits = Day08.ProcessCircuits(Sample, 10);
        var product = Day08.GetProduct(circuits);
        Assert.Equal(40, product);
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
        var junctionBoxes = Day08.ParseInput(Sample).ToArray();
        var ordered = Day08.EnumeratePairs(junctionBoxes).OrderBy(x => x.Box1.GetDistanceTo(x.Box2)).Take(5).ToList();
        
        Assert.Equal("<162, 817, 812>", ordered[0].Box1.ToString());
        Assert.Equal("<425, 690, 689>", ordered[0].Box2.ToString());
        
        Assert.Equal("<162, 817, 812>", ordered[1].Box1.ToString());
        Assert.Equal("<431, 825, 988>", ordered[1].Box2.ToString());
        
        Assert.Equal("<906, 360, 560>", ordered[2].Box1.ToString());
        Assert.Equal("<805, 96, 715>", ordered[2].Box2.ToString());
        
        Assert.Equal("<431, 825, 988>", ordered[3].Box1.ToString());
        Assert.Equal("<425, 690, 689>", ordered[3].Box2.ToString());
    }
    
    [Theory]
    [InlineData(1, 19)]
    [InlineData(2, 18)]
    [InlineData(3, 17)]
    [InlineData(10, 11)]
    public void Test_connection_count(int steps, int expected)
    {
        var circuits = Day08.ProcessCircuits(Sample, steps);
        Assert.Equal(expected, circuits.Length);
    }

    [Theory]
    [InlineData(5, 1)]
    [InlineData(4, 1)]
    [InlineData(2, 2)]
    [InlineData(1, 7)]
    public void Test_junction_count(int junctionCount, int expectedNumCircuits)
    {
        var circuits = Day08.ProcessCircuits(Sample, 10);
        Assert.Equal(expectedNumCircuits, circuits.Count(c => c.Junctions.Count == junctionCount));
    }
}
