namespace AdventOfCode.Y2025;

public sealed class Day05 : IAdventDay
{
    public int Year => 2025;
    public int Day => 5;

    public AdventDaySolution Solve(string input)
    {
        var (ranges, ids) = ParseInput(input);

        var part1 = ids.Count(id => ranges.Any(r => r.Contains(id)));
        var part2 = 0L;

        var lastProcessed = 0L;

        foreach (var range in ranges.OrderBy(x => x.Min).ThenBy(x => x.Max))
        {
            var min = Math.Max(range.Min, lastProcessed + 1);

            if (min > range.Max)
                continue;

            part2 += (range with { Min = min }).GetSize();
            lastProcessed = range.Max;
        }

        return (part1, part2);
    }

    private static (List<Range> ranges, List<long> ids) ParseInput(string input)
    {
        var ranges = new List<Range>();
        var ids = new List<long>();

        foreach (var line in input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
        {
            if (line.Contains('-'))
            {
                ranges.Add(Range.Parse(line));
            }
            else
            {
                ids.Add(long.Parse(line));
            }
        }

        return (ranges, ids);
    }

    private sealed record Range(long Min, long Max)
    {
        public bool Contains(long value)
        {
            return value >= Min && value <= Max;
        }

        public static Range Parse(string line)
        {
            var parts = line.Split('-');
            var start = long.Parse(parts[0]);
            var end = long.Parse(parts[1]);
            return new Range(start, end);
        }

        public long GetSize()
        {
            return Max - Min + 1;
        }
    }
}
