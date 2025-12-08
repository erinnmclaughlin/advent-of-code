namespace AdventOfCode.Y2025;

public sealed class Day04() : AdventDay(2025, 4)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        var grid = PaperRollGrid.Parse(lines);

        var totalCount = 0;
        int initialRemovedCount = grid.RemoveRemovableRolls();
        var lastRemovedCount = initialRemovedCount;

        while (lastRemovedCount > 0)
        {
            totalCount += lastRemovedCount;
            lastRemovedCount = grid.RemoveRemovableRolls();
        }

        return (initialRemovedCount, totalCount);
    }
}

file sealed class PaperRollGrid(List<PaperRoll> rolls)
{
    private readonly HashSet<PaperRoll> _rolls = [.. rolls];

    public bool CanRemove(PaperRoll roll)
    {
        return _rolls.Where(other => other.IsAdjacentTo(roll)).Take(4).Count() < 4;
    }

    public int RemoveRemovableRolls()
    {
        var removable = _rolls.AsParallel().Where(CanRemove).ToHashSet();
        return _rolls.RemoveWhere(removable.Contains);
    }

    public static PaperRollGrid Parse(string[] lines)
    {
        var rolls = new List<PaperRoll>();

        for (var row = 0; row < lines.Length; row++)
            for (var col = 0; col < lines[row].Length; col++)
                if (lines[row][col] == '@')
                    rolls.Add(new PaperRoll(col, row));

        return new PaperRollGrid(rolls);
    }
}

file sealed record PaperRoll(int Col, int Row)
{
    public bool IsAdjacentTo(PaperRoll other) =>
        other != this &&
        other.Col >= Col - 1 &&
        other.Col <= Col + 1 &&
        other.Row >= Row - 1 &&
        other.Row <= Row + 1;
}
