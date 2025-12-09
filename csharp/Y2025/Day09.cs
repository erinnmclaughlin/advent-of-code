using System.Drawing;

namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var points = InputHelper.GetLines(input).Select(ParseLine).ToHashSet();
        var maxArea = points.Max(p1 => points.Where(p2 => p2 != p1).Max(p2 => GetArea(p1, p2)));
        return (maxArea, "");
    }

    public static long GetArea(Point p1, Point p2)
    {
        var width = Math.Abs(p1.X - p2.X) + 1L;
        var height = Math.Abs(p1.Y - p2.Y) + 1L;
        return width * height;
    }

    public static Point ParseLine(string line)
    {
        var parts = line.Split(',');
        return new Point(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public static IEnumerable<Point> EnumerateEdges(HashSet<Point> points)
    {
        for (var i = 1; i < points.Count; i++)
        {
            var currPoint = points.ElementAt(i);
            var prevPoint = points.ElementAt(i - 1);

            foreach (var point in EnumeratePointsBetween(currPoint, prevPoint))
                yield return point;
        }

        foreach (var point in EnumeratePointsBetween(points.First(), points.Last()))
        {
            yield return point;
        }
    }
    
    public static IEnumerable<Point> EnumeratePointsBetween(Point p1, Point p2)
    {
        if (p1.X == p2.X)
        {
            var minY = Math.Min(p1.Y, p2.Y);
            var maxY = Math.Max(p1.Y, p2.Y);

            for (var y = minY; y <= maxY; y++)
                yield return p1 with { Y = y };
        }
        else
        {
            var minX = Math.Min(p1.X, p2.X);
            var maxX = Math.Max(p1.X, p2.X);
                
            for (var x = minX; x <= maxX; x++)
                yield return p1 with { X = x };
        }
    }
}
