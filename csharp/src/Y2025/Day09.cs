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
        foreach (var line in InputHelper.GetLines(input))
        {
            var parts = line.Split(',');
            yield return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }

    public static IEnumerable<GridRectangle> EnumerateRectangles(ICollection<GridCell> cells)
    {
        return cells.SelectMany(c1 => cells.Select(c2 => new GridRectangle(c1, c2)));
    }
}
