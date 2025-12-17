namespace AdventOfCode.Y2024;

// 9476 is too high
// 1398 is too high
public sealed class Day20() : AdventDay(2024, 20)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        var (maze, start, end) = ParseLines(lines);
        
        return (GetSavedStepCountAlt(maze, start, end, 99, null), "");
    }

    public static int GetSavedStepCountAlt(Maze2D maze, Vector2D start, Vector2D end, int minSavings, int? maxSavings)
    {
        var defaultSolution = GetSolutions(maze, start, end)
            .MinBy(x => x.Cost)!
            .EnumerateVisitedPositions()
            .ToList();
        
        var count = 0;
        
        for (var i = 0; i < defaultSolution.Count; i++)
        {
            var step = defaultSolution[i];
            
            foreach (var dir in Enum.GetValues<Direction>())
            {
                // if we can't get there by removing a wall, it's not valid
                if (!maze.Walls.Contains(step + dir))
                    continue;

                // this extra loop doesn't seem actually necessary, but it does account for L-shapes
                foreach (var otherDir in Enum.GetValues<Direction>()) 
                {
                    // where would we end up?
                    var cheatStep = step + dir + otherDir;
                
                    // if the result is not already on our path, then it's not valid
                    var cheatStepIndex = defaultSolution.IndexOf(cheatStep);
                    if (cheatStepIndex == -1)
                        continue;

                    // if the result comes before where we already are, then this won't save us any time
                    if (cheatStepIndex <= i + 2)
                        continue;
                
                    var savings = cheatStepIndex - i - 2;
                
                    if (savings > minSavings && (maxSavings is null || savings < maxSavings))
                        count++;
                }
            }
        }

        return count;
    }
    
    public static IEnumerable<MazeRunnerState2D> GetSolutions(Maze2D maze, Vector2D start, Vector2D end)
    {
        foreach (var runner in Enum.GetValues<Direction>().Select(d => new MazeRunner2D(maze, d, start, end)))
        foreach (var solution in runner.EnumerateSolutions())
            yield return solution;
    }

    public static (Maze2D Maze, Vector2D Start, Vector2D End) ParseLines(string[] lines)
    {
        var maze = new Maze2D(lines.Length, lines[0].Length);
        var start = new Vector2D(0, 0);
        var end = new Vector2D(0, 0);

        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == '#')
                    maze.Walls.Add(new Vector2D(x, y));
                else if (lines[y][x] == 'S')
                    start = new  Vector2D(x, y);
                else if (lines[y][x] == 'E')
                    end = new  Vector2D(x, y);
            }
        }
        
        return (maze, start, end);
    }
}
