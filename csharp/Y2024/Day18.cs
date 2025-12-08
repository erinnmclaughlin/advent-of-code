using System.Text;

namespace AdventOfCode.Y2024;

public sealed class Day18() : AdventDay(2024, 18)
{
    public override AdventDaySolution Solve(string input) => Solve(input, 71, 1024);

    public static AdventDaySolution Solve(string input, int size, int steps)
    {
        var maze = new Maze2D(size, size);
        var startPosition = new Vector2D(0, 0);
        var targetPosition = new Vector2D(size - 1, size - 1);

        var bytes = InputHelper.GetLines(input).Select(ParseLine).ToArray();

        foreach (var (x,y) in bytes.Take(steps))
        {
            maze.Walls.Add(new Vector2D(x, y));
        }

        var runners = new List<MazeRunner2D>
        {
            new(maze, Direction.Down, startPosition, targetPosition),
            new(maze, Direction.Right, startPosition, targetPosition)
        };

        var cost = runners.Min(x => x.EnumerateSolutions().Min(x => x.Cost));

        foreach (var (x,y) in bytes.Skip(steps))
        {
            maze.Walls.Add(new Vector2D(x, y));

            runners.ForEach(r => r.Reset());

            if (runners.All(r => !r.EnumerateSolutions().Any()))
                return (cost, $"{x},{y}");
        }

        return (cost, "");
    }

    public static (int X, int Y) ParseLine(string line)
    {
        var parts = line.Split(',');
        return (int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public sealed class Map(int size)
    {
        public List<(int X, int Y)> CorruptedPositions { get; } = [];
        public int Size { get; } = size;

        public bool Try(List<(int X, int Y)> path)
        {
            for (var i = 0; i < path.Count; i++)
            {
                if (CorruptedPositions[i] == path[i])
                    return false;
            }

            return true;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (var y = 0; y < Size; y++)
            {
                for (var x = 0; x < Size; x++)
                {
                    sb.Append(CorruptedPositions.Contains((x, y)) ? '#' : '.');
                }

                sb.AppendLine();
            }

            return sb.ToString().TrimEnd();
        }
    }
}
