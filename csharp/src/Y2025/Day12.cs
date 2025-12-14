using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day12() : AdventDay(2025, 12)
{
    public override AdventDaySolution Solve(string input)
    {
        var fitCount = 0;
        var (_, instructions) = ParseInput(input);
        
        foreach (var (bigShape, targets) in instructions)
        {
            var neededArea = targets.Sum() * 3L * 3L; // ...ugh
            var actualArea = Math.BigMul(bigShape.Width, bigShape.Height);

            if (neededArea <= actualArea)
                fitCount++;
        }

        return (fitCount, "");
    }

    public static bool TryFit(LargeGridShape bigShape, SmallGridShape smallShape)
    {
        foreach (var shape in smallShape.GetPermutations())
        {
            for (var row = 0; row < bigShape.Width - 2; row++)
            {
                for (var col = 0; col < bigShape.Height - 2; col++)
                {
                    if (bigShape.TryAdd(shape, row, col))
                        return true;
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

        public IEnumerable<(SmallGridShape Shape, int TopOffset, int LeftOffset)> EnumerateAvailablePositions(SmallGridShape shape)
        {
            var transformedShapes =
                new List<SmallGridShape>
                    {
                        shape,
                        shape.GetFlippedHorizontal(),
                        shape.GetFlippedVertical(),
                        shape.GetFlippedHorizontal().GetFlippedVertical()
                    }
                    .Distinct();

            foreach (var transformedShape in transformedShapes)
            {
                for (var i = 0; i < 4; i++)
                {
                    var toTry = transformedShape;
                    
                    for (var x = 0; x < i; x++)
                        toTry = toTry.GetRotatedLeft();
                    
                    for (var row = 0; row < Height; row++)
                    {
                        for (var col = 0; col < Width; col++)
                        {
                            if (CanAdd(toTry, row, col))
                                yield return (toTry, row, col); 
                        }
                    }
                }
            }
        }

        public bool CanAdd(SmallGridShape shape, int topOffset, int leftOffset)
        {
            var cellsToAdd = shape.Cells.Select(c => new GridCell(c.Col + leftOffset, c.Row + topOffset)).ToList();
            
            if (cellsToAdd.Max(c => c.Row) + 1 > Height) return false;
            if (cellsToAdd.Max(c => c.Col) + 1 > Width) return false;
            
            foreach (var cell in cellsToAdd)
                if (Cells.Contains(cell))
                    return false;

            return true;
        }
        
        public bool TryAdd(SmallGridShape shape, int topOffset = 0, int leftOffset = 0)
        {
            if (!CanAdd(shape, topOffset, leftOffset)) return false;
            
            foreach (var cell in shape.Cells.Select(c => new GridCell(c.Col + leftOffset, c.Row + topOffset)))
                Cells.Add(cell);
            
            return true;
        }
    }
    
    public sealed class SmallGridShape : IGridShape2D
    {
        public GridRectangle BoundingBox { get; }

        private readonly HashSet<GridCell> _cells;
        public IReadOnlySet<GridCell> Cells => _cells;

        public SmallGridShape(ICollection<GridCell> cells)
        {
            _cells = cells.OrderBy(c => c.Row).ThenBy(c => c.Col).ToHashSet();
            
            BoundingBox = new GridRectangle(
                new GridCell(_cells.Min(c => c.Col), _cells.Min(c => c.Row)),
                new GridCell(_cells.Max(c => c.Col), _cells.Max(c => c.Row))
            );
        }
        
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

        public override bool Equals(object? obj)
        {
            return obj is SmallGridShape shape && Equals(shape);
        }

        private bool Equals(SmallGridShape other)
        {
            return other.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(SmallGridShape? left, SmallGridShape? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SmallGridShape? left, SmallGridShape? right)
        {
            return !Equals(left, right);
        }

        public IEnumerable<SmallGridShape> GetOptionsWithPair(SmallGridShape other)
        {
            var offsetX = Cells.Max(c => c.Col) + 3;
            var offsetY = Cells.Max(c => c.Row) + 3;
            
            foreach (var p1 in GetPermutations())
            {
                foreach (var p2 in other.GetPermutations())
                {
                    for (var rowOffset = 0; rowOffset <= offsetY; rowOffset++)
                    {
                        for (var colOffset = 0; colOffset <= offsetX; colOffset++)
                        {
                            var p2Cells = p2.Cells.Select(c => new GridCell(c.Col + colOffset, c.Row + rowOffset)).ToHashSet();

                            if (p1.Cells.Any(c => p2Cells.Contains(c)))
                                continue;

                            yield return new SmallGridShape(p1.Cells.Concat(p2Cells).ToArray());
                        }
                    }
                }
            }
        }

        public IEnumerable<SmallGridShape> GetPermutations()
        {
            var current = this;

            for (var i = 0; i < 4; i++)
            {
                yield return current;
                yield return current.GetFlippedHorizontal();
                yield return current.GetFlippedVertical();
                yield return current.GetFlippedHorizontal().GetFlippedVertical();
                current = current.GetRotatedLeft();
            }
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
        
        public bool Contains(GridCell cell)
        {
            return Cells.Contains(cell);
        }
    }
}
