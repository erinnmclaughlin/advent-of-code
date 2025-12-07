namespace AdventOfCode.Y2025;

public abstract class AdventDay2025(int day) : IAdventDay
{
    public int Day { get; } = day;
    public int Year => 2025;

    public abstract (string Part1, string Part2) Solve(string input);
}
