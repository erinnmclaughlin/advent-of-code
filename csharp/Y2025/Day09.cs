using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Y2025;

// 4634026886 is too high
public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var cornerPoints = ParseInput(input).ToList();
        var shape = new GridShape(cornerPoints);
        
        var rectangles = cornerPoints.SelectMany(c1 => cornerPoints.Where(c2 => c1 != c2).Select(c2 => BuildRectangle(c1, c2)));
        
        var maxArea = 0L;
        var maxBoundedArea = 0L;

        foreach (var rectangle in rectangles)
        {
            var area = rectangle.GetArea();

            if (area > maxArea) 
                maxArea = area;
            
            if (area > maxBoundedArea && shape.Contains(rectangle))
                maxBoundedArea = area;
        }
        
        return (maxArea, maxBoundedArea);
    }
    
    public static GridRectangle BuildRectangle(GridCell cell1, GridCell cell2)
    {
        var topLeft = new GridCell(Math.Min(cell1.Col, cell2.Col), Math.Min(cell1.Row, cell2.Row));
        var bottomRight = new GridCell(Math.Max(cell1.Col, cell2.Col), Math.Max(cell1.Row, cell2.Row));
        return new GridRectangle(topLeft, bottomRight);
    }
    
    public static IEnumerable<GridCell> ParseInput(string input)
    {
        return InputHelper.GetLines(input).Select(GridCell.Parse);
    }
}
