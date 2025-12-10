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

    public bool Contains(GridCell cell)
    {
        if (IsHorizontal)
        {
            return cell.Row == Start.Row && cell.Col >= Start.Col && cell.Col <= End.Col;
        }

        if (IsVertical)
        {
            return cell.Col == Start.Col && cell.Row >= Start.Row && cell.Row <= End.Row;
        }
        
        throw new NotImplementedException();
    }

    public static GridLine CreateStraight(GridCell cell1, GridCell cell2)
    {
        if (cell1.Col == cell2.Col)
            return CreateVertical(cell1, cell2);
        
        if (cell1.Row == cell2.Row)
            return CreateHorizontal(cell1, cell2);
        
        throw new ArgumentException();
    }

    public static GridLine CreateVertical(GridCell cell1, GridCell cell2)
    {
        if (cell1.Col != cell2.Col)
            throw new ArgumentException();

        if (cell1.Row > cell2.Row)
            return new GridLine(cell2, cell1);
        
        return new GridLine(cell1, cell2);
    }

    public static GridLine CreateHorizontal(GridCell cell1, GridCell cell2)
    {
        if (cell1.Row != cell2.Row)
            throw new ArgumentException();

        if (cell1.Col > cell2.Col)
            return new GridLine(cell2, cell1);

        return new GridLine(cell1, cell2);
    }
}