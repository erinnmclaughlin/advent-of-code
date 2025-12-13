namespace AdventOfCode.Y2025.Common;

public interface IGridShape2D
{
    GridRectangle BoundingBox { get; }
    bool Contains(GridCell cell);
}