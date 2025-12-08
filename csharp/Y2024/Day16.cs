namespace AdventOfCode.Y2024;

public sealed class Day16() : AdventDay(2024, 16)
{
    public override AdventDaySolution Solve(string input)
    {
        var solutions =  CreateRunner(InputHelper.GetLines(input)).EnumerateSolutions().ToArray();

        var part1 = solutions.First().Cost;
        var part2 = solutions.SelectMany(x => x.EnumerateVisitedPositions()).Distinct().Count();

        return (part1, part2);
    }

    private static MazeRunner2D CreateRunner(string[] fileLines)
    {
        var maze = new Maze2D(fileLines.Length, fileLines[0].Length);
        var (start, target) = (Vector2D.Zero, Vector2D.Zero);
        
        for (var y = 0; y < maze.Height; y++)
        for (var x = 0; x < maze.Width; x++)
        {
            var character = fileLines[y][x];

            if (character == '.')
                continue;

            var position = new Vector2D(x, y);

            switch (character)
            {
                case '#':
                    maze.Walls.Add(position);
                    break;
                case 'S':
                    start = position;
                    break;
                case 'E':
                    target = position;
                    break;
            }
        }

        return new MazeRunner2D(maze, Direction.Right, start, target);
    }
}
