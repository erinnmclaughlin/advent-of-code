using System.Text;

namespace AdventOfCode.Y2025.Common;

public sealed class GridShape
{
    private readonly List<GridLine> _edges;
    private readonly List<GridCell> _innerCorners = [];
    private readonly List<GridCell> _outerCorners = [];

    public GridRectangle BoundingBox { get; }
    
    public int EdgeCount => _edges.Count;
    
    public GridShape(List<GridCell> corners) : this(GetEdgesFromCorners(corners))
    {
    }
    
    public GridShape(List<GridLine> edges)
    {
        // TODO: Add error checking (edge[i] needs to connect to edge[i+1]. edge[0] connects to edge[n])
        _edges = edges;
        
        var minRow = Math.Min(edges.Min(e => e.Start.Row), edges.Min(e => e.End.Row));
        var maxRow = Math.Max(edges.Max(e => e.Start.Row), edges.Max(e => e.End.Row));
        var minCol = Math.Min(edges.Min(e => e.Start.Col), edges.Min(e => e.End.Col));
        var maxCol = Math.Max(edges.Max(e => e.Start.Col), edges.Max(e => e.End.Col));

        BoundingBox = new GridRectangle(
            new GridCell(minCol, minRow),
            new GridCell(maxCol, maxRow)
        );

        foreach (var edge in _edges)
        {
            var cell = edge.Start;

            if (Contains(new GridCell(cell.Col, cell.Row - 1)) &&
                Contains(new GridCell(cell.Col, cell.Row + 1)) &&
                Contains(new GridCell(cell.Col - 1, cell.Row)) &&
                Contains(new GridCell(cell.Col + 1, cell.Row)))
            {
                _innerCorners.Add(cell);
            }
            else
            {
                _outerCorners.Add(cell);
            }
        }
    }

    public bool Contains(GridRectangle rectangle)
    {
        if (!BoundingBox.Contains(rectangle))
            return false;

        if (!Contains(rectangle.TopLeft) ||
            !Contains(rectangle.TopRight) ||
            !Contains(rectangle.BottomLeft) ||
            !Contains(rectangle.BottomRight))
            return false;
        
        foreach (var ic in EnumerateCorners())
        {
            if (!rectangle.Contains(ic))
                continue;
            
            if (!rectangle.IsOnEdge(ic))
                return false;

            // todo: really only need to check diagonals but i'm being extra
            for (var rowOffset = -1; rowOffset <= 1; rowOffset++)
            {
                for (var colOffset = -1; colOffset <= 1; colOffset++)
                {
                    if (rowOffset == 0 && colOffset == 0) continue;
                        
                    var cell = new GridCell(ic.Col + colOffset, ic.Row + rowOffset);
                    if (rectangle.Contains(cell) && !Contains(cell))
                        return false;
                }
            }

        }
        
        return true;
    }

    public bool Contains(GridLine line)
    {
        // todo: be more efficient
        return line.EnumerateCells().AsParallel().All(Contains);
    }

    public bool Contains(GridCell cell)
    {
        if (!BoundingBox.Contains(cell))
            return false;

        if (IsOnEdge(cell))
            return true;
        
        if (!EnumerateEdgesAbove(cell).Any()) return false;
        if (!EnumerateEdgesBelow(cell).Any()) return false;
        if (!EnumerateEdgesLeftOf(cell).Any()) return false;
        if (!EnumerateEdgesRightOf(cell).Any()) return false;

        return true;
    }
    
    public IEnumerable<GridLine> EnumerateEdgesAbove(GridCell cell)
    {
        foreach (var edge in _edges.Where(e => e.IsHorizontal && e.Start.Row < cell.Row))
        {
            var startCol = Math.Min(edge.Start.Col, edge.End.Col);
            var endCol = Math.Max(edge.Start.Col, edge.End.Col);
            
            if (endCol < cell.Col || startCol > cell.Col)
                continue;
            
            yield return edge;
        }
    }

