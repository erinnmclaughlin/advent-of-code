namespace AdventOfCode.Y2025;

public sealed class Day03 : IAdventDay
{
    public int Year => 2025;
    public int Day => 3;

    public AdventDaySolution Solve(string input)
    {
        var digits = ParseInput(input).ToList();
        var part1 = digits.Sum(d => Solve(d, 2));
        var part2 = digits.Sum(d => Solve(d, 12));
        return (part1, part2);
    }

    private IEnumerable<List<(int Value, int Position)>> ParseInput(string input) => InputHelper
       .GetLines(input)
       .Select(line => line.Select((c, i) => (int.Parse(c.ToString()), i)).ToList());

    private static long Solve(List<(int Value, int Position)> digits, int targetDigitCount)
    {
        var selectedDigits = new List<int>();
        var startingPosition = 0;

        while (selectedDigits.Count < targetDigitCount)
        {
            var remainingDigitCount = targetDigitCount - selectedDigits.Count;

            var max = digits
                .Where(x => x.Position >= startingPosition && x.Position < digits.Count - remainingDigitCount + 1)
                .MaxBy(x => x.Value);

            if (max == default)
                break;

            startingPosition = max.Position + 1;
            selectedDigits.Add(max.Value);
        }

        return long.Parse(string.Join("", selectedDigits));
    }
}
