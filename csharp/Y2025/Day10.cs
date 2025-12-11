using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public sealed record Instruction(string Line, int[][] Buttons, string TargetIndicatorLight, int[] JoltageRequirements)
    {
        public static Instruction Parse(string input)
        {
            var parts = input.Split(' ');
            var targetState = parts[0][1..^1];
            var joltageRequirements = parts[^1][1..^1].Split(',').Select(int.Parse).ToArray();
            var buttons = parts[1..^1].Select(b => b[1..^1].Split(',').Select(int.Parse).ToArray()).ToArray();
            return new Instruction(input, buttons, targetState, joltageRequirements);
        }
        
        public override string ToString() => Line;
    }
    
    public override AdventDaySolution Solve(string input)
    {
        var instructions = ParseInput(input);
        var (part1, part2) = (0, 0);
        
        var stopwatch = new Stopwatch();
        
        foreach (var instruction in instructions)
        {
            Console.WriteLine("Solving instruction: {0}", instruction);
            
            stopwatch.Start();
            var nextPart1 = CountFewestStepsForIndicatorLight(instruction);
            part1 += nextPart1;
            stopwatch.Stop();
            Console.WriteLine("Part 1: {0} (Time: {1})", nextPart1, stopwatch.Elapsed);
            stopwatch.Reset();
            
            stopwatch.Start();
            var nextPart2 = CountFewestStepsForJoltageMeter(instruction);
            part2 += nextPart2;
            stopwatch.Stop();
            Console.WriteLine("Part 2: {0} (Time: {1})", nextPart2, stopwatch.Elapsed);
            stopwatch.Reset();
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
        var queue = CreateQueue(Enumerable.Range(0, instruction.Buttons.Length).Select(i => (NextButton: i, State: emptyState, StepCount: 1)));
            
        while (queue.TryDequeue(out var current))
        {
            var nextButton = instruction.Buttons[current.NextButton];
            
            var nextState = GetNextIndicatorLight(current.State, nextButton);

            if (nextState == instruction.TargetIndicatorLight)
                return current.StepCount;

            for (var i = 0; i < instruction.Buttons.Length; i++)
            {
                if (current.NextButton == i)
                    continue;
                
                queue.Enqueue((i, nextState, current.StepCount + 1));
            }
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
        var prioritizedButtons = GetPrioritizedButtonsForTargetJoltage(instruction).Select(i => instruction.Buttons[i]).ToArray();
        var queue = CreateQueue(prioritizedButtons.Select(b => (NextButton: b, State: instruction.JoltageRequirements, StepCount: 1)));
        
        while (queue.TryDequeue(out var current))
        {
            var (canGet, isComplete) = TryGetNextJoltageMeter(current.NextButton, current.State, out var nextMeter);
            
            if (!canGet) continue;
            
            if (isComplete) return current.StepCount;
            
            foreach (var b in prioritizedButtons)
                queue.Enqueue((b, nextMeter, current.StepCount + 1));
        }
        
        return 0;
    }

    private static int[] GetPrioritizedButtonsForTargetJoltage(Instruction instruction)
    {
        // this doesn't really seem to help anything but leaving it here for now in case i think of a better way of prioritizing
        
        return instruction.Buttons
            .Index()
            .Select(bi => new
            {
                Button = bi.Index,
                MaxPushes = bi.Item.Min(i => instruction.JoltageRequirements[i])
            })
            .Where(x => x.MaxPushes != 0)
            .OrderBy(x => x.MaxPushes)
            .Select(x => x.Button)
            .ToArray();
    }
    
    private static (bool CanGet, bool IsComplete) TryGetNextJoltageMeter(int[] button, int[] current, out int[] next)
    {
        next = new int[current.Length];
        var isComplete = true;

        for (var i = 0; i < current.Length; i++)
        {
            next[i] = current[i] - (button.Contains(i) ? 1 : 0);

            if (next[i] > 0)
                isComplete = false;
            
            if (next[i] < 0)
                return (false, false);
        }

        return (true, isComplete);
    }
    
    private static Queue<T> CreateQueue<T>(IEnumerable<T> items) => new(items);
}
