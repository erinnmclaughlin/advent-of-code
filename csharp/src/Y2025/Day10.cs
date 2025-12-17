using System.Diagnostics;

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
        stopwatch.Start();
        
        foreach (var instruction in instructions)
        {
            part1 += CountFewestStepsForIndicatorLight(instruction);
            part2 += CountFewestStepsForJoltageMeter(instruction);
        }

        stopwatch.Stop();
        Console.WriteLine($"Puzzle solved in {stopwatch.ElapsedMilliseconds}ms");
        
        return (part1, part2);
    }
    
    public static Instruction[] ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(Instruction.Parse)
        .ToArray();
    
    // Part 1:
    public static int CountFewestStepsForIndicatorLight(Instruction instruction)
    {
        var emptyState = new string('.', instruction.TargetIndicatorLight.Length);
        var queue = CreateQueue(Enumerable.Range(0, instruction.Buttons.Length).Select(i => (NextButton: i, State: emptyState, StepCount: 1)));
            
        while (queue.TryDequeue(out var current))
        {
            var nextButton = instruction.Buttons[current.NextButton];
            
            var nextState = string.Join("", current.State.Select((c, i) => nextButton.Contains(i) ? c == '.' ? '#' : '.' : c));

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
    
    private static Queue<T> CreateQueue<T>(IEnumerable<T> items) => new(items);
    
    // Part 2:
    // Input takes 10-15 seconds to solve with this approach
        
    // Strategy:
    // When we reach the "target" state, each button will have been pushed either an odd number of times or an even
    // number of times. We also know that the total number of buttons is relatively small (my puzzle input has max 13
    // buttons). Since we know that the number of buttons is relatively small, we can test each combination without too
    // much explosion.
        
    // Steps:
    // First, we find all even/odd combinations. Then for each combination, we calculate the net effect of pushing the
    // "odd" buttons 1 time, and the "even" buttons 0 times. After that, to maintain the same even/odd combination
    // status, any additional button presses have to happen in pairs. And since every remaining button press has to be
    // done twice, the joltage meter has to contain all even numbers to be a state that can possibly be reached. SO, we
    // do the single-button press for odd buttons, and then test that the joltage meter is even. If not, move on.
    // Otherwise, we know the rest of the answer must be "doubled". So to get the rest of the answer, we divide the
    // joltage meter values by 2. At that point, we have a new target state, so we recurse.
    private static int CountFewestStepsForJoltageMeter(Instruction instruction)
    {
        var cache = new Dictionary<int[], int>(IntArrayComparer.Instance);
        return CountFewestStepsForJoltageMeter(instruction.Buttons, instruction.JoltageRequirements, cache);
    }
    
    private static int CountFewestStepsForJoltageMeter(int[][] buttons, int[] target, Dictionary<int[], int> cache)
    {
        if (cache.TryGetValue(target, out var cached))
            return cached;
        
        if (target.All(x => x == 0))
            return 0;

        var best = int.MaxValue;

        foreach (var chosenIndexes in EnumerateAllButtonCombinations(buttons))
        {
            // calculate the net change to the target
            var effect = GetEffect(buttons, chosenIndexes, target.Length);

            // if the effect doesn't result in all even numbers for the next state, then it's not a possible solution
            if (!Apply(effect, target, out var nextState))
                continue;

            var buttonPresses = chosenIndexes.Count;
            var nextButtonPresses = CountFewestStepsForJoltageMeter(buttons, nextState, cache);
            
            if (nextButtonPresses == int.MaxValue)
                continue;
            
            best = Math.Min(best, buttonPresses + 2 * nextButtonPresses);
        }

        cache[target] = best;
        return best;
    }
    
    private static IEnumerable<List<int>> EnumerateAllButtonCombinations(int[][] buttons)
    {
        var indices = Enumerable.Range(0, buttons.Length).ToArray();

        for (var k = 0; k <= buttons.Length; k++)
        {
            // choose indexes to make "odd"
            foreach (var chosenIndexes in EnumerateAllButtonCombinations(indices, k, 0, new List<int>(k)))
            {
                yield return chosenIndexes;
            }
        }
    }
    
    private static IEnumerable<List<int>> EnumerateAllButtonCombinations(
        int[] buttonIndexes,
        int numberToSelect,
        int startIndex,
        List<int> current)
    {
        if (numberToSelect == 0)
        {
            yield return current;
            yield break;
        }

        for (var i = startIndex; i <= buttonIndexes.Length - numberToSelect; i++)
        {
            current.Add(buttonIndexes[i]);

            foreach (var combo in EnumerateAllButtonCombinations(buttonIndexes, numberToSelect - 1, i + 1, [..current]))
                yield return combo;

            current.RemoveAt(current.Count - 1);
        }
    }
    
    private static int[] GetEffect(int[][] buttons, List<int> buttonIndexes, int targetLength)
    {
        var effect = new int[targetLength];

        foreach (var i in buttonIndexes)
        foreach (var counter in buttons[i])
            effect[counter]++;

        return effect;
    }

    private static bool Apply(int[] effect, int[] target, out int[] nextState)
    {
        nextState = new int[target.Length];
        
        for (var i = 0; i < effect.Length; i++)
        {
            var diff = target[i] - effect[i];
            if (diff < 0 || diff % 2 != 0)
                return false;

            nextState[i] = diff / 2;
        }

        return true;
    }
    
    public sealed class IntArrayComparer : IEqualityComparer<int[]>
    {
        public static IntArrayComparer Instance { get; } = new();
        
        private IntArrayComparer() { }
        
        public bool Equals(int[]? x, int[]? y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
       
            return x.Length == y.Length && x.AsSpan().SequenceEqual(y.AsSpan());
        }

        public int GetHashCode(int[] obj)
        {
            var hc = new HashCode();
            hc.Add(obj.Length);
            foreach (var i in obj)
                hc.Add(i);
            return hc.ToHashCode();
        }
    }
}
