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
    public void GetEdgeWorks()
    {
        var cell1 = new Day09.GridCell(Col: 7, Row: 1);
        var cell2 = new Day09.GridCell(Col: 7, Row: 3);
        
        var edge = Assert.IsType<Day09.GridCol>(Day09.GetEdge(cell1, cell2));
        Assert.Equal(3, edge.Size);
        Assert.True(edge.Contains(cell1));
        Assert.True(edge.Contains(cell2));
        
    }

    [Fact]
    public void TestRow()
    {
        var row = new Day09.GridRow(new Day09.GridCell(Col: 7, Row: 1), 5);
        Assert.Equal(5, row.Size);
        Assert.Equal(1, row.Start.Row);
        Assert.Equal(1, row.End.Row);
        Assert.Equal(11, row.End.Col);
    }

    [Fact]
    public void TestCol()
    {
        var col = new Day09.GridCol(new Day09.GridCell(Col: 7, Row: 1), 3);
        Assert.Equal(3, col.Size);
        Assert.Equal(3, col.End.Row);
        Assert.Equal(1, col.Start.Row);
        Assert.Equal(7, col.Start.Col);
        Assert.Equal(7, col.End.Col);

    }

    [Fact]
    public void EnumerateEdgesWorks()
    {
        var cells = Day09.ParseInput(Sample);
        var edges = Day09.EnumerateEdges(cells).ToHashSet();

        var minCol = cells.Min(c => c.Col) - 2;
        var maxCol = cells.Max(c => c.Col) + 2;
        var minRow = cells.Min(c => c.Row) - 1;
        var maxRow = cells.Max(c => c.Row) + 1;

        var sb = new StringBuilder();
        for (var row = minRow; row <= maxRow; row++)
        {
            for (var col = minCol; col <= maxCol; col++)
            {
                var c = new Day09.GridCell(col, row);

                if (cells.Contains(c))
                    sb.Append('#');
                else if (edges.Any(e => e.Contains(c)))
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
