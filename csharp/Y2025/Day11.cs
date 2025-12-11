namespace AdventOfCode.Y2025;

public sealed class Day11() : AdventDay(2025, 11)
{
    public sealed class Item(string label)
    {
        public string Label { get; } = label;
        public List<Item> Connections { get; } = [];
    }
    
    public override AdventDaySolution Solve(string input)
    {
        var items = ParseInput(input);
        var start1 = items.FirstOrDefault(x => x.Label == "you");
        var part1 = start1 is null ? 0 : CountPathsToOut(start1);
        
        var start2 = items.FirstOrDefault(x => x.Label == "svr");
        var part2 = start2 is null ? 0 : CountSvrPathsToOut(start2, false, false, []);
        return (part1, part2);
    }

    private static int CountPathsToOut(Item start)
    {
        if (start.Label == "out")
            return 1;

        return start.Connections.Sum(CountPathsToOut);
    }

    private static long CountSvrPathsToOut(
        Item item,
        bool sawDac,
        bool sawFft, 
        Dictionary<(string Item, bool SawDac, bool SawFft), long> memory)
    {
        if (item.Label == "out")
            return sawDac && sawFft ? 1 : 0;

        sawDac = sawDac || item.Label == "dac";
        sawFft = sawFft || item.Label == "fft";

        var sum = 0L;
        
        foreach (var connection in item.Connections)
        {
            var key = (connection.Label, sawDac, sawFft);
            
            if (!memory.TryGetValue(key, out var value))
            {
                value = CountSvrPathsToOut(connection, sawDac, sawFft, memory);
                memory[key] = value;
            }
            
            sum += value;
        }

        return sum;
    }
    
    private static List<Item> ParseInput(string input)
    {
        var lines = InputHelper.GetLines(input);
        var items = new Dictionary<string, Item>
        {
            ["out"] = new("out")
        };

        // loop twice: once to get the items, and then again to add the connections
        foreach (var line in lines.Concat(lines))
        {
            var parts = line.Split(' ');
            var label = parts[0].TrimEnd(':');

            if (!items.TryGetValue(label, out var item))
            {
                items[label] = new Item(label);
                continue;
            }
            
            foreach (var part in parts.Skip(1))
            {
                item.Connections.Add(items[part]);
            }
        }

        return items.Values.ToList();
    }
}
