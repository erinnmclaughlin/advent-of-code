using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var points = ParseInput(input).ToHashSet();
        var shape = GridShape.CreateFromCorners(points);

        long? max = null;
        long? boundedMax = null;

        foreach (var rectangle in EnumerateRectangles(points).OrderByDescending(r => r.GetArea()))
        {
            max ??= rectangle.GetArea();

            if (shape.IsSupershapeOf(rectangle))
            {
                boundedMax = rectangle.GetArea();
                break;
            }
        }

        return (max, boundedMax);
    }
    
    public static IEnumerable<GridCell> ParseInput(string input)
    {
        return InputHelper.GetLines(input).Select(GridCell.Parse);
    }

    public static IEnumerable<GridRectangle> EnumerateRectangles(ICollection<GridCell> cells)
    {
        return cells.SelectMany(c1 => cells.Select(c2 => new GridRectangle(c1, c2)));
    }
}
