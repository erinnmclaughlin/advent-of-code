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
        var p2 = Day09.SolvePartTwo(Sample).ToString();
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
        var cell3 = new Day09.GridCell(Col: 11, Row: 1);
        
        var col = Day09.GetCol(cell1, cell2);
        Assert.Equal(3, col.Size);
        Assert.True(col.Contains(cell1));
        Assert.True(col.Contains(cell2));
        Assert.False(col.Contains(new Day09.GridCell(Col: 7, Row: 0)));
        Assert.False(col.Contains(new Day09.GridCell(Col: 7, Row: 4)));

        
        var row = Day09.GetRow(cell1, cell3);
        Assert.Equal(5, row.Size);
        Assert.True(row.Contains(cell1));
        Assert.True(row.Contains(cell3));
        Assert.False(row.Contains(new Day09.GridCell(Col: 6, Row: 1)));
        Assert.False(row.Contains(new Day09.GridCell(Col: 12, Row: 1)));
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
    public void GetFullShapeWorks1()
    {
        var cells = Day09.ParseInput(Sample);
        var (rows, cols) = Day09.EnumerateEdges(cells);
        var shape = new Day09.GridShape(rows, cols);
        
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
                
                if (shape.Contains(c))
                    sb.Append('X');
                else
                    sb.Append('.');
            }
            
            sb.AppendLine();
        }

        var result = sb.ToString().TrimEnd();

        output.WriteLine(result);
    }
    
    [Fact]
    public void GetFullShapeWorks2()
    {
        var cells = Day09.ParseInput(Sample);
        var (rows, cols) = Day09.EnumerateEdges(cells);
        var shape = new Day09.GridShape(rows, cols);
        
        var minCol = cells.Min(c => c.Col) - 2;
        var maxCol = cells.Max(c => c.Col) + 2;
        var minRow = cells.Min(c => c.Row) - 1;
        var maxRow = cells.Max(c => c.Row) + 1;

        var sb = new List<List<char>>();
        for (var row = minRow; row <= maxRow; row++)
        {
            var sbRow = new List<char>();
            
            for (var col = minCol; col <= maxCol; col++)
            {
                sbRow.Add('.');
            }
            
            sb.Add(sbRow);
        }

        foreach (var cell in shape.EnumerateCells())
        {
            sb[cell.Row][cell.Col] = 'X';
        }

        foreach (var row in sb)
        {
            output.WriteLine(string.Join("", row));
        }
    }

    [Fact]
    public void EnumerateEdgesWorks()
    {
        var cells = Day09.ParseInput(Sample);
        var (rows, cols) = Day09.EnumerateEdges(cells);

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
                {
                    Assert.True(rows.Any(r1 => r1.Contains(c) || cols.Any(c1 => c1.Contains(c))));
                    sb.Append('#');
                }
                else if (rows.Any(r1 => r1.Contains(c) || cols.Any(c1 => c1.Contains(c))))
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

    [Fact]
    public void IntersectsWorks()
    {
        var cell1 = new Day09.GridCell(Col: 7, Row: 1);
        var cell2 = new Day09.GridCell(Col: 7, Row: 3);
        var cell3 = new Day09.GridCell(Col: 11, Row: 1);
        
        var col = Assert.IsType<Day09.GridCol>(Day09.GetCol(cell1, cell2));
        var row = Assert.IsType<Day09.GridRow>(Day09.GetRow(cell1, cell3));
        
        Assert.True(col.Intersects(row));
        Assert.True(row.Intersects(col));
    }

    [Fact]
    public void GetShapeWorks()
    {
        var topLeft = new Day09.GridCell(Col: 7, Row: 1);
        var bottomRight = new Day09.GridCell(Col: 11, Row: 7);
        var topRight = new Day09.GridCell(Col: 11, Row: 1);
        var bottomLeft = new Day09.GridCell(Col: 7, Row: 7);
        
        var shape = Day09.GetShape(topLeft, bottomRight);
        
        Assert.True(shape.Contains(topLeft));
        Assert.True(shape.Contains(bottomRight));
        Assert.True(shape.Contains(topRight));
        Assert.True(shape.Contains(bottomLeft));
        
        Assert.False(shape.Contains(new Day09.GridCell(Col: topLeft.Col - 1, topLeft.Row)));
        Assert.False(shape.Contains(new Day09.GridCell(Col: topLeft.Col, topLeft.Row - 1)));
        
        Assert.False(shape.Contains(new Day09.GridCell(Col: topRight.Col + 1, topLeft.Row)));
        Assert.False(shape.Contains(new Day09.GridCell(Col: topRight.Col, topLeft.Row - 1)));
        
        Assert.False(shape.Contains(new Day09.GridCell(Col: bottomLeft.Col - 1, bottomLeft.Row)));
        Assert.False(shape.Contains(new Day09.GridCell(Col: bottomLeft.Col, bottomLeft.Row + 1)));
        
        Assert.False(shape.Contains(new Day09.GridCell(Col: bottomRight.Col + 1, bottomRight.Row)));
        Assert.False(shape.Contains(new Day09.GridCell(Col: bottomRight.Col, bottomRight.Row + 1)));
    }

    [Fact]
    public void CanCreateRow()
    {
        var left = new Day09.GridCell(Col: 7, Row: 1);
        var right = new Day09.GridCell(Col: 11, Row: 1);

        var row = new Day09.GridRow(left, 5);
        Assert.True(row.Contains(left));
        Assert.True(row.Contains(right));
        Assert.False(row.Contains(new Day09.GridCell(Col: 6, Row: 1)));
        Assert.False(row.Contains(new Day09.GridCell(Col: 12, Row: 1)));
        Assert.False(row.Contains(new Day09.GridCell(Col: 7, Row: 0)));
        Assert.Equal(left, row.Start);
        Assert.Equal(right, row.End);
    }

    [Fact]
    public void CanCreateCol()
    {
        var top = new Day09.GridCell(Col: 7, Row: 1);
        var bottom = new Day09.GridCell(Col: 7, Row: 7);

        var col = new Day09.GridCol(top, 7);
        Assert.True(col.Contains(top));
        Assert.True(col.Contains(bottom));
        Assert.False(col.Contains(new Day09.GridCell(Col: 7, Row: 0)));
        Assert.False(col.Contains(new Day09.GridCell(Col: 7, Row: 8)));
    }

    [Fact]
    public void GetTopLeftBottomRightWorks()
    {
        var topLeft = new Day09.GridCell(Col: 7, Row: 1);
        var bottomRight = new Day09.GridCell(Col: 11, Row: 7);
        var topRight = new Day09.GridCell(Col: 11, Row: 1);
        var bottomLeft = new Day09.GridCell(Col: 7, Row: 7);
        
        var tlbr = Day09.GetTopLeftBottomRight(topLeft, bottomRight);

        var (tl, br) = tlbr;
        Assert.Equal(tl, topLeft);
        Assert.Equal(br, bottomRight);
        
        Assert.Equivalent(tlbr, Day09.GetTopLeftBottomRight(bottomRight, topLeft));
        Assert.Equivalent(tlbr, Day09.GetTopLeftBottomRight(topRight, bottomLeft));
        Assert.Equivalent(tlbr, Day09.GetTopLeftBottomRight(bottomLeft, topRight));
    }
}
