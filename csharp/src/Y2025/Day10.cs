using System.Collections;
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
        var (part1, part2) = (0L, 0L);
        
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
            var nextPart2 = CountFewestStepsForJoltageMeter_WithRecursion(instruction);
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

    public static long CountFewestStepsForJoltageMeter_WithRecursion(Instruction instruction)
    {
        var memory = new Dictionary<int, long>();
        var best = long.MaxValue;
        var prioritizedButtons = instruction.Buttons
            .OrderBy(b => GetMaxPushes(b, instruction.JoltageRequirements))
            .ToArray();
        
        foreach (var button in prioritizedButtons)
        {
            var count = CountFewestStepsForJoltageMeter(
                prioritizedButtons,
                new PartTwoState(instruction.JoltageRequirements, button),
                0,
                ref best,
                memory);
            
            if (count != -1)
                best = Math.Min(best, count);
        }

        return best;
    }
    
    private static long CountFewestStepsForJoltageMeter(
        int[][] prioritizedButtons,
        PartTwoState current,
        long stepCount,
        ref long knownBest, 
        Dictionary<int, long> memory)
    {
        if (stepCount > knownBest)
            return -1;

        if (memory.TryGetValue(current.Hash, out var count))
        {
            return count == -1 ? -1 : stepCount + count + 1;
        }
        
        var (canGet, isComplete) = TryGetNextJoltageMeter(current.NextButton, current.JoltageMeter, out var nextMeter);

        if (!canGet)
        {
            memory[current.Hash] = -1;
            return -1;
        }

        if (isComplete)
        {
            knownBest = stepCount;
            memory[current.Hash] = stepCount;
            return stepCount;
        }
        
        stepCount++;

        var min = -1L;

        foreach (var button in prioritizedButtons)
        {
            var nextCount = CountFewestStepsForJoltageMeter(prioritizedButtons, new(nextMeter, button), stepCount, ref knownBest, memory);
            if (min == -1 || nextCount < min)
                min = nextCount;
        }

        memory[current.Hash] = min;
        return min;
    }
    
    public static int CountFewestStepsForJoltageMeter_WithQueue(Instruction instruction, HashSet<int>? seenStates = null)
    {
        seenStates ??= [];
        var prioritizedButtons = instruction.Buttons.Index().OrderBy(bi => GetMaxPushes(bi.Item, instruction.JoltageRequirements)).Select(x => x.Item).ToArray();
        var queue = CreateQueue(prioritizedButtons.Select(b => (State: new PartTwoState(instruction.JoltageRequirements, b), StepCount: 1)));
        
        while (queue.TryDequeue(out var current))
        {
            if (!seenStates.Add(current.State.Hash))
                continue;
            
            var (canGet, isComplete) = TryGetNextJoltageMeter(current.State.NextButton, current.State.JoltageMeter, out var nextMeter);

            if (!canGet)
            {
                continue;
            }

            if (isComplete)
            {
                return current.StepCount;
            }

            foreach (var b in prioritizedButtons)
            {
                queue.Enqueue((new PartTwoState(nextMeter, b), current.StepCount + 1));
            }
        }

        return -1;
    }
    
    private static int GetMaxPushes(int[] button, int[] joltages)
    {
        return button.Min(i => joltages[i]);
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

    private sealed class PartTwoState
    {
        public int[] JoltageMeter { get; }
        public int[] NextButton { get; }
        public int Hash { get; }
        
        public PartTwoState(int[] joltageMeter, int[] nextButton)
        {
            JoltageMeter = joltageMeter;
            NextButton = nextButton;
            Hash = HashCode.Combine(
                StructuralComparisons.StructuralEqualityComparer.GetHashCode(nextButton),
                StructuralComparisons.StructuralEqualityComparer.GetHashCode(joltageMeter)
            );
        }
    }
}
