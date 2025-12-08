namespace AdventOfCode.Y2025;

public sealed class Day07() : AdventDay(2025, 7)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        return (SolvePartOne(lines), SolvePartTwo(lines));
    }

    private static long SolvePartOne(string[] lines)
    {
        var currentPositions = new HashSet<int> { lines[0].IndexOf('S') };
        var nextPositions = new HashSet<int>();
        var splitCount = 0;

        for (var i = 1; i < lines.Length; i++)
        {
            var line = lines[i];

            foreach (var pos in currentPositions)
            {
                if (line[pos] == '^')
                {
                    nextPositions.Add(pos - 1);
                    nextPositions.Add(pos + 1);
                    splitCount++;
                }
                else
                {
                    nextPositions.Add(pos);
                }
            }

            currentPositions = nextPositions;
            nextPositions = [];
        }

        return splitCount;
    }

    private static long SolvePartTwo(string[] lines)
    {
        return CountPossiblePaths(lines[0].IndexOf('S'), 1, lines, []);
    }

    private static long CountPossiblePaths(int col, int row, string[] map, Dictionary<(int Col, int Row), long> cache)
    {
        if (row >= map.Length - 1)
            return 1;

        if (cache.TryGetValue((col, row), out var value))
            return value;

        long result;

        if (map[row][col] == '^')
        {
            var left = CountPossiblePaths(col - 1, row + 1, map, cache);
            var right = CountPossiblePaths(col + 1, row + 1, map, cache);
            result = left + right;
        }
        else
        {
            result = CountPossiblePaths(col, row + 1, map, cache);
        }

        cache.Add((col, row), result);
        return result;
    }
}
