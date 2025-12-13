namespace AdventOfCode.Y2025.Common;

public sealed record GridCell(int Col, int Row) : AdventCell(Row, Col), IGridShape2D
{
    public GridRectangle BoundingBox => new(this, this);
    
    public static GridCell Parse(string input)
    {
        var parts = input.Split(',');
        return new GridCell(int.Parse(parts[0]), int.Parse(parts[1]));
    }
    
    public override string ToString() => $"{Col},{Row}";
    
    public bool Contains(GridCell cell)
    {
        return cell == this;
    }
}