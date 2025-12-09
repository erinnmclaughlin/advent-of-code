namespace AdventOfCode.Common;

public abstract class AdventCellGroup<T> where T : AdventCell
{
    public abstract IReadOnlyList<T> Cells { get; }
    
    public int GetPerimeter() => Cells.Sum(cell => 4 - Cells.Count(c => c.IsAdjacentTo(cell)));

    public int GetNumberOfSides()
    {
        return CountOuterCorners() + CountInnerCorners();
    }

    public int CountOuterCorners()
    {
        var count = 0;
        
        foreach (var cell in Cells)
        {
            var top = Cells.FirstOrDefault(c => c.Row == cell.Row - 1 && c.Col == cell.Col);
            var left = Cells.FirstOrDefault(c => c.Row == cell.Row && c.Col == cell.Col - 1);
            var bottom = Cells.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Col == cell.Col);
            var right = Cells.FirstOrDefault(c => c.Row == cell.Row && c.Col == cell.Col + 1);

            if (top is null && left is null) count++;
            if (left is null && bottom is null) count++;
            if (bottom is null && right is null) count++;
            if (right is null && top is null) count++;
        }

        return count;
    }

    public int CountInnerCorners()
    {
        var count = 0;
        foreach (var cell in Cells)
        {
            var top = Cells.FirstOrDefault(c => c.Row == cell.Row - 1 && c.Col == cell.Col);
            var left = Cells.FirstOrDefault(c => c.Row == cell.Row && c.Col == cell.Col - 1);
            var bottom = Cells.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Col == cell.Col);
            var right = Cells.FirstOrDefault(c => c.Row == cell.Row && c.Col == cell.Col + 1);

            var topLeft = Cells.FirstOrDefault(c => c.Row == cell.Row - 1 && c.Col == cell.Col - 1);
            var topRight = Cells.FirstOrDefault(c => c.Row == cell.Row - 1 && c.Col == cell.Col + 1);
            var bottomLeft = Cells.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Col == cell.Col - 1);
            var bottomRight = Cells.FirstOrDefault(c => c.Row == cell.Row + 1 && c.Col == cell.Col + 1);
            
            if (top is not null && left is not null && topLeft is null) count++;
            if (top is not null && right is not null && topRight is null) count++;
            if (bottom is not null && left is not null && bottomLeft is null) count++;
            if (bottom is not null && right is not null && bottomRight is null) count++;
        }

        return count;
    }
}