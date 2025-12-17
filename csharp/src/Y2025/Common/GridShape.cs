namespace AdventOfCode.Y2025.Common;

public sealed record GridShape
{
    private readonly HashSet<GridRectangle> _innerRectangles;
    
    public GridRectangle BoundingBox { get; }
    public IReadOnlySet<GridRectangle> Edges { get; }
    
    private GridShape(HashSet<GridRectangle> edges)
    {
        Edges = edges;
        BoundingBox = new GridRectangle(
            new GridCell(edges.Min(e => e.Left), edges.Min(e => e.Top)),
            new GridCell(edges.Max(e => e.Right), edges.Max(e => e.Bottom))
        );
        _innerRectangles = EnumerateInnerRectangles().ToHashSet();
    }

    public bool Contains(GridCell cell)
    {
        if (!BoundingBox.Contains(cell))
            return false;

        return _innerRectangles.Any(r => r.Contains(cell));
    }

    public bool IsSupershapeOf(GridRectangle other)
    {
        if (!Contains(other.TopLeft) ||
            !Contains(other.TopRight) ||
            !Contains(other.BottomLeft) ||
            !Contains(other.BottomRight))
            return false;
        
        return !Edges.Any(s => s.OverlapsWith(other, includeEdges: false));
    }

    public static GridShape CreateFromCorners(params HashSet<GridCell> corners)
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
    
    private IEnumerable<GridRectangle> EnumerateInnerRectangles()
    {
        var horizontalEdges = Edges.Where(e => e.Height == 1).ToArray();

        foreach (var e1 in horizontalEdges)
        {
            foreach (var e2 in horizontalEdges.Where(e2 => e2.Top > e1.Top && e2.Left <= e1.Right && e1.Left <= e2.Right))
            {
                yield return new GridRectangle(
                    new GridCell(Math.Max(e1.Left, e2.Left), e1.Top),
                    new GridCell(Math.Min(e1.Right, e2.Right), e2.Top)
                );
            }
        }
    }
}