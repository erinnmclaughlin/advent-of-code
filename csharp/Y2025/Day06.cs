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

    private static long SolvePartOne(string[] lines, char[] operators)
    {
        var sum = 0L;

        for (var i = 0; i < operators.Length; i++)
        {
            var op = operators[i];
            long? columnResult = null;

            foreach (var line in lines[..^1])
            {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var number = int.Parse(parts[i].Trim());
                columnResult = DoOperation(columnResult, number, op);
            }

            sum += columnResult ?? 0;
        }

        return sum;
    }

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
