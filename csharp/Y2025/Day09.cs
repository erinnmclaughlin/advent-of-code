using AdventOfCode.Common;

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
        var width = Math.Abs(c1.Col - c2.Col) + 1L;
        var height = Math.Abs(c1.Row - c2.Row) + 1L;
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
    
    public sealed record GridCell(int Col, int Row) : AdventCell(Row, Col)
    {
        public static GridCell Parse(string input)
        {
            var parts = input.Split(',');
            return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        public IEnumerable<GridCell> EnumerateCellsUpTo(GridCell other)
        {
            if (Col == other.Col && Row == other.Row)
            {
                yield return this;
                yield break;
            }
            
            if (Col == other.Col)
            {
                var minRow = Math.Min(Row, other.Row);
                var maxRow = Math.Max(Row, other.Row);

                for (var row = minRow; row <= maxRow; row++)
                    yield return this with { Row = row };

                yield break;
            }
            
            if (Row == other.Row)
            {
                var minCol = Math.Min(Col, other.Col);
                var maxCol = Math.Max(Col, other.Col);
                
                for (var col = minCol; col <= maxCol; col++)
                    yield return this with { Col = col };

                yield break;
            }
            
            throw new ArgumentException("Can only enumerate cells if other cell is in same row or column as this cell.", nameof(other));
        }
    }
}
