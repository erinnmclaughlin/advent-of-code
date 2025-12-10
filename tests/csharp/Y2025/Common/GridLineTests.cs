using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025.Common;

public sealed class GridLineTests
{
    [Fact]
    public void Can_create_vertical_line()
    {
        var start = new GridCell(0, 0);
        var end = new GridCell(0, 5);
        var line = GridLine.CreateVertical(start, end);
        
        Assert.True(line.IsVertical);
        Assert.Equal(start, line.Start);
        Assert.Equal(end, line.End);
        Assert.True(line.Contains(start));
        Assert.True(line.Contains(end));
    }
    
    [Fact]
    public void Can_create_horizontal_line()
    {
        var start = new GridCell(0, 0);
        var end = new GridCell(5, 0);
        var line = GridLine.CreateHorizontal(start, end);
        
        Assert.True(line.IsHorizontal);
        Assert.Equal(start, line.Start);
        Assert.Equal(end, line.End);
        Assert.True(line.Contains(start));
        Assert.True(line.Contains(end));
    }

    [Fact]
    public void Can_create_straight_line_with_vertical_cells()
    {
        var start = new GridCell(0, 0);
        var end = new GridCell(0, 5);
        var line = GridLine.CreateStraight(start, end);
        var vLine = GridLine.CreateVertical(start, end);
        Assert.Equivalent(vLine, line);
    }
    
    [Fact]
    public void Can_create_straight_line_with_horizontal_cells()
    {
        var start = new GridCell(0, 0);
        var end = new GridCell(5, 0);
        var line = GridLine.CreateStraight(start, end);
        var hLine = GridLine.CreateHorizontal(start, end);
        Assert.Equivalent(hLine, line);
    }
}