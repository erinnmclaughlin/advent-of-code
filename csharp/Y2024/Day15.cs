namespace AdventOfCode.Y2024;

public sealed class Day15() : AdventDay(2024, 15)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input).AsSpan();

        var part1 = SolvePartOne(lines);
        var part2 = SolvePartTwo(lines);

        return (part1, part2);
    }

    private static long SolvePartOne(ReadOnlySpan<string> span)
    {
        var splitIndex = span.IndexOf(string.Empty);
        var world = CreatePartOneWorld(span[..splitIndex]);

        for (var i = splitIndex; i < span.Length; i++)
        {
            foreach (var dir in span[i].Select(ParseInstruction))
            {
                world.TryMove(world.Robot, dir);
            }
        }

        return world.MapObjects.OfType<Box>().Sum(World.GetGpsLocation);
    }

    private static long SolvePartTwo(ReadOnlySpan<string> span)
    {
        var splitIndex = span.IndexOf(string.Empty);
        var world = CreatePartTwoWorld(span[..splitIndex]);
        
        for (var i = splitIndex; i < span.Length; i++)
        {
            foreach (var dir in span[i].Select(ParseInstruction))
            {
                world.TryMove(world.Robot, dir);
            }
        }

        return world.MapObjects.OfType<Box>().Sum(World.GetGpsLocation);
    }

    private static World CreatePartOneWorld(ReadOnlySpan<string> input)
    {
        var world = new World();
        
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y].AsSpan();
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                    world.MapObjects.Add(new Wall { Position = new Vector2D(x, y) });
                if (line[x] == 'O')
                    world.MapObjects.Add(new Box { Position = new Vector2D(x, y) });
                if (line[x] == '@')
                    world.Robot.Position = new Vector2D(x, y);
            }
        }

        return world;
    }

    public static World CreatePartTwoWorld(ReadOnlySpan<string> input)
    {
        var world = new World();
        
        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y].AsSpan();
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                    world.MapObjects.Add(new Wall { Position = new Vector2D(x * 2, y), Width = 2 });

                else if (line[x] == 'O')
                    world.MapObjects.Add(new Box { Position = new Vector2D(x * 2, y), Width = 2 });

                else if (line[x] == '@')
                    world.Robot.Position = new Vector2D(x * 2, y);
            }
        }

        return world;
    }
    
    public sealed class World
    {
        public HashSet<MapObject> MapObjects { get; set; } = [];
        public Robot Robot { get; set; } = new();

        public bool TryMove(MapObject target, Vector2D dir) => TryMove(target, dir, []);
        
        public bool TryMove(MapObject target, Vector2D dir, List<MapObject> affected)
        {
            if (!target.Movable) return false;
            
            target.Position += dir;
            affected.Add(target);
            
            var collidingObjects = MapObjects.Where(x => x.CollidesWith(target)).ToList();

            var canMove = true;
            
            foreach (var collidingObject in collidingObjects)
            {
                if (!TryMove(collidingObject, dir, affected))
                {
                    canMove = false;
                    break;
                }
            }

            if (!canMove)
            {
                for (var i = affected.Count - 1; i >= 0; i--)
                {
                    affected[i].Position -= dir;
                    affected.RemoveAt(i);
                }
            }
            
            return canMove;
        }

        public static long GetGpsLocation(MapObject target) => 100 * target.Position.Y + target.Position.X;
    }
    
    public abstract class MapObject
    {
        public int Height { get; set; } = 1;
        public int Width { get; set; } = 1;
        public Vector2D Position { get; set; } = new(0,0);
        public virtual bool Movable { get; set; } = true;

        private IEnumerable<Vector2D> EnumerateCoordinates => Enumerable
            .Range(Position.Y, Height)
            .SelectMany(y => Enumerable.Range(Position.X, Width).Select(x => new Vector2D(x, y)));

        public bool CollidesWith(MapObject other) => other != this && EnumerateCoordinates.Any(other.EnumerateCoordinates.Contains);
    }

    public sealed class Box : MapObject;
    public sealed class Robot : MapObject;
    public sealed class Wall : MapObject
    {
        public override bool Movable { get; set; } = false;
    }
    
    public static Vector2D ParseInstruction(char c) => c switch
    {
        '>' => new Vector2D(+1,+0),
        '<' => new Vector2D(-1,+0),
        '^' => new Vector2D(+0,-1),
        'v' => new Vector2D(+0,+1),
        _ => throw new Exception($"'{c}' is not a recognized instruction.")
    };
}
