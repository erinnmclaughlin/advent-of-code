namespace AdventOfCode.Y2024;

public sealed class Day19() : AdventDay(2024, 19)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        var towels = ParseTowels(lines[0]);

        var sum = 0L;

        foreach (var request in lines[2..])
        {
            if (CanMake(request, towels))
                sum++;
        }

        return (sum, "");
    }

    public static List<string> ParseTowels(string input)
    {
        return [.. input.Split(", ")];
    }

    public static IEnumerable<string> EnumerateCombos(string target, string current, List<string> options, HashSet<string> invalid)
    {
        foreach (var option in options)
        {
            var next = current + option;

            if (next == target)
                yield return next;

            if (!invalid.Contains(next) && target.StartsWith(next))
            {
                foreach (var combo in EnumerateCombos(target, next, options, invalid))
                    yield return combo;
            }

            invalid.Add(next);
        }
    }
    
    public static bool CanMake(string request, List<string> options)
    {
        var invalid = new HashSet<string>();
        return EnumerateCombos(request, "", options, invalid).Any();
    }
}
