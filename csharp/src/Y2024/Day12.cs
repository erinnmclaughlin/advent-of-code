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
                group.Add(cell);
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

                    kvp.Value[i].AddRange(kvp.Value[j].Cells);
                    kvp.Value.RemoveAt(j);
                    break;
                }
            }
        });

        return groupLookup.SelectMany(x => x.Value).ToList();
    }
    
    private sealed class CellGroup(char label) : AdventCellGroup<Cell>
    {
        private readonly List<Cell> _cells = [];
        public override IReadOnlyList<Cell> Cells => _cells;
        
        private char Label { get; } = label;
        
        public bool CanMergeWith(CellGroup other)
        {
            return other != this &&
                   other.Label == Label && 
                   Cells.Any(c => other.Cells.Any(oc => oc.IsAdjacentTo(c)));
        }

        public void Add(Cell cell)
        {
            _cells.Add(cell);
        }

        public void AddRange(params IEnumerable<Cell> cells)
        {
            _cells.AddRange(cells);
        }
    }

    private sealed record Cell(char Label, int Row, int Col) : AdventCell(Row, Col);
}
