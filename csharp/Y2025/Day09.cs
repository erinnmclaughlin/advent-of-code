using System.Collections.Immutable;
using AdventOfCode.Common;

namespace AdventOfCode.Y2025;

// 94006416 is too low
// 1963592680 is too high
public sealed class Day09() : AdventDay(2025, 9)
{
    public override AdventDaySolution Solve(string input)
    {
        var cells = ParseInput(input);
        var maxArea = cells.Max(c1 => cells.Where(c2 => c2 != c1).Max(c2 => GetArea(c1, c2)));
        return (maxArea, SolvePartTwo(input));
    }
    
    public static long SolvePartTwo(string input)
    {
        var cells = ParseInput(input);
        var (rows, cols) = EnumerateEdges(cells);
        var shape = new GridShape(rows, cols);

        var maxContainedArea = 0L;

        foreach (var c1 in cells)
        {
            foreach (var c2 in cells.Where(c2 => c1 != c2))
            {
                var area = GetArea(c1, c2);

                if (maxContainedArea >= area)
                    continue;

                var subShape = GetShape(c1, c2);
                if (shape.Contains(subShape))
                {
                    maxContainedArea = area;
                }
            }
        }
        
        return maxContainedArea;
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

    public static (HashSet<GridRow>, HashSet<GridCol>) EnumerateEdges(HashSet<GridCell> cells)
    {
        if (cells.Count < 2)
            throw new ArgumentException("Must have at least two grid cells.", nameof(cells));

        var rows = new HashSet<GridRow>();
        var cols = new HashSet<GridCol>();

        for (var i = 1; i < cells.Count; i++)
        {
            var c1 = cells.ElementAt(i);
            var c2 = cells.ElementAt(i - 1);

            if (c1.Row == c2.Row)
                rows.Add(GetRow(c1, c2));
            else
                cols.Add(GetCol(c1, c2));
        }

        var first = cells.First();
        var last = cells.Last();
        if (first.Row == last.Row)
            rows.Add(GetRow(first, last));
        else
            cols.Add(GetCol(first, last));

        return (rows, cols);
    }

    public static GridRow GetRow(GridCell cell1, GridCell cell2)
    {
        if (cell1.Row != cell2.Row)
            throw new InvalidOperationException();
        
        var start = cell1.Col < cell2.Col ? cell1 : cell2;
        return new GridRow(start, GetWidth(cell1, cell2));
    }

    public static GridCol GetCol(GridCell cell1, GridCell cell2)
    {
        if (cell1.Col != cell2.Col)
            throw new InvalidOperationException();
        
        var start = cell1.Row < cell2.Row ? cell1 : cell2;
        return new GridCol(start, GetHeight(cell1, cell2));
    }
    
    public static GridRectangle GetShape(GridCell cell1, GridCell cell2)
    {
        var (topLeft, bottomRight) = GetTopLeftBottomRight(cell1, cell2);
        return new GridRectangle(topLeft, bottomRight);
    }

    public static int GetWidth(GridCell cell1, GridCell cell2)
    {
        return Math.Abs(cell1.Col - cell2.Col) + 1;
    }

    public static int GetHeight(GridCell cell1, GridCell cell2)
    {
        return Math.Abs(cell1.Row - cell2.Row) + 1;
    }
    
    public static (GridCell TopLeft, GridCell BottomRight) GetTopLeftBottomRight(GridCell cell1, GridCell cell2)
    {
        var minRow = Math.Min(cell1.Row, cell2.Row);
        var maxRow = Math.Max(cell1.Row, cell2.Row);
        var minCol = Math.Min(cell1.Col, cell2.Col);
        var maxCol = Math.Max(cell1.Col, cell2.Col);

        var topLeft = new GridCell(Col: minCol, Row: minRow);
        var bottomRight = new GridCell(Col: maxCol, Row: maxRow);

        return (topLeft, bottomRight);
    }

    public interface IGridEdge
    {
        GridCell Start { get; }
        GridCell End { get; }
        bool Contains(GridCell cell);
        IEnumerable<GridCell> EnumerateCells();
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

        public bool Intersects(GridRow other)
        {
            var point = new GridCell(Start.Col, other.Start.Row);
            return Contains(point) && other.Contains(point);
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
        
        public bool Intersects(GridCol other)
        {
            var point = new GridCell(other.Start.Col, Start.Row);
            return Contains(point) && other.Contains(point);
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

    public sealed class GridRectangle : GridShape
    {
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public int Left { get; }

        public GridRow TopRow => Rows.ElementAt(0);
        public GridRow BottomRow => Rows.ElementAt(1);
        public GridCol LeftCol => Cols.ElementAt(0);
        public GridCol RightCol => Cols.ElementAt(1);
        
        public GridRectangle(GridCell topLeft, GridCell bottomRight) : base(GetRows(topLeft, bottomRight), GetCols(topLeft, bottomRight))
        {
            Top = topLeft.Row;
            Right = bottomRight.Col;
            Bottom = bottomRight.Row;
            Left = topLeft.Col;
        }

        public override bool Contains(GridCell cell)
        {
            return cell.Row >= Top && cell.Row <= Bottom && cell.Col >= Left && cell.Col <= Right;
        }

        public override bool Contains(GridRow row)
        {
            return Contains(row.Start) && Contains(row.End);
        }
        
        public override bool Contains(GridCol col)
        {
            return Contains(col.Start) && Contains(col.End);
        }

        private static HashSet<GridRow> GetRows(GridCell topLeft, GridCell bottomRight)
        {
            var width = GetWidth(topLeft, bottomRight);
            var topRow = new GridRow(topLeft, width);
            var bottomRow = new GridRow(new GridCell(Col: topLeft.Col, Row: bottomRight.Row), width);
            return [topRow, bottomRow];
        }

        private static HashSet<GridCol> GetCols(GridCell topLeft, GridCell bottomRight)
        {
            var height = GetHeight(topLeft, bottomRight);
            var leftCol = new GridCol(topLeft, height);
            var rightCol = new GridCol(new GridCell(Col: bottomRight.Col, Row: topLeft.Row), height);
            return [leftCol, rightCol];
        }
    }

    public class GridShape
    {
        protected ImmutableHashSet<GridRow> Rows { get; }
        protected ImmutableHashSet<GridCol> Cols { get; }

        public GridShape(HashSet<GridRow> rows, HashSet<GridCol> cols)
        {
            Rows = rows.ToImmutableHashSet();
            Cols = cols.ToImmutableHashSet();
        }

        public virtual bool Contains(GridShape shape)
        {
            return shape.Rows.All(Contains) && shape.Cols.All(Contains);
        }

        public virtual bool Contains(GridRow row)
        {
            if (!Contains(row.Start))
                return false;

            return !Cols.Any(c => c.Start.Row != row.Start.Row && c.End.Row != row.End.Row && c.Intersects(row));
        }
        
        public virtual bool Contains(GridCol col)
        {
            if (!Contains(col.Start))
                return false;

            return !Rows.Any(r => r.Start.Col != col.Start.Col && r.End.Col != col.End.Col && r.Intersects(col));
        }
        
        public virtual bool Contains(GridCell cell)
        {
            return HasRowAbove(cell) && HasRowBelow(cell) && HasColumnLeft(cell) && HasColumnRight(cell);
        }

        public IEnumerable<GridCell> EnumerateCells()
        {
            return Rows.SelectMany(row => row.EnumerateCells()).Concat(Cols.SelectMany(col => col.EnumerateCells()));
        }

        private bool HasRowAbove(GridCell cell)
        {
            return Rows.Any(r =>
                // above
                r.Start.Row <= cell.Row &&
                // covers
                r.Start.Col <= cell.Col && r.End.Col >= cell.Col);
        }

        private bool HasRowBelow(GridCell cell)
        {
            return Rows.Any(r =>
                // below
                r.Start.Row >= cell.Row &&
                // covers
                r.Start.Col <= cell.Col && r.End.Col >= cell.Col);
        }

        private bool HasColumnLeft(GridCell cell)
        {
            return Cols
                .Any(c =>
                    // left
                    c.Start.Col <= cell.Col &&
                    // covers
                    c.Start.Row <= cell.Row && c.End.Row >= cell.Row);
        }

        private bool HasColumnRight(GridCell cell)
        {
            return Cols
                .Any(c =>
                    // right
                    c.Start.Col >= cell.Col &&
                    // covers
                    c.Start.Row <= cell.Row && c.End.Row >= cell.Row);
        }
    }
}
