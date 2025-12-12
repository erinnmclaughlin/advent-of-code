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

    public static bool TryFit(LargeGridShape bigShape, SmallGridShape smallShape)
    {
        var shapes = new List<SmallGridShape>
        {
            smallShape,
            smallShape.GetFlippedVertical(),
            smallShape.GetFlippedHorizontal(),
            smallShape.GetFlippedVertical().GetFlippedHorizontal()
        };

        foreach (var shape in shapes)
        {
            for (var i = 0; i < 5; i++)
            {
                var toTry = shape.GetRotatedLeft();
                for (var row = 0; row < bigShape.Width - 2; row++)
                {
                    for (var col = 0; col < bigShape.Height - 2; col++)
                    {
                        if (bigShape.TryAdd(toTry, row, col))
                            return true;
                    }
                }
            }
        }

        return false;
    }
    
    public static (List<SmallGridShape> Shapes, List<(LargeGridShape BigShape, int[] Target)> Instructions) ParseInput(string input)
    {
        var shapes = new List<SmallGridShape>();
        var lines = InputHelper.GetLines(input);
        var instructions = new List<(LargeGridShape BigShape, int[] Target)>();
        
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].EndsWith(':'))
            {
                var cells = ParseCells(lines[(i + 1)..(i + 4)]);
                shapes.Add(new SmallGridShape(cells.ToHashSet()));
                i += 4;
                continue;
            }

            var instructionParts = lines[i].Split(' ');
            var bigShapeParts = instructionParts[0].TrimEnd(':').Split('x');
            var bigShape = new LargeGridShape(int.Parse(bigShapeParts[1]), int.Parse(bigShapeParts[0]));
            var targets = instructionParts[1..].Select(int.Parse).ToArray();

            instructions.Add((bigShape, targets));
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

    public sealed class LargeGridShape(int height, int width)
    {
        public HashSet<GridCell> Cells { get; } = [];
        public int Height { get; } = height;
        public int Width { get; } = width;

        public bool TryAdd(SmallGridShape shape, int topOffset = 0, int leftOffset = 0)
        {
            if (shape.Cells.Max(c => c.Row) + topOffset + 1 > Height) return false;
            if (shape.Cells.Max(c => c.Col) + leftOffset + 1 > Width) return false;
            
            var cellsToAdd = new HashSet<GridCell>();

            foreach (var cell in shape.Cells)
            {
                var newCell = new GridCell(cell.Col + leftOffset, cell.Row + topOffset);

                if (Cells.Contains(newCell))
                    return false;
                
                cellsToAdd.Add(newCell);
            }

            foreach (var cell in cellsToAdd)
                Cells.Add(cell);
            
            return true;
        }
    }
    
    public sealed class SmallGridShape(HashSet<GridCell> cells)
    {
        public HashSet<GridCell> Cells { get; } = cells;

        public SmallGridShape GetFlippedVertical()
        {
            return new SmallGridShape(Cells.Select(c => new GridCell(2 - c.Col, c.Row)).ToHashSet());
        }

        public SmallGridShape GetFlippedHorizontal()
        {
            return new SmallGridShape(Cells.Select(c => new GridCell(c.Col, 2 - c.Row)).ToHashSet());
        }
        
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
