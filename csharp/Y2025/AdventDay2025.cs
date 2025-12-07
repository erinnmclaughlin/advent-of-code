namespace AdventOfCode.Y2025;

public abstract class AdventDay2025(int day) : IAdventDay
{
    public int Day { get; } = day;
    public int Year => 2025;

    public abstract AdventDaySolution Solve(string input);
}
