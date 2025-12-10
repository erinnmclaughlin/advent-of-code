namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public override AdventDaySolution Solve(string input)
    {
        var sum = 0L;
        
        foreach (var line in InputHelper.GetLines(input))
        {
            var sc = StateContainer.Parse(line);
            sum += sc.CountFewestSteps();
        }

        // TODO: implement puzzle logic here

        return (sum, "");
    }
    
    public sealed class StateContainer
    {
        public bool[] Target { get; }
        public int[][] Buttons { get; }
        public int[] JoltageRequirements { get; }

        public StateContainer(int[][] buttons, bool[] targetState, int[] joltageRequirements)
        {
            Buttons = buttons;
            Target = targetState;
            JoltageRequirements = joltageRequirements;
        }

        public int CountFewestSteps()
        {
            var initialLightState = Enumerable.Repeat(false, Target.Length);
            var initialPressedState = Buttons.ToDictionary(b => b, _ => 0);

            var lowesetKnown = CountStepsToAnySolution();
            return Buttons.Min(b => CountFewestSteps(initialLightState.ToArray(), b, initialPressedState.ToDictionary(), lowesetKnown));
        }
        
        public int CountFewestSteps(bool[] state, int[] button, Dictionary<int[], int> pressed, int lowestKnown)
        {
            var currentCost = pressed.Sum(b => b.Value);

            if (currentCost > lowestKnown)
                return int.MaxValue;

            if (state.SequenceEqual(Target))
                return currentCost;

            foreach (var i in button)
                state[i] = !state[i];

            pressed[button]++;

            var buttons = Buttons.Where(b => b != button && pressed[b] < 2).ToArray();

            foreach (var b in buttons)
            {
                var cost = CountFewestSteps([..state], b, pressed.ToDictionary(), lowestKnown);
                
                if (cost < lowestKnown)
                    lowestKnown = cost;
            }

            return lowestKnown;
        }

        public int CountStepsToAnySolution()
        {
            var state = Enumerable.Repeat(false, Target.Length).ToArray();
            var pressed = Buttons.ToDictionary(b => b, _ => 0);
            
            foreach (var b in Buttons)
            {
                var cost = CountStepsToAnySolution([..state], b, pressed.ToDictionary());

                if (cost < int.MaxValue)
                    return cost;
            }

            return int.MaxValue;
        }
        
        public int CountStepsToAnySolution(bool[] state, int[] button, Dictionary<int[], int> pressed)
        {
            if (state.SequenceEqual(Target))
                return pressed.Sum(b => b.Value);

            foreach (var i in button)
                state[i] = !state[i];

            pressed[button]++;

            foreach (var b in Buttons.Where(b => b != button && pressed[b] < 2))
            {
                var cost = CountStepsToAnySolution([..state], b, pressed.ToDictionary());

                if (cost < int.MaxValue)
                    return cost;
            }
            
            return int.MaxValue;
        }
        
        public static StateContainer Parse(string input)
        {
            var parts = input.Split(' ');
            var targetState = ParseTargetState(parts[0]);
            var joltageRequirements = ParseJoltageRequirement(parts[^1]);
            var buttons = parts[1..^1].Select(ParseButton).ToArray();
            
            return new StateContainer(buttons, targetState, joltageRequirements);
        }

        public static bool[] ParseTargetState(string targetStatePart)
        {
            return targetStatePart[1..^1].Select(c => c is '#').ToArray();
        }

        public static int[] ParseJoltageRequirement(string joltageRequirementPart)
        {
            return joltageRequirementPart[1..^1].Split(',').Select(int.Parse).ToArray();
        }

        private static int[] ParseButton(string buttonPart)
        {
            return buttonPart[1..^1].Split(',').Select(int.Parse).ToArray();
        }
    }
}
