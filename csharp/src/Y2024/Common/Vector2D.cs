namespace AdventOfCode.Y2024;

public readonly struct Vector2D(int x, int y) : IEquatable<Vector2D>
{
    public int X { get; } = x;
    public int Y { get; } = y;
    
    public static Vector2D Zero => new(0, 0);
    
    public static Vector2D operator +(Vector2D a, Vector2D b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2D operator -(Vector2D a, Vector2D b) => new(a.X - b.X, a.Y - b.Y);

    public bool Equals(Vector2D other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(Vector2D left, Vector2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2D left, Vector2D right)
    {
        return !(left == right);
    }

    public static implicit operator Vector2D(Direction direction) => direction switch
    {
        Direction.Up => new Vector2D(0, -1),
        Direction.Right => new Vector2D(+1, 0),
        Direction.Down => new Vector2D(0, 1),
        Direction.Left => new Vector2D(-1, 0),
        _ => throw new ArgumentOutOfRangeException(nameof(direction))
    };
}