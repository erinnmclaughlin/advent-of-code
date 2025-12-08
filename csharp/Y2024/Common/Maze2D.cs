namespace AdventOfCode.Y2024;

public class Maze2D(int height, int width)
{
    public int Height { get; } = height;
    public int Width { get; } = width;

    public HashSet<Vector2D> Walls { get; } = [];

    public bool IsOpen(Vector2D position) => 
        position.X >= 0 && position.X < Width &&
        position.Y >= 0 && position.Y < Height &&
        !Walls.Contains(position);

    public IEnumerable<(Direction Direction, Vector2D Position)> EnumerateOpenAdjacentPaths(Vector2D position) => Directions
        .EnumerateClockwise()
        .Select(x => (Direction: x, Position: position + x))
        .Where(x => IsOpen(x.Position));
}
