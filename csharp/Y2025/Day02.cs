namespace AdventOfCode.Y2025;

public sealed class Day02() : AdventDay(2025, 2)
{
    public override AdventDaySolution Solve(string input)
    { 
        var (part1, part2) = (0L, 0L);

        foreach (var (min, max) in ParseInput(input))
        {
            for (var id = min; id <= max; id++)
            {
                var idString = id.ToString();

                if (idString.Length < 2)
                    continue;

                if (!IsValid(idString, 2))
                {
                    part1 += id;
                    part2 += id;
                    continue;
                }

                if (Enumerable.Range(2, idString.Length - 1).Any(m => !IsValid(idString, m)))
                    part2 += id;
            }
        }

        return (part1, part2);
    }

    private static bool IsValid(string idString, int numParts)
    {
        if (idString.Length == 0 || idString.Length % numParts != 0)
            return true;

        var size = idString.Length / numParts;
        var part = idString[..size];
        var invalid = string.Join("", Enumerable.Repeat(part, numParts));

        return !idString.Equals(invalid);
    }

    private static IEnumerable<(long MinValue, long MaxValue)> ParseInput(string input) => input
        .Split(',')
        .Select(line => line.Split('-'))
        .Select(parts => (long.Parse(parts[0]), long.Parse(parts[1])));
}