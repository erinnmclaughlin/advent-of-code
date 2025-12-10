using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public override AdventDaySolution Solve(string input)
    {
        var sum = ParseInput(input).Sum(i => i.CountFewestSteps());
        return (sum, "");
    }
    
    public static Instruction[] ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(Instruction.Parse)
        .ToArray();

    public sealed class Machine
    {
        public string IndicatorLights { get; private set; }
        public int[] Joltages { get; private init; }

        public Machine(Instruction instruction) : this(instruction.TargetState.Length)
        {
        }
        
        public Machine(int size)
        {
            IndicatorLights = new string('.', size);
            Joltages = new int[size];
        }
        
        public void PressButton(int[] button)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < IndicatorLights.Length; i++)
            {
                if (button.Contains(i))
                {
                    sb.Append(IndicatorLights[i] == '.' ? '#' : '.');
                    Joltages[i]++;
                }
                else
                {
                    sb.Append(IndicatorLights[i]);
                }
                
            }

            IndicatorLights = sb.ToString();
        }
    }
    
    public sealed class Instruction
    {
        public string EmptyState => new('.', TargetState.Length);
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
        
        public int CountFewestSteps()
        {
            if (TargetState == EmptyState) 
                return 0;

            var queue = CreateInitialQueue();
            
            while (queue.TryDequeue(out var next))
            {
                var (button, state, steps) = next;
                
                state = GetNextState(state, button);
                steps++;
                
                if (state == TargetState) 
                    return steps;
                
                foreach (var b in Buttons.Keys.Where(b => button != b))
                    queue.Enqueue((b, state, steps));
            }
            
            // puzzle input should always be valid
            throw new InvalidOperationException("No solution found");
        }

        private Queue<(string Button, string State, int Steps)> CreateInitialQueue()
        {
            var queue = new Queue<(string Button, string State, int Steps)>();

            foreach (var button in Buttons.Keys)
            {
                queue.Enqueue((button, EmptyState, 0));
            }

            return queue;
        }
        
        private string GetNextState(string current, string button)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < current.Length; i++)
            {
                if (Buttons[button].Contains(i))
                    sb.Append(current[i] == '.' ? '#' : '.');
                else
                    sb.Append(current[i]);
            }

            return sb.ToString();
        }
    }
}
