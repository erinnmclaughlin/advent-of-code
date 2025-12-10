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
    
    public sealed class Instruction
    {
        public string TargetState { get; }
        public string[] Buttons { get; }
        public int[] JoltageRequirements { get; }

        public Instruction(string[] buttons, string targetState, int[] joltageRequirements)
        {
            Buttons = buttons;
            TargetState = targetState;
            JoltageRequirements = joltageRequirements;
        }

        public int CountFewestSteps()
        {
            var initialLightState = new string('.', TargetState.Length);
            var initialPressedState = Buttons.ToDictionary(b => b, _ => 0);

            var lowestKnown = int.MaxValue;
            return Buttons.Min(b => CountFewestSteps(initialLightState, b, initialPressedState.ToDictionary(), ref lowestKnown));
        }
        
        public int CountFewestSteps(string state, string button, Dictionary<string, int> pressed, ref int lowestKnown)
        {
            var currentCost = pressed.Sum(b => b.Value);

            if (currentCost > lowestKnown)
                return int.MaxValue;

            if (state.Equals(TargetState))
                return currentCost;

            var nextState = new Span<char>(state.ToCharArray());

            foreach (var i in button.Split(',').Select(int.Parse))
                nextState[i] = nextState[i] == '.' ? '#' : '.';

            pressed[button]++;

            var buttons = Buttons.Where(b => b != button && pressed[b] < 2).ToArray();

            foreach (var b in buttons)
            {
                var cost = CountFewestSteps(nextState.ToString(), b, pressed.ToDictionary(), ref lowestKnown);
                
                if (cost < lowestKnown)
                    lowestKnown = cost;
            }

            return lowestKnown;
        }

        public static Instruction Parse(string input)
        {
            var parts = input.Split(' ');
            var targetState = ParseTargetState(parts[0]);
            var joltageRequirements = ParseJoltageRequirement(parts[^1]);
            var buttons = parts[1..^1].Select(ParseButton).ToArray();
            
            return new Instruction(buttons, targetState, joltageRequirements);
        }

        private static string ParseTargetState(string targetStatePart)
        {
            return targetStatePart[1..^1];
        }

        private static int[] ParseJoltageRequirement(string joltageRequirementPart)
        {
            return joltageRequirementPart[1..^1].Split(',').Select(int.Parse).ToArray();
        }

        private static string ParseButton(string buttonPart)
        {
            return buttonPart[1..^1];
        }
    }
}
