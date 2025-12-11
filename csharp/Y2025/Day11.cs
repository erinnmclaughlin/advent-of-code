namespace AdventOfCode.Y2025;

public sealed class Day11() : AdventDay(2025, 11)
{
    public override AdventDaySolution Solve(string input)
    {
        var items = ParseInput(input);
        var start1 = items.FirstOrDefault(x => x.Label == "you");
        var part1 = start1 is null ? 0 : CountPathsToOut(start1);
        
        var start2 = items.FirstOrDefault(x => x.Label == "svr");
        var part2 = start2 is null ? 0 : CountSvrPathsToOut(start2);
        return (part1, part2);
    }

    public static int CountPathsToOut(Item start)
    {
        if (start.Label == "out")
            return 1;

        return start.Connections.Sum(CountPathsToOut);
    }

    public static int CountSvrPathsToOut(Item item, bool sawDac = false, bool sawFft = false)
    {
        if (item.Label == "out")
            return sawDac && sawFft ? 1 : 0;

        sawDac = sawDac || item.Label == "dac";
        sawFft = sawFft || item.Label == "fft";

        return item.Connections.Sum(child => CountSvrPathsToOut(child, sawDac, sawFft));
    }
    
    public static List<Item> ParseInput(string input)
    {
        var lines = InputHelper.GetLines(input);
        var items = new Dictionary<string, Item>
        {
            ["out"] = new Item("out")
        };

        foreach (var line in lines)
        {
            var label = line.Split(' ')[0].TrimEnd(':');
            items[label] = new Item(label);
        }

        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var label = parts[0].TrimEnd(':');
            var item = items[label];

            foreach (var part in parts.Skip(1))
            {
                item.Connections.Add(items[part]);
            }
        }
        
        return items.Values.ToList();
    }

    public sealed class Item(string label)
    {
        public string Label { get; } = label;
        public List<Item> Connections { get; } = [];
    }
}
