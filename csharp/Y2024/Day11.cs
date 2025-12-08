namespace AdventOfCode.Y2024;

public sealed class Day11() : AdventDay(2024, 11)
{
    public override AdventDaySolution Solve(string input)
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
}
