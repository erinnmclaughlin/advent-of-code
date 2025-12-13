namespace AdventOfCode.Y2025.Common;

public sealed class GridRectangle
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

    public GridRectangle(GridCell cell) : this(cell, cell)
    {
    }
    
    public GridRectangle(GridCell c1, GridCell c2)
    {
        var top = Math.Min(c1.Row, c2.Row);
        var bottom = Math.Max(c1.Row, c2.Row);
        var left = Math.Min(c1.Col, c2.Col);
        var right = Math.Max(c1.Col, c2.Col);

        TopLeft = new GridCell(left, top);
        BottomRight = new GridCell(right, bottom);
        TopRight = new GridCell(right, top);
        BottomLeft = new GridCell(left, bottom);
    }
    
    public bool Contains(GridCell cell) =>
        cell.Row >= Top && 
        cell.Row <= Bottom && 
        cell.Col >= Left &&
        cell.Col <= Right;

    public long GetArea() => 
        Math.BigMul(Width, Height);

    public bool IsAbove(GridCell cell) =>
        Top > cell.Row && Left <= cell.Col && Right >= cell.Col;
    
    public bool IsBelow(GridCell cell) =>
        Bottom < cell.Row && Left <= cell.Col && Right >= cell.Col;
    
    public bool IsToTheLeftOf(GridCell cell) =>
        Left > cell.Col && Top <= cell.Row && Bottom >= cell.Row;
    
    public bool IsToTheRightOf(GridCell cell) =>
        Right < cell.Col && Top <= cell.Row && Bottom >= cell.Row;
    
    public bool IsSubshapeOf(GridRectangle other) => 
        other.IsSupershapeOf(this);
    
    public bool IsSubshapeOf(GridShape other) => 
        other.IsSupershapeOf(this);

    public bool IsSupershapeOf(GridRectangle other) => 
        Top <= other.Top && 
        Bottom >= other.Bottom && 
        Left <= other.Left && 
        Right >= other.Right;

    public bool OverlapsWith(GridRectangle other) =>
        // Overlaps on the vertical axis:
        Right > other.Left && Left < other.Right &&
        // Overlaps on the horizontal axis:
        Bottom > other.Top && Top < other.Bottom;

    public bool IsOnEdge(GridCell cell) =>
        Contains(cell) && (
            cell.Row == Top || 
            cell.Row == Bottom || 
            cell.Col == Left || 
            cell.Col == Right
        );
}