namespace AdventOfCode.Y2025;

public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var cells = ParseInput(input);
        var maxArea = cells.Max(c1 => cells.Where(c2 => c2 != c1).Max(c2 => GetArea(c1, c2)));
        return (maxArea, "");
    }

    public static HashSet<GridCell> ParseInput(string input)
    {
        return InputHelper.GetLines(input).Select(GridCell.Parse).ToHashSet();
    }

    public static long GetArea(GridCell c1, GridCell c2)
    {
        var width = Math.Abs(c1.X - c2.X) + 1L;
        var height = Math.Abs(c1.Y - c2.Y) + 1L;
        return width * height;
    }

    public static IEnumerable<GridCell> EnumerateEdges(HashSet<GridCell> cells)
    {
        for (var i = 1; i < cells.Count; i++)
        {
            var currCell = cells.ElementAt(i);
            var prevCell = cells.ElementAt(i - 1);

            foreach (var cell in currCell.EnumerateCellsUpTo(prevCell))
                yield return cell;
        }

        foreach (var cell in cells.First().EnumerateCellsUpTo(cells.Last()))
            yield return cell;
    }
    
    public sealed record GridCell(int X, int Y)
    {
        public static GridCell Parse(string input)
        {
            var parts = input.Split(',');
            return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public IEnumerable<GridCell> EnumerateCellsUpTo(GridCell other)
        {
            if (X == other.X && Y == other.Y)
            {
                yield return this;
                yield break;
            }
            
            if (X == other.X)
            {
                var minY = Math.Min(Y, other.Y);
                var maxY = Math.Max(Y, other.Y);

                for (var y = minY; y <= maxY; y++)
                    yield return this with { Y = y };

                yield break;
            }
            
            if (Y == other.Y)
            {
                var minX = Math.Min(X, other.X);
                var maxX = Math.Max(X, other.X);
                
                for (var x = minX; x <= maxX; x++)
                    yield return this with { X = x };

                yield break;
            }
            
            throw new ArgumentException("Can only enumerate cells if other cell is in same row or column as this cell.", nameof(other));
        }
    }
}
