using AdventOfCode;

if (args.Length < 2)
{
    Console.Error.WriteLine("Usage: aoc <year> <day>");
    return;
}

if (!int.TryParse(args[0], out var year) ||
    !int.TryParse(args[1], out var day))
{
    Console.Error.WriteLine("Year and day must be integers.");
    return;
}

// Resolve solver
AdventDay solver;
try
{
    solver = SolverRegistry.Get(year, day);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return;
}

// Read entire input from stdin
var input = await Console.In.ReadToEndAsync();

if (string.IsNullOrWhiteSpace(input))
{
    Console.Error.WriteLine("No input provided on stdin.");
    return;
}

// Solve
var (part1, part2) = solver.Solve(input.Trim());

// Output (AoC-style)
Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
