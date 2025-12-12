using System.Text;
using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Y2025;

public sealed class Day12() : AdventDay(2025, 12)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);

        // TODO: implement puzzle logic here

        return ("", "");
    }

    public static (List<SmallGridShape> Shapes, string[] Instructions) ParseInput(string input)
    {
        var shapes = new List<SmallGridShape>();
        var lines = InputHelper.GetLines(input);
        var instructions = new string[] { };
        
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].EndsWith(':'))
            {
                var cells = ParseCells(lines[(i + 1)..(i + 4)]);
                shapes.Add(new SmallGridShape(cells.ToHashSet()));
                i += 4;
                continue;
            }

            //if (lines[i].Contains('#') || lines[i].Contains('.'))
            //    continue;

            instructions = lines[i..];
            break;
        }

        return (shapes, instructions);
    }

    private static IEnumerable<GridCell> ParseCells(string[] lines)
    {
        for (var row = 0; row < lines.Length; row++)
        {
            for (var col = 0; col < lines[row].Length; col++)
            {
                if (lines[row][col] == '#')
                    yield return new GridCell(col, row);
            }
        }
    }

    public sealed class SmallGridShape(HashSet<GridCell> cells)
    {
        public HashSet<GridCell> Cells { get; } = cells;

        public SmallGridShape GetRotatedLeft()
        {
            return new SmallGridShape(Cells.Select(c => new GridCell(2 - c.Row, c.Col)).ToHashSet());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var row = 0; row < 3; row++)
            {
                for (var col = 0; col < 3; col++)
                {
                    var hasCell = Cells.Any(c => c.Row == row && c.Col == col);
                    sb.Append(hasCell ? '#' : '.');
                }
                
                sb.AppendLine();
            }
            
            return sb.ToString().TrimEnd();
        }
    }
}
