namespace AdventOfCode;

public abstract class AdventDay(int year, int day) : IAdventDay
{
    public int Day { get; } = day;
    public int Year { get; } = year;

    public abstract AdventDaySolution Solve(string input);
}
