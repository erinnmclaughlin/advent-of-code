using System.Data;
using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025.Common;

public sealed class GridShapeTests(ITestOutputHelper output)
{
    [Fact]
    public void Can_create_shape_from_corners()
    {
        var c1 = new GridCell(0, 0);
        var c2 = new GridCell(5, 0);
        var c3 = new GridCell(5, 5);
        var c4 = new GridCell(0, 5);
        
        var corners = new List<GridCell> { c1, c2, c3, c4 };
        var shape = new GridShape(corners);
        output.WriteLine(shape.ToString());
        Assert.Equal(4, shape.EdgeCount);
        
        Assert.All(corners, c => Assert.True(shape.Contains(c)));
        Assert.All(corners, c => Assert.True(shape.IsOnCorner(c)));
        Assert.All(corners, c => Assert.True(shape.IsOnEdge(c)));
        
        var boundingBox = shape.BoundingBox;
        Assert.Equal(c1, boundingBox.TopLeft);
        Assert.Equal(c2, boundingBox.TopRight);
        Assert.Equal(c3, boundingBox.BottomRight);
        Assert.Equal(c4, boundingBox.BottomLeft);
    }

    [Fact]
    public void Can_get_edges_above_cell()
    {
        var shape = BuildRegularTestShape();
        Assert.NotEmpty(shape.EnumerateEdgesAbove(new GridCell(2, 2)));
    }

    [Fact]
    public void Can_get_edges_below_cell()
    {
        var shape = BuildRegularTestShape();
        Assert.NotEmpty(shape.EnumerateEdgesBelow(new GridCell(2, 2)));
    }

    [Fact]
    public void Can_get_edges_left_of_cell()
    {
        var shape = BuildRegularTestShape();
        Assert.NotEmpty(shape.EnumerateEdgesLeftOf(new GridCell(2, 2)));
    }

    [Fact]
    public void Can_get_edges_right_of_cell()
    {
        var shape = BuildRegularTestShape();
        Assert.NotEmpty(shape.EnumerateEdgesRightOf(new GridCell(2, 2)));
    }
    
    [Fact]
    public void Contains_returns_true_when_inside()
    {
        var shape = BuildRegularTestShape();
        
        var cell = new GridCell(2, 2);
        Assert.True(shape.Contains(cell));
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(5, 1)]
    [InlineData(4, 5)]
    [InlineData(0, 4)]
    public void Contains_returns_true_when_on_edge(int col, int row)
    {
        var shape = BuildRegularTestShape();
        Assert.True(shape.Contains(new GridCell(col, row)));
    }

    [Fact]
    public void Can_create_irregular_shape()
    {
        var shape = BuildIrregularTestShape();
        output.WriteLine(shape.ToString());
        Assert.Equal(6, shape.EdgeCount);
    }

    [Fact]
    public void Contains_returns_true_for_all_points_in_irregular_shape()
    {
        var shape = BuildIrregularTestShape();
        
        // First 4 rows, all 6 cells
        // Next 2 rows, cells 0-3 (4 total)

        for (var row = 0; row < 6; row++)
        {
            for (var col = 0; col < 6; col++)
            {
                if (row < 4 || col < 4)
                    Assert.True(shape.Contains(new GridCell(col, row)));
                else
                    Assert.False(shape.Contains(new GridCell(col, row)));
            }
        }
    }

    [Fact]
    public void Irregular_shape_does_not_contain_its_bounding_box()
    {
        var shape = BuildIrregularTestShape();
        Assert.False(shape.Contains(shape.BoundingBox));
    }

    [Fact]
    public void Irregular_shape_contains_inner_rectangle()
    {
        var shape = BuildIrregularTestShape();
        var r1 = new GridRectangle(new GridCell(0, 0), new GridCell(3, 3));
        Assert.True(shape.Contains(r1));
        
        var r2 = new GridRectangle(new GridCell(0, 0), new GridCell(3, 5));
        Assert.True(shape.Contains(r2));
    }

    [Fact]
    public void Irregular_shape_wicked_irregular_1()
    {
        var shape = new GridShape([
            new GridCell(5, 0),
            new GridCell(10, 0),
            new GridCell(10, 5),
            new GridCell(15, 5),
            new GridCell(15, 10),
            new GridCell(10, 10),
            new GridCell(10, 15),
            new GridCell(5, 15),
            new GridCell(5, 10),
            new GridCell(0, 10),
            new GridCell(0, 5),
            new GridCell(5, 5)
        ]);
        
        output.WriteLine(shape.ToString());
    }

    [Fact]
    public void Irregular_shape_wicked_irregular_2()
    {
        var shape = new GridShape([
            new GridCell(0, 0),
            new GridCell(20, 0),
            new GridCell(20, 20),
            new GridCell(15, 20),
            new GridCell(15, 15),
            new GridCell(5, 15),
            new GridCell(5, 20),
            new GridCell(0, 20)
        ]);
        
        output.WriteLine(shape.ToString());

        var rect = new GridRectangle(
                new GridCell(6, 18),
                new GridCell(9, 20));
        
        output.WriteLine(shape.ToString(rect));
        Assert.False(shape.Contains(rect.TopLeft));
        Assert.False(shape.Contains(rect.BottomRight));
        Assert.False(shape.Contains(rect.TopRight));
        Assert.False(shape.Contains(rect.BottomLeft));
        Assert.False(shape.Contains(rect));
    }
    
    [Fact]
    public void Irregular_shape_does_not_contain_overflowing_rectangle()
    {
        var shape = BuildIrregularTestShape();
        var rect = new GridRectangle(new GridCell(0, 0), new GridCell(4, 4));
        Assert.False(shape.Contains(rect));
    }

    private static GridShape BuildRegularTestShape()
    {
        var c1 = new GridCell(0, 0);
        var c2 = new GridCell(5, 0);
        var c3 = new GridCell(5, 5);
        var c4 = new GridCell(0, 5);
        
        var corners = new List<GridCell> { c1, c2, c3, c4 };
        return new GridShape(corners);
    }

    private static GridShape BuildIrregularTestShape()
    {
        var c1 = new GridCell(0, 0);
        var c2 = new GridCell(5, 0);
        var c3 = new GridCell(5, 3);
        var c4 = new GridCell(3, 3);
        var c5 = new GridCell(3, 5);
        var c6 = new GridCell(0, 5);
        
        var corners = new List<GridCell> { c1, c2, c3, c4, c5, c6 };
        return new GridShape(corners);
    }
}