namespace AdventOfCode;

public record AdventDaySolution(string Part1, string Part2)
{
    
    public static implicit operator AdventDaySolution((object? Part1, object? Part2) s) => new(s.Part1?.ToString() ?? string.Empty, s.Part2?.ToString() ?? string.Empty);
}
