using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public sealed record Instruction(Dictionary<string, int[]> Buttons, string TargetIndicatorLight, int[] JoltageRequirements)
    {
        public static Instruction Parse(string input)
        {
            var parts = input.Split(' ');
            var targetState = parts[0][1..^1];
            var joltageRequirements = parts[^1][1..^1].Split(',').Select(int.Parse).ToArray();
            var buttons = parts[1..^1].Select(b => b[1..^1]);
            var buttonLookup = buttons.ToDictionary(b => b, b => b.Split(',').Select(int.Parse).ToArray());
            return new Instruction(buttonLookup, targetState, joltageRequirements);
        }
    }
    
    public override AdventDaySolution Solve(string input)
    {
        var instructions = ParseInput(input);
        var (part1, part2) = (0, 0);

        foreach (var instruction in instructions)
        {
            part1 += CountFewestStepsForIndicatorLight(instruction);
            part2 += CountFewestStepsForJoltageMeter(instruction);
        }
        
        return (part1, part2);
    }
    
    public static Instruction[] ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(Instruction.Parse)
        .ToArray();

    
    public static int CountFewestStepsForIndicatorLight(Instruction instruction)
    {
        var emptyState = new string('.', instruction.TargetIndicatorLight.Length);
        var queue = CreateQueue(instruction.Buttons.Keys.Select(b => (NextButton: b, State: emptyState, StepCount: 1)));
            
        while (queue.TryDequeue(out var current))
        {
            var nextState = GetNextIndicatorLight(current.State, instruction.Buttons[current.NextButton]);

            if (nextState == instruction.TargetIndicatorLight)
                return current.StepCount;
                
            foreach (var b in instruction.Buttons.Keys.Where(b => b != current.NextButton))
                queue.Enqueue((b, nextState, current.StepCount + 1));
        }
            
        // puzzle input should always be valid
        throw new InvalidOperationException("No solution found");
    }
    
    private static string GetNextIndicatorLight(string current, int[] button)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < current.Length; i++)
        {
            if (button.Contains(i))
                sb.Append(current[i] == '.' ? '#' : '.');
            else
                sb.Append(current[i]);
        }

        return sb.ToString();
    }

    public static int CountFewestStepsForJoltageMeter(Instruction instruction)
    {
        var lastId = 0;
        var emptyState = new int[instruction.JoltageRequirements.Length];
        var stateLookup = new Dictionary<int, int[]> { [0] = emptyState };
        var clientLookup = new Dictionary<int, int> { [0] = instruction.Buttons.Count };
        var reusableIds = new Stack<int>();
        var queue = CreateQueue(instruction.Buttons.Keys.Select(b => (NextButton: b, StateId: 0, StepCount: 1)));
        
        while (queue.TryDequeue(out var current))
        {
            var state = stateLookup[current.StateId];
            clientLookup[current.StateId]--;
            
            if (clientLookup[current.StateId] == 0)
            {
                reusableIds.Push(current.StateId);
            }
            
            if (!TryGetNextJoltageMeter(
                    instruction.Buttons[current.NextButton],
                    state,
                    instruction.JoltageRequirements,
                    out var nextMeter))
                continue;
            
            if (nextMeter.SequenceEqual(instruction.JoltageRequirements))
                return current.StepCount;

            var nextId = reusableIds.TryPop(out var id) ? id : ++lastId;
            clientLookup[nextId] = instruction.Buttons.Count;
            stateLookup[nextId] = nextMeter;
            
            foreach (var b in instruction.Buttons.Keys)
                queue.Enqueue((b, nextId, current.StepCount + 1));
        }
        
        return 0;
    }

    private static bool TryGetNextJoltageMeter(int[] button, int[] current, int[] required, out int[] next)
    {
        next = new int[current.Length];

        for (var i = 0; i < current.Length; i++)
        {
            next[i] = current[i] + (button.Contains(i) ? 1 : 0);

            if (next[i] > required[i])
                return false;
        }

        return true;
    }
    
    private static Queue<T> CreateQueue<T>(IEnumerable<T> items) => new(items);
}
