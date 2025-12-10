using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var points = ParseInput(input).ToArray();
        var boundaries = EnumerateBoundaries(points).ToArray();

        long? max = null;
        long? boundedMax = null;

        foreach (var rectangle in EnumerateOrderedRectangles(points))
        {
            max ??= rectangle.GetArea();

            if (boundaries.All(s => !s.IsCollidingWith(rectangle)))
            {
                boundedMax = rectangle.GetArea();
                break;
            }
        }

        return (max, boundedMax);
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
    
    public static IOrderedEnumerable<GridRectangle> EnumerateOrderedRectangles(GridCell[] cells) => cells
        .SelectMany(c1 => cells.Select(c2 => BuildRectangle(c1, c2)))
        .OrderByDescending(r => r.GetArea());

    public static IEnumerable<GridRectangle> EnumerateBoundaries(GridCell[] corners) => corners
        .Zip(corners.Prepend(corners.Last()))
        .Select(p => BuildRectangle(p.First, p.Second));
}
