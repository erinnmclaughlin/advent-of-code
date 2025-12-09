using System.Drawing;
using System.Text;
using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day09Tests(ITestOutputHelper output)
{
    private readonly Day09 _solver = new();

    private const string Sample = """
    7,1
    11,1
    11,7
    9,7
    9,5
    2,5
    2,3
    7,3
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("50", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("24", p2);
    }
    
    [Theory]
    [InlineData("2,5", "9,7", 24)]
    public void GetAreaTests(string p1, string p2, long expected)
    {
        var point1 = Day09.ParseLine(p1);
        var point2 = Day09.ParseLine(p2);
        var area = Day09.GetArea(point1, point2);
        Assert.Equal(expected, area);
    }

    [Fact]
    public void EnumerateEdgesWorks()
    {
        var points = InputHelper.GetLines(Sample).Select(Day09.ParseLine).ToHashSet();
        var edgePoints = Day09.EnumerateEdges(points).ToHashSet();

        var minX = points.Min(p => p.X) - 2;
        var maxX = points.Max(p => p.X) + 2;
        var minY = points.Min(p => p.Y) - 1;
        var maxY = points.Max(p => p.Y) + 1;

        var sb = new StringBuilder();
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Point(x, y);

                if (points.Contains(p))
                    sb.Append('#');
                else if (edgePoints.Contains(p))
                    sb.Append('X');
                else
                    sb.Append('.');
            }
            
            sb.AppendLine();
        }

        var result = sb.ToString().TrimEnd();

        output.WriteLine(result);

        const string expected =
            """
            ..............
            .......#XXX#..
            .......X...X..
            ..#XXXX#...X..
            ..X........X..
            ..#XXXXXX#.X..
            .........X.X..
            .........#X#..
            ..............
            """;
        
        Assert.Equal(expected, result);
    }
}
