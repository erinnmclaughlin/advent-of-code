namespace AdventOfCode.Y2024;

public interface IMazeRunner2DCostCalculator
{
    int CalculateCost(MazeRunnerState2D currentState, Direction directionToMove);
}

public sealed class DefaultMazeRunner2DCostCalculator : IMazeRunner2DCostCalculator
{
    public int CalculateCost(MazeRunnerState2D currentState, Direction directionToMove)
    {
        return 1;
    }
}