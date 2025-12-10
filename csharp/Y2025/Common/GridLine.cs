namespace AdventOfCode.Y2025.Common;

public sealed record GridLine
{
    public GridCell Start { get; }
    public GridCell End { get; }
    
    public bool IsHorizontal => Start.Row == End.Row;
    public bool IsVertical => Start.Col == End.Col;
    
    private GridLine(GridCell start, GridCell end)
    {
        Start = start;
        End = end;
    }

    public IEnumerable<GridCell> EnumerateCells()
    {
        if (IsHorizontal)
        {
            var minCol = Math.Min(Start.Col, End.Col);
            var maxCol = Math.Max(Start.Col, End.Col);
            
            for (var col = minCol; col <= maxCol; col++)
                yield return new GridCell(col, Start.Row);

            yield break;
        }

        if (IsVertical)
        {
            var minRow = Math.Min(Start.Row, End.Row);
            var maxRow = Math.Max(Start.Row, End.Row);
            
            for (var row = minRow; row <= maxRow; row++)
                yield return new GridCell(Start.Col, row);
            
            yield break;
        }
        
        throw new NotImplementedException();
    }

    public bool Contains(GridCell cell)
    {
        if (IsHorizontal)
        {
            var minCol = Math.Min(Start.Col, End.Col);
            var maxCol = Math.Max(Start.Col, End.Col);
            return cell.Row == Start.Row && cell.Col >= minCol && cell.Col <= maxCol;
        }

        if (IsVertical)
        {
            var minRow = Math.Min(Start.Row, End.Row);
            var maxRow = Math.Max(Start.Row, End.Row);
            return cell.Col == Start.Col && cell.Row >= minRow && cell.Row <= maxRow;
        }
        
        throw new NotImplementedException();
    }

    public bool Contains(GridLine other)
    {
        return Contains(other.Start) && Contains(other.End);
    }

    public static GridLine CreateStraight(GridCell cell1, GridCell cell2)
    {
        if (cell1.Col == cell2.Col)
            return CreateVertical(cell1, cell2);
        
        if (cell1.Row == cell2.Row)
            return CreateHorizontal(cell1, cell2);
        
        throw new ArgumentException($"Cannot create straight line between {cell1} and {cell2}.");
    }

    public static GridLine CreateVertical(GridCell cell1, GridCell cell2)
    {
        if (cell1.Col != cell2.Col)
            throw new ArgumentException();

        //if (cell1.Row > cell2.Row)
        //    return new GridLine(cell2, cell1);
        
        return new GridLine(cell1, cell2);
    }

    public static GridLine CreateHorizontal(GridCell cell1, GridCell cell2)
    {
        if (cell1.Row != cell2.Row)
            throw new ArgumentException();

        //if (cell1.Col > cell2.Col)
        //    return new GridLine(cell2, cell1);

        return new GridLine(cell1, cell2);
    }
}