namespace AdventOfCode.Y2024;

public sealed class Day01() : AdventDay(2024, 1)
{
    public override AdventDaySolution Solve(string input)
    {
        var (left, right) = ParseFile(input);

        var part1 = left.Order().Zip(right.Order().ToArray(), (l, r) => Math.Abs(l - r)).Sum();
        var part2 = left.Sum(l => l * right.Count(r => r == l));

        return (part1, part2);
    }
    
    private static (int[] Left, int[] Right) ParseFile(string input)
    {
        var lines = InputHelper.GetLines(input);
        var (left, right) = (new int[lines.Length], new int[lines.Length]);

        for (var i = 0; i < lines.Length; i++)
        {
            var parts = lines[i].Split("   ");
            (left[i], right[i]) = (int.Parse(parts[0]), int.Parse(parts[1]));
        }

        return (left, right);
    }
}
