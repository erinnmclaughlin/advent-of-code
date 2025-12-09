namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var points = InputHelper.GetLines(input).Select(ParseLine).ToArray();
        var maxArea = points.Max(p1 => points.Where(p2 => p2 != p1).Max(p2 => GetArea(p1, p2)));
        return (maxArea, "");
    }

    public static long GetArea((int X, int Y) p1, (int X, int Y) p2)
    {
        var width = Math.Abs(p1.X - p2.X) + 1L;
        var height = Math.Abs(p1.Y - p2.Y) + 1L;
        return width * height;
    }

    public static (int X, int Y) ParseLine(string line)
    {
        var parts = line.Split(',');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }
}
