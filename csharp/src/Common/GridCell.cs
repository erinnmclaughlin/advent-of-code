namespace AdventOfCode.Common;

public record GridCell(int Col, int Row) : IGridShape2D
{
    public GridRectangle BoundingBox => new(this, this);
    
    public bool Contains(GridCell cell) => cell == this;
    
    public bool IsAdjacentTo(GridCell other) =>
        Row == other.Row && Math.Abs(Col - other.Col) == 1 ||
        Col == other.Col && Math.Abs(Row - other.Row) == 1;
}