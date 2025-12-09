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

    public static IEnumerable<IGridEdge> EnumerateEdges(HashSet<GridCell> cells)
    {
        if (cells.Count < 2)
            throw new ArgumentException("Must have at least two grid cells.", nameof(cells));

        for (var i = 1; i < cells.Count; i++)
        {
            yield return GetEdge(cells.ElementAt(i), cells.ElementAt(i - 1));
        }

        yield return GetEdge(cells.Last(), cells.First());
    }

    public static IGridEdge GetEdge(GridCell currCell, GridCell prevCell)
    {
        if (currCell.Col == prevCell.Col)
        {
            var startRow = Math.Min(currCell.Row, prevCell.Row);
            var endRow = Math.Max(currCell.Row, prevCell.Row);

            var size = endRow - startRow + 1;
            return new GridCol(new GridCell(currCell.Col, startRow), size);
        }
        
        if (currCell.Row == prevCell.Row)
        {
            var startCol = Math.Min(currCell.Col, prevCell.Col);
            var endCol = Math.Max(currCell.Col, prevCell.Col);
            var size = endCol - startCol + 1;
            return new GridRow(new GridCell(startCol, currCell.Row), size);
        }

        throw new InvalidOperationException();
    }

    public interface IGridEdge
    {
        bool Contains(GridCell cell);
        bool Intersects(IGridEdge other);
    }

    public sealed record GridCol(GridCell Start, int Size) : IGridEdge
    {
        public GridCell End => new(Col: Start.Col, Row: Start.Row + Size - 1);
        
        public bool Contains(GridCell cell)
        {
            if (Start == cell) return true;
            if (Start.Col != cell.Col) return false;
            return cell.Row >= Start.Row && cell.Row <= (Start.Row + Size - 1);
        }

        public bool Intersects(IGridEdge other)
        {
            return EnumerateCells().Any(other.Contains);
        }

        public IEnumerable<GridCell> EnumerateCells()
        {
            for (var i = 0; i < Size; i++)
                yield return new GridCell(Col: Start.Col, Row: Start.Row + i);
        }
    }

    public sealed record GridRow(GridCell Start, int Size) : IGridEdge
    {
        public GridCell End => new(Col: Start.Col + Size - 1, Row: Start.Row);
        
        public bool Contains(GridCell cell)
        {
            if (Start == cell) return true;
            if (Start.Row != cell.Row) return false;
            return cell.Col >= Start.Col && cell.Col <= (Start.Col + Size - 1);
        }
        
        public bool Intersects(IGridEdge other)
        {
            return EnumerateCells().Any(other.Contains);
        }

        public IEnumerable<GridCell> EnumerateCells()
        {
            for (var i = 0; i < Size; i++)
                yield return new GridCell(Col: Start.Col + i, Row: Start.Row);
        }
    }
    
    public sealed record GridCell(int Col, int Row) : AdventCell(Row, Col)
    {
        public static GridCell Parse(string input)
        {
            var parts = input.Split(',');
            return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
        }
    }
}
