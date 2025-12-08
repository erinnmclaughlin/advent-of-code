namespace AdventOfCode.Y2024;

public sealed class Day12() : AdventDay(2024, 12)
{
    public override AdventDaySolution Solve(string input)
    {
        var map = InputHelper.GetLines(input).AsSpan();
        var groups = FormGroups(map);

        var part1 = groups.Sum(group => group.Cells.Count * group.GetPerimeter());
        var part2 = groups.Sum(x => x.Cells.Count * x.GetNumberOfSides());
        return (part1, part2);
    }
    
    private static List<CellGroup> FormGroups(ReadOnlySpan<string> map)
    {
        var groupLookup = new Dictionary<char, List<CellGroup>>();

        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[i].Length; j++)
            { 
                var cell = new Cell(map[i][j], i, j);
                groupLookup.TryAdd(cell.Label, []);
                var group = new CellGroup(cell.Label);
                group.Cells.Add(cell);
                groupLookup[cell.Label].Add(group);
            }
        }

        Parallel.ForEach(groupLookup, kvp =>
        {
            for (var i = kvp.Value.Count - 1; i >= 0; i--)
            {
                for (var j = 0; j < i; j++)
                {
                    if (!kvp.Value[i].CanMergeWith(kvp.Value[j]))
                        continue;

                    kvp.Value[i].Cells.AddRange(kvp.Value[j].Cells);
                    kvp.Value.RemoveAt(j);
                    break;
                }
            }
        });

        return groupLookup.SelectMany(x => x.Value).ToList();
    }
    
    private sealed class CellGroup(char label)
    {
        private char Label { get; } = label;
        public List<Cell> Cells { get; } = [];

        public bool CanMergeWith(CellGroup other)
        {
            return other != this &&
                   other.Label == Label && 
                   Cells.Any(c => other.Cells.Any(oc => oc.IsAdjacentTo(c)));
        }

        public int GetPerimeter() => Cells.Sum(cell => 4 - Cells.Count(c => c.IsAdjacentTo(cell)));

        public int GetNumberOfSides()
        {
            return CountOuterCorners() + GetInnerCorners();
        }

        private int CountOuterCorners()
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

        private int GetInnerCorners()
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

    private sealed class Cell(char label, int row, int col)
    {
        public char Label { get; } = label;
        public int Row { get; } = row;
        public int Col { get; } = col;

        public bool IsAdjacentTo(Cell other) =>
            Row == other.Row && Math.Abs(Col - other.Col) == 1 ||
            Col == other.Col && Math.Abs(Row - other.Row) == 1;
    }
}
