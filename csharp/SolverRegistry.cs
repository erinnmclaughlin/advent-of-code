namespace AdventOfCode;

public static class SolverRegistry
{
    private static readonly Dictionary<(int Year, int Day), AdventDay> _solvers;

    static SolverRegistry()
    {
        _solvers = typeof(Program)
            .Assembly
            .GetTypes()
            .Where(t => !t.IsAbstract && typeof(AdventDay).IsAssignableFrom(t))
            .Select(t => (AdventDay)Activator.CreateInstance(t)!)
            .ToDictionary(s => (s.Year, s.Day));
    }

    public static AdventDay Get(int year, int day) =>
        _solvers.TryGetValue((year, day), out var solver)
            ? solver
            : throw new InvalidOperationException($"No solver for {year} day {day}");
}
