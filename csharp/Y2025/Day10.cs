using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public sealed class Instruction
    {
        public string TargetState { get; }
        
        public Dictionary<string, int[]> Buttons { get; }
        public int[] JoltageRequirements { get; }

        private Instruction(string[] buttons, string targetState, int[] joltageRequirements)
        {
            Buttons = buttons.ToDictionary(b => b, b => b.Split(',').Select(int.Parse).ToArray());
            TargetState = targetState;
            JoltageRequirements = joltageRequirements;
        }
        
        public static Instruction Parse(string input)
        {
            var parts = input.Split(' ');
            var targetState = parts[0][1..^1];
            var joltageRequirements = parts[^1][1..^1].Split(',').Select(int.Parse).ToArray();
            var buttons = parts[1..^1].Select(b => b[1..^1]).ToArray();
            
            return new Instruction(buttons, targetState, joltageRequirements);
        }
    }
    
    public override AdventDaySolution Solve(string input)
    {
        var sum = ParseInput(input).Sum(CountFewestStepsForIndicatorLight);
        return (sum, "");
    }
    
    public static Instruction[] ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(Instruction.Parse)
        .ToArray();

    
    public static int CountFewestStepsForIndicatorLight(Instruction instruction)
    {
        var emptyState = new string('.', instruction.TargetState.Length);
            
        if (instruction.TargetState == emptyState) return 0;
        
        var queue = CreateQueue(instruction.Buttons.Keys.Select(b => (NextButton: b, State: emptyState, StepCount: 1)));
            
        while (queue.TryDequeue(out var current))
        {
            var nextState = GetNextIndicatorLight(current.State, instruction.Buttons[current.NextButton]);

            if (nextState == instruction.TargetState)
                return current.StepCount;
                
            foreach (var b in instruction.Buttons.Keys.Where(b => b != current.NextButton))
                queue.Enqueue((b, nextState, current.StepCount + 1));
        }
            
        // puzzle input should always be valid
        throw new InvalidOperationException("No solution found");
    }
    
    private static Queue<T> CreateQueue<T>(IEnumerable<T> items) => new(items);
    
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
}
