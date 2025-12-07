namespace AdventOfCode.Extensions;

public static class InputExtensions
{
    public static string[] ReadNonEmptyLines(this string input) =>
        input.Split(
            '\n',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
