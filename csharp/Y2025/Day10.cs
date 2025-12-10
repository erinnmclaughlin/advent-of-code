using System.Text;

namespace AdventOfCode.Y2025;

public sealed class Day10() : AdventDay(2025, 10)
{
    public override AdventDaySolution Solve(string input)
    {
        foreach (var line in InputHelper.GetLines(input))
        {
            
        }

        // TODO: implement puzzle logic here

        return ("", "");
    }

    public sealed class StateContainer
    {
        public bool[] Target { get; }
        public bool[] Lights { get; }
        public int[][] Buttons { get; }
        public int[] JoltageRequirements { get; }

        public StateContainer(int[][] buttons, bool[] targetState, int[] joltageRequirements)
        {
            Buttons = buttons;
            Target = targetState;
            Lights = Enumerable.Repeat(false, targetState.Length).ToArray();
            JoltageRequirements = joltageRequirements;
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
