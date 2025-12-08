namespace AdventOfCode.Y2024;

public sealed class MazeRunner2D
{
    private readonly PriorityQueue<MazeRunnerState2D, int> _queue = new();
    private readonly HashSet<(Direction Direction, Vector2D Position)> _visited = [];
    
    public Maze2D Maze { get; }
    public Direction StartDirection { get; }
    public Vector2D StartPosition { get; }
    public Vector2D TargetPosition { get; }
    
    public MazeRunner2D(Maze2D maze, Direction startDirection, Vector2D startPosition, Vector2D targetPosition)
    {
        Maze = maze;
        StartDirection = startDirection;
        StartPosition = startPosition;
        TargetPosition = targetPosition;
        
        _queue.Enqueue(new MazeRunnerState2D(startDirection, startPosition), 0);
    }

    public IEnumerable<MazeRunnerState2D> EnumerateSolutions(IMazeRunner2DCostCalculator? costCalculator = null)
    {
        MazeRunnerState2D? knownSolution = null;
        
        while (_queue.TryDequeue(out var currentState, out _))
        {
            var state = (currentState.Direction, currentState.Position);

            if (!_visited.Add(state) && knownSolution?.Contains(currentState.Direction, currentState.Position) is not true)
                continue;

            if (currentState.Position == TargetPosition)
            {
                if (knownSolution is null)
                {
                    _visited.Clear();
                    _queue.Clear();
                    _queue.Enqueue(new MazeRunnerState2D(StartDirection, StartPosition), 0);
                }
                else if (currentState.Cost > knownSolution.Cost || knownSolution.EnumerateVisitedPositions().SequenceEqual(currentState.EnumerateVisitedPositions()))
                {
                    continue;
                }

                knownSolution = currentState;
                yield return currentState;
            }
            else
            {
                foreach (var (dir, _) in Maze.EnumerateOpenAdjacentPaths(currentState.Position))
                {
                    if (dir == currentState.Direction.GetOpposite()) 
                        continue;

                    var nextState = currentState.GetNextState(dir, costCalculator);
                    _queue.Enqueue(nextState, nextState.Cost);
                }
            }
        }
    }
}