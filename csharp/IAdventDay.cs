namespace AdventOfCode;

public interface IAdventDay
{
    int Year { get; }
    int Day  { get; }

    (string Part1, string Part2) Solve(string input);
}
