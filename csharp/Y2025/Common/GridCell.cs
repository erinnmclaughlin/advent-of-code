using AdventOfCode.Common;

namespace AdventOfCode.Y2025.Common;

public sealed record GridCell(int Col, int Row) : AdventCell(Row, Col)
{
    public static GridCell Parse(string input)
    {
        var parts = input.Split(',');
        return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
    }
}