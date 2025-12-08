using System.Diagnostics;

namespace AdventOfCode.Y2024;

public sealed class Day06() : AdventDay(2024, 6)
{
    public override AdventDaySolution Solve(string input)
    {
        var map = InputHelper.GetLines(input).AsSpan();
        var (x, y) = GetStartingPos(ref map);

        var part1 =  GetPath(ref map, x, y);
        var part2 = 0;

         _ = TryGetPath(ref map, x, y, out var mainPath);

        foreach (var point in mainPath.DistinctBy(p => (p.x,p.y)).Skip(1))
        {
            var originalRow = map[point.x];
            var rowChars = originalRow.ToCharArray();
            rowChars[point.y] = '#';
            
            map[point.x] = string.Join("", rowChars); // todo: avoid all this stringy stuff
            
            if (!TryGetPath(ref map, x, y, out _ ))
                part2++;
            
            map[point.x] = originalRow;
        }

        return (part1, part2);
    }

     private static int GetPath(ref Span<string> map, int x, int y)
    {
        var (facing, nextX, nextY) = (Facing.Up, x, y);
        var visited = new HashSet<(int, int)> { (nextX,nextY) };
        
        while (true)
        {
            (nextX, nextY) = facing switch
            {
                Facing.Up =>    (x - 1, y    ),
                Facing.Right => (x    , y + 1),
                Facing.Down =>  (x + 1, y    ),
                Facing.Left =>  (x,     y - 1),
                _ => throw new UnreachableException()
            };
            
            if (nextX < 0 || nextX >= map.Length || nextY < 0 || nextY >= map[0].AsSpan().Length)
                break;

            if (map[nextX].AsSpan()[nextY] == '#')
            {
                facing = facing == Facing.Left ? Facing.Up : facing + 1;
                continue;
            }

            visited.Add((nextX, nextY));
            (x, y) = (nextX, nextY);
        }
        
        return visited.Count;
    }
    
    private static bool TryGetPath(ref Span<string> map, int x, int y, out HashSet<(Facing dir, int x, int y)> path)
    {
        var (facing, nextX, nextY) = (Facing.Up, x, y);
        path = new HashSet<(Facing, int, int)> { (facing,nextX,nextY) };

        while (true)
        {
            (nextX, nextY) = facing switch
            {
                Facing.Up =>    (x - 1, y    ),
                Facing.Right => (x    , y + 1),
                Facing.Down =>  (x + 1, y    ),
                Facing.Left =>  (x,     y - 1),
                _ => throw new UnreachableException()
            };
            
            if (nextX < 0 || nextX >= map.Length || nextY < 0 || nextY >= map[0].AsSpan().Length)
                break;

            if (map[nextX].AsSpan()[nextY] == '#')
            {
                facing = facing == Facing.Left ? Facing.Up : facing + 1;
                continue;
            }

            if (!path.Add((facing, nextX, nextY)))
                return false;
            
            (x, y) = (nextX, nextY);
        }

        return true;
    }
    
    private static (int, int) GetStartingPos(ref Span<string> map)
    { 
        for (var i = 0; i < map.Length; i++)
        {
            var index = map[i].IndexOf('^');
            
            if (index != -1)
                return (i, index);
        }
        
        throw new Exception("Could not find start pos");
    }
    
    private enum Facing : byte
    {
        Up,
        Right,
        Down,
        Left
    }
}
