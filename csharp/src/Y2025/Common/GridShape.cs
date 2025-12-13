namespace AdventOfCode.Y2025.Common;

public sealed class GridShape
{
    public GridRectangle BoundingBox { get; }
    public IReadOnlySet<GridRectangle> Edges { get; }
    
    private GridShape(HashSet<GridRectangle> edges)
    {
        Edges = edges;
        BoundingBox = new GridRectangle(
            new GridCell(edges.Min(e => e.Left), edges.Min(e => e.Top)),
            new GridCell(edges.Max(e => e.Right), edges.Max(e => e.Bottom))
        );
    }

    public bool IsSupershapeOf(GridRectangle other)
    {
        return BoundingBox.IsSupershapeOf(other) && 
               !Edges.Any(s => s.OverlapsWith(other));
    }

    public static GridShape CreateFromCorners(HashSet<GridCell> corners)
    {
        if (corners.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(corners), "At least one corner must be specified.");
        
        var edges = new HashSet<GridRectangle>();
        
        for (var i = 0; i < corners.Count; i++)
        {
            var previous = i == 0 ? corners.Last() : corners.ElementAt(i - 1);
            var current = corners.ElementAt(i);

            if (current.Row != previous.Row && current.Col != previous.Col)
            {
                throw new NotSupportedException("Grid shapes do not support diagonal edges.");
            }
            
            edges.Add(new GridRectangle(previous, current));
        }

        return new GridShape(edges);
    }
}