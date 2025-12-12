using System.Diagnostics;

namespace AdventOfCode.Y2024;

public enum Direction : byte
{
    Up,
    Right,
    Down,
    Left
}

public static class Directions
{
    public static IEnumerable<Direction> EnumerateClockwise() => EnumerateClockwise(Direction.Up);
    
    public static IEnumerable<Direction> EnumerateClockwise(Direction startingDirection)
    {
        var dir = startingDirection;
        yield return dir;
        
        for (var i = 0; i < 3; i++)
        {
            yield return dir = dir switch
            {
                Direction.Up => Direction.Right,
                Direction.Right => Direction.Down,
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                _ => throw new UnreachableException()
            };
        }
    }

    public static Direction GetOpposite(this Direction dir) => dir switch
    {
        Direction.Up => Direction.Down,
        Direction.Down => Direction.Up,
        Direction.Left => Direction.Right,
        Direction.Right => Direction.Left,
        _ => throw new UnreachableException()
    };
}