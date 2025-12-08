using System.Text;

namespace AdventOfCode.Y2024;

public sealed class Day11 : IAdventDay
{
    public int Year => 2024;
    public int Day => 11;

    public AdventDaySolution Solve(string input)
    {
        var part1 = CountStones(input, 25);
        var part2 = CountStones(input, 75);
        return (part1, part2);
    }

    public static long CountStones(string input, int numberOfRounds) =>  Enumerable
        .Range(0, numberOfRounds)
        .AsParallel()
        .Aggregate(
            input.Split(' ').Select(n => new Stone(n)).ToList(),
            (current, _) => current
                .SelectMany(Blink)
                .GroupBy(s => s.Number)
                .Select(s =>
                {
                    var stone = s.First();
                    stone.Count += s.Skip(1).Sum(x => x.Count);
                    return stone;
                })
                .ToList()
        )
        .Sum(s => s.Count);

    private static IEnumerable<Stone> Blink(Stone stone)
    {
        if (stone.Number is "" or "0")
        {
            stone.Number = "1";
        }
        else if (stone.Number.Length % 2 == 0)
        {
            yield return new Stone(stone.Number[..(stone.Number.Length / 2)]) { Count = stone.Count };
            stone.Number = stone.Number[(stone.Number.Length / 2)..].TrimStart('0');
        }
        else
        {
            stone.Number = (long.Parse(stone.Number) * 2024).ToString();
        }

        yield return stone;
    }
    
    private sealed class Stone(string number)
    {
        public string Number { get; set; } = number;
        public long Count { get; set; } = 1;
    }
    
    // original solution; worked for part one but way too slow for part two
    public static long CountStonesSlow(ReadOnlySpan<char> span, int numberOfRounds)
    {
        for (var i = 0; i < numberOfRounds; i++)
        {
            var newSpan = new StringBuilder();
            var split = span.Split(' ');
            while (split.MoveNext())
            {
                var (offset, length) = split.Current.GetOffsetAndLength(span.Length);
                var number = span.Slice(offset, length);

                if (number is "0")
                {
                    newSpan.Append("1 ");
                }
                else if (number.Length % 2 == 0)
                {
                    var n1 = number[..(number.Length / 2)];
                    newSpan.Append(n1).Append(' ');
                    var n2 = number[(number.Length / 2)..].TrimStart('0');
                    newSpan.Append(n2.IsEmpty ? "0" : n2).Append(' ');
                }
                else
                {
                    newSpan.Append(long.Parse(number) * 2024).Append(' ');
                }
            }

            span = newSpan.ToString().AsSpan().TrimEnd(' ');
        }

        return span.ToString().Split(' ').Distinct().Count();
        //return span.Count(' ') + 1;
    }
}
