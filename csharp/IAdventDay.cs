namespace AdventOfCode;

public interface IAdventDay
{
    int Year { get; }
    int Day  { get; }

    AdventDaySolution Solve(string input);
}

public record AdventDaySolution(string Part1, string Part2)
{
    
    public static implicit operator AdventDaySolution((object? Part1, object? Part2) s) => new(s.Part1?.ToString() ?? string.Empty, s.Part2?.ToString() ?? string.Empty);
}
