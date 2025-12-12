namespace AdventOfCode;

public static class InputHelper
{
    public static string[] GetLines(string input) => input.Split(["\r\n", "\n"], StringSplitOptions.None);
    public static string[] GetNonEmptyLines(string input) => input.Split(["\r\n", "\n"], StringSplitOptions.RemoveEmptyEntries);
}