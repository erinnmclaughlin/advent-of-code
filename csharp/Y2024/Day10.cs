namespace AdventOfCode.Y2024;

using Coord = (int Row, int Col);

public sealed class Day10() : AdventDay(2024, 10)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        return (SolvePartOne(lines), SolvePartTwo(lines));
    }

    private static long SolvePartOne(string[] lines) => lines
        .EnumerateStartingCoords()
        .SelectMany(x => lines.EnumeratePathEnds(x).Distinct())
        .Count();

    private static long SolvePartTwo(string[] lines) => lines
        .EnumerateStartingCoords()
        .SelectMany(lines.EnumeratePathEnds)
        .Count();
}

file static class Extensions
{
    public static IEnumerable<Coord> EnumerateStartingCoords(this string[] lines)
    {
        for (var r = 0; r < lines.Length; r++)
        for (var c = 0; c < lines[r].Length; c++)
            if (lines[r][c] == '0')
                yield return new Coord(r, c);
    }
    
    public static IEnumerable<Coord> EnumeratePathEnds(this string[] lines, Coord pos)
    {
        if (lines[pos.Row][pos.Col] == '9')
            yield return pos;

        foreach (var next in EnumerateNextSteps(lines, pos))
        foreach (var nextEnd in EnumeratePathEnds(lines, next))
            yield return nextEnd;
    }

    public static IEnumerable<Coord> EnumerateNextSteps(string[] map, Coord pos)
    {
        var nextValue = map[pos.Row][pos.Col] + 1;
        
        if (pos.Row > 0 && map[pos.Row - 1][pos.Col] == nextValue)
            yield return new Coord(pos.Row - 1, pos.Col);
        
        if (pos.Row < map.Length - 1 && map[pos.Row + 1][pos.Col] == nextValue)
            yield return new Coord(pos.Row + 1, pos.Col);

        if (pos.Col > 0 && map[pos.Row][pos.Col - 1] == nextValue)
            yield return new Coord(pos.Row, pos.Col - 1);

        if (pos.Col < map[0].Length - 1 && map[pos.Row][pos.Col + 1] == nextValue)
            yield return new Coord(pos.Row, pos.Col + 1);
    }
}
