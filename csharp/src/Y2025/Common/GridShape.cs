using System.Text;

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

    public bool Contains(GridCell cell)
    {
        // If we're out of bounds entirely, then this is easy:
        if (!BoundingBox.Contains(cell)) 
            return false;

        // otherwise, a cell is "contained" if it is an edge cell, or if it is surrounded by an odd number of edges on all sides
        var (top, bottom, left, right) = (0, 0, 0, 0);
        foreach (var edge in Edges)
        {
            if (edge.Contains(cell))
                return true;
            
            if (edge.IsAbove(cell)) top++;
            if (edge.IsBelow(cell)) bottom++;
            if (edge.IsToTheLeftOf(cell)) left++;
            if (edge.IsToTheRightOf(cell)) right++;
        }
        return top % 2 != 0 && 
               bottom % 2 != 0 && 
               left % 2 != 0 && 
               right % 2 != 0;
        
        // Original linq version, but this is less performant (loops five times instead of just once):
        /*
        Edges.Any(e => e.Contains(cell)) || (
            Edges.Count(e => e.IsAbove(cell)) % 2 != 0 &&
            Edges.Count(e => e.IsBelow(cell)) % 2 != 0 &&
            Edges.Count(e => e.IsToTheLeftOf(cell)) % 2 != 0 &&
            Edges.Count(e => e.IsToTheRightOf(cell)) % 2 != 0
        );*/
    }

    public bool IsSupershapeOf(GridRectangle other) =>
        BoundingBox.OverlapsWith(other) && 
        !Edges.Any(s => s.OverlapsWith(other));

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

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        for (var row = BoundingBox.Top; row <= BoundingBox.Bottom; row++)
        {
            for (var col = BoundingBox.Left; col <= BoundingBox.Right; col++)
            {
                sb.Append(Contains(new GridCell(col, row)) ? '#' : '.');
            }
            
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }
}