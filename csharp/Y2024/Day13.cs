namespace AdventOfCode.Y2024;

using Button = (long X, long Y);
using Target = (long X, long Y);

public sealed class Day13() : AdventDay(2024, 13)
{
    public override AdventDaySolution Solve(string input)
    {
        var fileLines = InputHelper.GetLines(input).AsSpan();
        var part1 = SolvePartOne(fileLines);
        var part2 = SolvePartTwo(fileLines);
        return (part1, part2);
    }
    
    private long SolvePartOne(ReadOnlySpan<string> fileLines)
    {
        long sum = 0;
        
        for (var i = 0; i < fileLines.Length; i += 4)
        {
            var result = GetCounts(fileLines.Slice(i, 3));
            if (result is null || result.Value.A > 100 || result.Value.B > 100) continue;

            sum += 3 * result.Value.A + result.Value.B;
        }

        return sum;
    }

    private static long SolvePartTwo(ReadOnlySpan<string> fileLines)
    {
        long sum = 0;
        
        for (var i = 0; i < fileLines.Length; i+=4)
        {
            var result = GetCounts(fileLines.Slice(i,3), 10000000000000);
            if (result is null) continue;

            sum += 3 * result.Value.A + result.Value.B;
        }

        return sum;
    }

    private static Button ParseButtonText(ReadOnlySpan<char> input)
    {
        // Format: "Button A: X+94, Y+34"

        var trimmed = input[11..];
        var x = trimmed[..trimmed.IndexOf(',')];
        var y = trimmed[trimmed.LastIndexOf('+')..];

        return (int.Parse(x), int.Parse(y));
    }

    private static Target ParseTargetText(ReadOnlySpan<char> input, long padding = 0)
    {
        // Format: "Prize: X=8400, Y=5400"
        
        var trimmed = input[9..];
        var x = trimmed[..trimmed.IndexOf(',')];
        var y = trimmed[(trimmed.LastIndexOf('=') + 1)..];

        return (long.Parse(x) + padding, long.Parse(y) + padding);
    }
    

/*
    we know...
    a.Count * a.X + b.Count * b.X = t.X
    a.Count = (t.X - b.Count * b.X) / a.X

    and...
    a.Count * a.Y + b.Count * b.Y = t.Y
    a.Count = (t.Y - b.Count * b.Y) / a.Y

    so substitute & solve (in this case, solve for b.Count)...
    (t.X - b.Count * b.X) / a.X = (t.Y - b.Count * b.Y) / a.Y
    (t.X - b.Count * b.X) * a.Y  = (t.Y - b.Count * b.Y) * a.X
    (t.X * a.Y) - (b.Count * b.X * a.Y) = (t.Y * a.X) - (b.Count * b.Y *  a.X)
    (b.Count * b.Y *  a.X) - (b.Count * b.X * a.Y) = (t.Y * a.X) - (t.X * a.Y)
    b.Count * ((b.Y * a.X) - (b.X * a.Y)) = (t.Y * a.X) - (t.X * a.Y)
    b.Count = ((t.Y * a.X) - (t.X * a.Y)) / ((b.Y * a.X) - (b.X * a.Y))
 */
    private static (long A, long B)? GetCounts(ReadOnlySpan<string> input, long targetPadding = 0)
    {
        var a = ParseButtonText(input[0]);
        var b = ParseButtonText(input[1]);
        var t = ParseTargetText(input[2], targetPadding);
        
        var bCount =  (a.Y * t.X - a.X * t.Y) / (a.Y * b.X - a.X * b.Y);
        var aCount = (t.X - bCount * b.X) / a.X;

        if (aCount * a.Y + bCount * b.Y != t.Y || aCount< 0 || bCount < 0)
            return null;
        
        return (aCount, bCount);
    }
}
