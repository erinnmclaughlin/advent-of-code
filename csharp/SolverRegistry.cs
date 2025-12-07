namespace AdventOfCode;

public static class SolverRegistry
{
    private static readonly Dictionary<(int Year, int Day), IAdventDay> _solvers;

    static SolverRegistry()
    {
        _solvers = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .SelectMany(asm => asm.GetTypes())
            .Where(t => !t.IsAbstract && typeof(IAdventDay).IsAssignableFrom(t))
            .Select(t => (IAdventDay)Activator.CreateInstance(t)!)
            .ToDictionary(s => (s.Year, s.Day));
    }

    public static IAdventDay Get(int year, int day) =>
        _solvers.TryGetValue((year, day), out var solver)
            ? solver
            : throw new InvalidOperationException($"No solver for {year} day {day}");
}