    public IEnumerable<GridLine> EnumerateEdgesBelow(GridCell cell)
    {
        foreach (var edge in _edges.Where(e => e.IsHorizontal && e.Start.Row > cell.Row))
        {
            var startCol = Math.Min(edge.Start.Col, edge.End.Col);
            var endCol = Math.Max(edge.Start.Col, edge.End.Col);
            
            if (endCol< cell.Col || startCol > cell.Col)
                continue;
            
            yield return edge;
        }
    }

    public IEnumerable<GridLine> EnumerateEdgesLeftOf(GridCell cell)
    {
        var maybes = _edges.Where(e => e.IsVertical && e.Start.Col < cell.Col).ToList();
        
        foreach (var edge in maybes)
        {
            var startRow = Math.Min(edge.Start.Row, edge.End.Row);
            var endRow = Math.Max(edge.Start.Row, edge.End.Row);
            
            var endsTooEarly = endRow < cell.Row;
            var startsTooLate = startRow > cell.Row;
            
            if (endsTooEarly || startsTooLate)
                continue;
            
            yield return edge;
        }
    }

    public IEnumerable<GridLine> EnumerateEdgesRightOf(GridCell cell)
    {
        foreach (var edge in _edges.Where(e => e.IsVertical && e.Start.Col > cell.Col))
        {
            var startRow = Math.Min(edge.Start.Row, edge.End.Row);
            var endRow = Math.Max(edge.Start.Row, edge.End.Row);
            
            if (endRow < cell.Row || startRow > cell.Row)
                continue;
            
            yield return edge;
        }
    }

    public bool IsOnCorner(GridCell cell) => EnumerateCorners().Contains(cell);
    public bool IsOnEdge(GridCell cell) => _edges.Any(e => e.Contains(cell));
    
    private IEnumerable<GridCell> EnumerateCorners()
    {
        foreach (var edge in _edges)
        {
            yield return edge.Start;
            yield return edge.End;
        }
    }

    private static List<GridLine> GetEdgesFromCorners(List<GridCell> corners)
    {
        var edges = new List<GridLine>();
        
        for (var i = 0; i < corners.Count; i++)
        {
            var c1 = i == 0? corners[^1] : corners[i - 1];
            var c2 = corners[i];
            
            if (c1.Row != c2.Row && c1.Col != c2.Col)
                throw new ArgumentException($"Cannot create straight line between {c1} and {c2}. Corners must be adjacent.");

            edges.Add(GridLine.CreateStraight(c1, c2));
        }
        
        return edges;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        
        for (var row = BoundingBox.Top; row <= BoundingBox.Bottom; row++)
        {
            for (var col = BoundingBox.Left; col <= BoundingBox.Right; col++)
            {
                var cell = new GridCell(col, row);

                if (_innerCorners.Contains(cell))
                {
                    sb.Append('I');
                }
                else if (_outerCorners.Contains(cell))
                {
                    sb.Append('O');
                }
                else if (IsOnEdge(cell))
                {
                    sb.Append('X');
                }
                else if (Contains(new GridCell(col, row)))
                {
                    sb.Append('C');
                }
                else
                {
                    sb.Append('.');
                }
            }
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }

    public string ToString(GridRectangle rect)
    {
        var sb = new StringBuilder();
        
        for (var row = BoundingBox.Top; row <= BoundingBox.Bottom; row++)
        {
            for (var col = BoundingBox.Left; col <= BoundingBox.Right; col++)
            {
                var cell = new GridCell(col, row);

                if (_innerCorners.Contains(cell))
                {
                    sb.Append('I');
                }
                else if (_outerCorners.Contains(cell))
                {
                    sb.Append('O');
                }
                else if (rect.Contains(cell))
                {
                    sb.Append('R');
                }
                else if (IsOnEdge(cell))
                {
                    sb.Append('X');
                }
                else if (Contains(new GridCell(col, row)))
                {
                    sb.Append('C');
                }
                else
                {
                    sb.Append('.');
                }
            }
            
            sb.AppendLine();
        }
        
        return sb.ToString();
    }
}