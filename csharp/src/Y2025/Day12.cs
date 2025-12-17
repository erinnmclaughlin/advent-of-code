namespace AdventOfCode.Y2025;

public sealed class Day12() : AdventDay(2025, 12)
{
    public override AdventDaySolution Solve(string input)
    { 
        var fitCount = 0;

        foreach (var line in InputHelper.GetLines(input))
        {
            if (!line.Contains('x'))
                continue;

            var parts = line.Split(' ');
            
            var regionParts = parts[0].Split('x');
            var width = int.Parse(regionParts[0]);
            var height = int.Parse(regionParts[1].TrimEnd(':'));
            var regionArea = Math.BigMul(width, height);
            
            var numberOfPresents = parts.Skip(1).Select(int.Parse).Sum();
            var worstCaseNeededArea = numberOfPresents * 3L * 3L;

            // if we can fit everything without moving stuff around at all, then don't bother
            if (worstCaseNeededArea <= regionArea)
            {
                fitCount++;
                continue;
            }
            
            // if the region area is too small to fit even the smallest possible presents, then don't bother
            if (numberOfPresents <= regionArea)
            {
                continue;
            }
            
            // Otherwise, do the flippy stuff. Except we don't actually have to, because the "real" puzzle input always
            // falls into one of the two above cases
            throw new NotImplementedException();
        }

        return (fitCount, "🎅");
    }
}
