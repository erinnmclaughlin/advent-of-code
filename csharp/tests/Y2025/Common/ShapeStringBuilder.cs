namespace AdventOfCode.Tests.Y2025.Common;

public sealed class ShapeStringBuilder(ITestOutputHelper output)
{
    private readonly List<(char Label, IGridShape2D Shape)> _shapes = [];

    public void Add(IGridShape2D shape, char label) => _shapes.Add((label, shape));
    
    public void Clear() => _shapes.Clear();

    public void Dump()
    {
        if (_shapes.Count == 0) return;
        
        var top = _shapes.Min(s => s.Shape.BoundingBox.Top);
        var bottom = _shapes.Max(s => s.Shape.BoundingBox.Bottom);
        var left = _shapes.Min(s => s.Shape.BoundingBox.Left);
        var right = _shapes.Max(s => s.Shape.BoundingBox.Right);
        
        for (var row = top; row <= bottom; row++)
        {
            for (var col = left; col <= right; col++)
            {
                var cell = new GridCell(col, row);
                var found = false;

                for (var i = _shapes.Count - 1; i >= 0; i--)
                {
                    if (_shapes[i].Shape.Contains(cell))
                    {
                        output.Write(_shapes[i].Label.ToString());
                        found = true;
                        break;
                    }
                }
                
                if (!found)
                    output.Write(".");
            }

            if (row != bottom)
                output.WriteLine("");
        }
    }
    
}