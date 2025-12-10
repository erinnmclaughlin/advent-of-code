namespace AdventOfCode.Y2025.Common;

public sealed record GridRectangle
{
    public int Top => TopLeft.Row;
    public int Right => TopRight.Col;
    public int Bottom => BottomRight.Row;
    public int Left => BottomLeft.Col;
    
    public int Height => Bottom - Top + 1;
    public int Width => Right - Left + 1;
    
    public GridCell TopLeft { get; }
    public GridCell TopRight { get; }
    public GridCell BottomLeft { get; }
    public GridCell BottomRight { get; }

    public GridLine TopRow => GridLine.CreateHorizontal(TopLeft, TopRight);
    public GridLine BottomRow => GridLine.CreateHorizontal(BottomLeft, BottomRight);
    public GridLine LeftCol => GridLine.CreateVertical(TopLeft, BottomLeft);
    public GridLine RightCol => GridLine.CreateVertical(TopRight, BottomRight);
        
    public GridRectangle(GridCell topLeft, GridCell bottomRight)
    {
        if (topLeft.Row > bottomRight.Row || topLeft.Col > bottomRight.Col)
            throw new ArgumentException();
        
        TopLeft = topLeft;
        BottomRight = bottomRight;
        TopRight = new GridCell(bottomRight.Col, topLeft.Row);
        BottomLeft = new GridCell(topLeft.Col, bottomRight.Row);
    }

    public long GetArea()
    {
        return Math.BigMul(Width, Height);
    }

    public bool Contains(GridCell cell)
    {
        return cell.Row >= Top && cell.Row <= Bottom && cell.Col >= Left && cell.Col <= Right;
    }

    public bool Contains(GridLine row)
    {
        return Contains(row.Start) && Contains(row.End);
    }
}