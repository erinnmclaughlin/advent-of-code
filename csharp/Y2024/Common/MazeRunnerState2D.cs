namespace AdventOfCode.Y2024;

public sealed class MazeRunnerState2D(Direction direction, Vector2D position)
{
    public int Cost { get; private init; }
    public Direction Direction { get; } = direction;
    public Vector2D Position { get; } = position;
    public MazeRunnerState2D? PreviousState { get; private init; }

    public bool Contains(Direction direction, Vector2D position)
    {
        var state = this;

        while (state != null)
        {
            if (state.Direction == direction && state.Position == position)
                return true;

            state = state.PreviousState;
        }

        return false;
    }

    public IEnumerable<Vector2D> EnumerateVisitedPositions()
    {
        var state = this;

        while (state != null)
        {
            yield return state.Position;
            state = state.PreviousState;
        }
    }
    
    public MazeRunnerState2D GetNextState(Direction directionToMove, IMazeRunner2DCostCalculator? costCalculator = null) => new(directionToMove, Position + directionToMove)
    {
        Cost = Cost + (costCalculator ?? new DefaultMazeRunner2DCostCalculator()).CalculateCost(this, directionToMove),
        PreviousState = this
    };
}