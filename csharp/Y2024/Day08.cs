namespace AdventOfCode.Y2024;

public sealed class Day08() : AdventDay(2024, 8)
{
    public override AdventDaySolution Solve(string input)
    {
        var grid = InputHelper.GetLines(input).AsSpan();

        var part1 =  grid.CountAntiNodes((antiNodes, r1, c1, r2, c2, maxRow, maxCol) =>
        {
            var (dRow, dCol) = (r2 - r1, c2 - c1);

            if (IsInBounds(r1 -= dRow, c1 -= dCol, maxRow, maxCol))
                antiNodes.Add(HashCode.Combine(r1, c1));

            if (IsInBounds(r2 += dRow, c2 += dCol, maxRow, maxCol))
                antiNodes.Add(HashCode.Combine(r2, c2));
        });

        var part2 =  grid.CountAntiNodes((antiNodes, r1, c1, r2, c2, maxRow, maxCol) =>
        {
            var (dRow, dCol) = (r2 - r1, c2 - c1);

            while (IsInBounds(r1, c1, maxRow, maxCol))
            {
                antiNodes.Add(HashCode.Combine(r1, c1));
                r1 -= dRow;
                c1 -= dCol;
            }

            while (IsInBounds(r2, c2, maxRow, maxCol))
            {
                antiNodes.Add(HashCode.Combine(r2, c2));
                r2 += dRow;
                c2 += dCol;
            }
        });

        return (part1, part2);
    }
    
    private static bool IsInBounds(int row, int col, int numRows, int numCols) =>
        row >= 0 && 
        col >= 0 && 
        row < numRows &&
        col < numCols;
}

file static class Extensions
{
    public static int CountAntiNodes(this in ReadOnlySpan<string> grid, in Action<HashSet<int>, int, int, int, int, int, int> handle)
    {
        var antiNodes = new HashSet<int>();
        var (numRows, numCols) = (grid.Length, grid[0].Length);

        for (var i = 0; i < grid.Length; i++)
        {
            var rowSpan = grid[i].AsSpan();
            
            var j = -1;
            while (rowSpan[(j + 1)..].ContainsAnyExcept('.'))
            {
                j += rowSpan[(j + 1)..].IndexOfAnyExcept('.') + 1;

                for (var k = grid.Length - 1; k > i; k--)
                {
                    var l = grid[k].AsSpan().IndexOf(rowSpan[j]);
                    if (l != -1) handle(antiNodes, i, j, k, l, numRows, numCols);
                }
            }
        }

        return antiNodes.Count;
    }
}