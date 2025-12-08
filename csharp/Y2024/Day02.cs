namespace AdventOfCode.Y2024;

public sealed class Day02 : IAdventDay
{
    public int Year => 2024;
    public int Day => 2;

    public AdventDaySolution Solve(string input)
    {
        var lines = ParseInput(input);

        // TODO: implement puzzle logic here

        var part1 = lines.Count(IsSafe);
        var part2 = lines.Count(levels => IsSafe(levels) || levels.Any((_, i) => IsSafeWithoutElement(levels, i)));

        return (part1, part2);
    }
    
    private static IEnumerable<int[]> ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(l => l.Split(' ').Select(int.Parse).ToArray());
    
    private static bool IsSafe(int[] levels)
    {
        var shouldDescend = levels[1] < levels[0];

        for (var i = 0; i < levels.Length - 1; i++)
        {
            var (current, next) = (levels[i], levels[i + 1]);
            if (Math.Abs(next - current) is < 1 or > 3 || shouldDescend != next < current) 
                return false;
        }

        return true;
    }
    
    private static bool IsSafeWithoutElement(int[] levels, int index)
    {
        return IsSafe(levels.Where((_, i) => i != index).ToArray());
    }
}

file static class Extensions
{
    public static bool Any<T>(this IEnumerable<T> source, Func<T, int, bool> criteria) => source
        .Select((item, index) => (item, index))
        .Any(r => criteria(r.item, r.index));

}