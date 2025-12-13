using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025.Common;

public sealed class GridRectangleTests
{
    [Fact]
    public void Can_create_rectangle()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        
        Assert.Equal(topLeft, rectangle.TopLeft);
        Assert.Equal(bottomRight, rectangle.BottomRight);
        Assert.Equal(new GridCell(5, 0), rectangle.TopRight);
        Assert.Equal(new GridCell(0, 5), rectangle.BottomLeft);
        
        Assert.Equal(6, rectangle.Width);
        Assert.Equal(6, rectangle.Height);
    }

    [Fact]
    public void Can_get_area()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        Assert.Equal(36, rectangle.GetArea());
    }

    [Fact]
    public void Contains_cell_returns_true_when_inside()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        
        Assert.True(rectangle.Contains(new GridCell(2, 2)));
    }
    
    [Fact]
    public void Contains_cell_returns_false_when_outside()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        
        Assert.False(rectangle.Contains(new GridCell(6, 6)));
    }

    [Fact]
    public void Contains_cell_returns_true_when_on_edge()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        
        Assert.True(rectangle.Contains(new GridCell(0, 0)));
        Assert.True(rectangle.Contains(new GridCell(0, 5)));
        Assert.True(rectangle.Contains(new GridCell(5, 2)));
    }
    
    [Fact]
    public void Contains_rectangle_returns_true_when_inside()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        var inside = new GridRectangle(new GridCell(2, 2), new GridCell(3, 3));
        Assert.True(rectangle.IsSupershapeOf(inside));
    }

    [Fact]
    public void Contains_rectangle_returns_true_when_equal()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle1 = new GridRectangle(topLeft, bottomRight);
        var rectangle2 = new GridRectangle(topLeft, bottomRight);
        Assert.True(rectangle1.IsSupershapeOf(rectangle2));
        Assert.True(rectangle2.IsSupershapeOf(rectangle1));
    }
    
    [Fact]
    public void Contains_rectangle_returns_false_when_outside()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        var outside = new GridRectangle(new GridCell(6, 6), new GridCell(7, 7));
        Assert.False(rectangle.IsSupershapeOf(outside));
    }

    [Fact]
    public void Contains_rectangle_returns_false_when_rectangle_is_contained_by_other_and_not_equal()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        var inside = new GridRectangle(new GridCell(1, 1), new GridCell(4, 4));
        Assert.False(inside.IsSupershapeOf(rectangle));
    }

    [Fact]
    public void Is_on_edge_returns_true_when_on_edge()
    {
        var topLeft = new GridCell(0, 0);
        var bottomRight = new GridCell(5, 5);
        var rectangle = new GridRectangle(topLeft, bottomRight);
        Assert.True(rectangle.IsOnEdge(new GridCell(0, 0)));
        Assert.True(rectangle.IsOnEdge(new GridCell(0, 5)));
        Assert.True(rectangle.IsOnEdge(new GridCell(5, 2)));
    }
}