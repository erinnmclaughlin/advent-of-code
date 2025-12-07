namespace AdventOfCode.Y2025;

public sealed class Day01() : AdventDay2025(1)
{
    public override AdventDaySolution Solve(string input)
    { 
        var (current, part1, part2) = (50, 0, 0);

        foreach (var line in input.Split('\n', StringSplitOptions.TrimEntries))
        {
            var increment = line[0] is 'L' ? -1 : 1;
            var ticks = int.Parse(line[1..]);

            for (var i = 0; i < ticks; i++)
            {
                current = (current + increment + 100) % 100;
                if (current == 0) part2++;
            }

            if (current == 0) part1++;
        }

        return (part1, part2);
    }
}