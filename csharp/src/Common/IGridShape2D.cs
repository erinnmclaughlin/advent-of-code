namespace AdventOfCode.Common;

public interface IGridShape2D
{
    GridRectangle BoundingBox { get; }
    bool Contains(GridCell cell);
}