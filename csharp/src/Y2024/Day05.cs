namespace AdventOfCode.Y2024;

public sealed class Day05() : AdventDay(2024, 5)
{
    public override AdventDaySolution Solve(string input)
    {
        var (numbers, comparer) = ParseInput(input);

        var part1 = numbers
            .Select(x => x.Split(','))
            .Where(x => IsOrdered(x, comparer))
            .Sum(x => int.Parse(x[x.Length / 2]));

        var part2 = numbers
            .Select(x => x.Split(','))
            .Where(x => !IsOrdered(x, comparer))
            .Select(x => x.Order(comparer).ToArray())
            .Sum(x => int.Parse(x[x.Length / 2]));

        return (part1, part2);
    }
    
    private static (string[], Comparer<string>) ParseInput(string input)
    {
        var lines = InputHelper.GetLines(input);
        var splitIndex = lines.IndexOf("");
        var numbers = lines[(splitIndex + 1)..].ToArray();
        var comparer = CreateComparer(lines[..splitIndex].ToArray());
        return (numbers, comparer);
    }

    private static bool IsOrdered(string[] items, Comparer<string> comparer)
        => items.SequenceEqual(items.Order(comparer));

    private static Comparer<string> CreateComparer(string[] rules)
        => Comparer<string>.Create((x, y) => rules.AsSpan().Contains($"{x}|{y}") ? - 1 : 1);
}
