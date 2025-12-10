using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var cells = ParseInput(input).ToArray();
        var rectangles = cells.SelectMany(c1 => cells.Select(c2 => BuildRectangle(c1, c2)));
        var maxArea = rectangles.Max(r => r.GetArea());
        return (maxArea, "");
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
