namespace AdventOfCode.Y2025;

public sealed class Day06() : AdventDay(2025, 6)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        var operators = lines[^1].Where(c => c is not ' ').ToArray();

        var part1 = SolvePartOne(lines, operators);
        var part2 = SolvePartTwo(lines, operators);

        return (part1, part2);
    }

    private static long SolvePartOne(string[] lines, char[] operators) => operators
        .Select((t, i) => 
            lines[..^1]
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Select(parts => int.Parse(parts[i].Trim()))
                .Aggregate<int, long?>(null, (current, number) => DoOperation(current, number, t)) ?? 0)
        .Sum();

    private static long SolvePartTwo(string[] lines, char[] operators)
    {
        var sum = 0L;
        var stack = new Stack<char>(operators);

        long? columnResult = null;
        var op = stack.Pop();

        for (var i = lines[0].Length - 1; i >= 0; i--)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var line in lines[..^1])
            {
                if (int.TryParse(line[i].ToString(), out var number))
                {
                    sb.Append(number);
                }
            }

            if (sb.Length == 0)
            {
                sum += columnResult ?? 0L;
                columnResult = null;
                op = stack.Pop();
            }
            else
            {
                var parsedNumber = long.Parse(sb.ToString());
                columnResult = DoOperation(columnResult, parsedNumber, op);
            }
        }

        return sum + columnResult ?? 0L;
    }

    private static long DoOperation(long? num1, long num2, char op) => num1 is null ? num2 : op switch
    {
        '+' => num1.Value + num2,
        '*' => num1.Value * num2,
        _ => throw new NotSupportedException()
    };
}
