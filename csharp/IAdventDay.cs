namespace AdventOfCode;

public interface IAdventDay
{
    int Year { get; }
    int Day  { get; }

    AdventDaySolution Solve(string input);
}
