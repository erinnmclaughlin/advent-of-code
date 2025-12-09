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
    public void GetAreaTests(string c1, string c2, long expected)
    {
        var cell1 = Day09.GridCell.Parse(c1);
        var cell2 = Day09.GridCell.Parse(c2);
        var area = Day09.GetArea(cell1, cell2);
        Assert.Equal(expected, area);
    }

    [Fact]
    public void EnumerateEdgesWorks()
    {
        var cells = Day09.ParseInput(Sample);
        var edgeCells = Day09.EnumerateEdges(cells).ToHashSet();

        var minX = cells.Min(c => c.Col) - 2;
        var maxX = cells.Max(c => c.Col) + 2;
        var minY = cells.Min(c => c.Row) - 1;
        var maxY = cells.Max(c => c.Row) + 1;

        var sb = new StringBuilder();
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var c = new Day09.GridCell(x, y);

                if (cells.Contains(c))
                    sb.Append('#');
                else if (edgeCells.Contains(c))
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
