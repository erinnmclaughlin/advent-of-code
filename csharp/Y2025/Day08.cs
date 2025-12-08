using System.Numerics;

namespace AdventOfCode.Y2025;

public sealed class Day08() : AdventDay(2025, 8)
{
    // 10260 is too low
    // 10773 "that's not the right answer"
    
    // Current output:
    // [999] There is 1 circuit with 27 junctions.
    // [999] There are 2 circuits with 19 junctions.
    // [999] There is 1 circuit with 17 junctions.
    // [999] There are 2 circuits with 16 junctions.
    // [999] There is 1 circuit with 15 junctions.
    // [999] There are 3 circuits with 13 junctions.
    // [999] There are 3 circuits with 12 junctions.
    // [999] There are 7 circuits with 11 junctions.
    // [999] There are 2 circuits with 10 junctions.
    // [999] There are 7 circuits with 9 junctions.
    // [999] There are 6 circuits with 8 junctions.
    // [999] There are 13 circuits with 7 junctions.
    // [999] There are 14 circuits with 6 junctions.
    // [999] There are 13 circuits with 5 junctions.
    // [999] There are 20 circuits with 4 junctions.
    // [999] There are 24 circuits with 3 junctions.
    // [999] There are 40 circuits with 2 junctions.
    // [999] There are 116 circuits with 1 junctions.
    // [999] Total junction count: 1000
    // [999] Total circuit count: 275
    // [999] : 27 * 19 * 19 = 9747
    // 
    // [1000] There is 1 circuit with 27 junctions.
    // [1000] There is 1 circuit with 20 junctions.
    // [1000] There is 1 circuit with 19 junctions.
    // [1000] There is 1 circuit with 17 junctions.
    // [1000] There are 2 circuits with 16 junctions.
    // [1000] There is 1 circuit with 15 junctions.
    // [1000] There are 3 circuits with 13 junctions.
    // [1000] There are 3 circuits with 12 junctions.
    // [1000] There are 7 circuits with 11 junctions.
    // [1000] There are 2 circuits with 10 junctions.
    // [1000] There are 7 circuits with 9 junctions.
    // [1000] There are 6 circuits with 8 junctions.
    // [1000] There are 13 circuits with 7 junctions.
    // [1000] There are 14 circuits with 6 junctions.
    // [1000] There are 13 circuits with 5 junctions.
    // [1000] There are 20 circuits with 4 junctions.
    // [1000] There are 24 circuits with 3 junctions.
    // [1000] There are 39 circuits with 2 junctions.
    // [1000] There are 117 circuits with 1 junctions.
    // [1000] Total junction count: 1000
    // [1000] Total circuit count: 275
    // [1000] : 27 * 20 * 19 = 10260 
    //  
    // [1001] There is 1 circuit with 27 junctions.
    // [1001] There is 1 circuit with 20 junctions.
    // [1001] There is 1 circuit with 19 junctions.
    // [1001] There is 1 circuit with 17 junctions.
    // [1001] There are 2 circuits with 16 junctions.
    // [1001] There is 1 circuit with 15 junctions.
    // [1001] There are 3 circuits with 13 junctions.
    // [1001] There are 3 circuits with 12 junctions.
    // [1001] There are 7 circuits with 11 junctions.
    // [1001] There are 3 circuits with 10 junctions.
    // [1001] There are 6 circuits with 9 junctions.
    // [1001] There are 6 circuits with 8 junctions.
    // [1001] There are 13 circuits with 7 junctions.
    // [1001] There are 14 circuits with 6 junctions.
    // [1001] There are 13 circuits with 5 junctions.
    // [1001] There are 20 circuits with 4 junctions.
    // [1001] There are 23 circuits with 3 junctions.
    // [1001] There are 40 circuits with 2 junctions.
    // [1001] There are 117 circuits with 1 junctions.
    // [1001] Total junction count: 1000
    // [1001] Total circuit count: 275
    // [1001] : 27 * 20 * 19 = 10260
    public override AdventDaySolution Solve(string input)
    {
        var part1 = 0L;
        
        for (var i = 999; i <= 1001; i++)
        {
            var circuits = GetCircuits(input, i);
            
            foreach (var group in circuits.GroupBy(x => x.Junctions.Count).OrderByDescending(x => x.Key))
            {
                var count = group.Count();
                var isAre = count == 1 ? "is" : "are";
                var pluralS = count == 1 ? "" : "s";
                Console.WriteLine($"[{i}] There {isAre} {count} circuit{pluralS} with {group.Key} junctions.");
            }
            
            Console.WriteLine($"[{i}] Total junction count: {circuits.Sum(x => x.Junctions.Count)}");
            Console.WriteLine($"[{i}] Total circuit count: {circuits.Count}");
            
            var biggest = GetLargestCircuits(circuits, 3);
            var product = GetProduct(biggest);

            Console.WriteLine($"[{i}] : {string.Join(" * ", biggest)} = {product}");
            Console.WriteLine();

            if (i == 1000)
                part1 = product;
        }

        return (part1, "");
    }

    public static long GetProduct(List<long> biggest)
    {
        return biggest.Skip(1).Aggregate(biggest[0], (current, b) => current * b);
    }

    public static List<long> GetLargestCircuits(List<Circuit> circuits, int take)
    {
        return circuits.Select(x => (long)x.Junctions.Count).OrderDescending().Take(take).ToList();
    }

    public static IEnumerable<(float Distance, JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(string input)
    {
        var positions = ParseInput(input).ToArray();
        return EnumeratePairs(positions);
    }
    
    public static IEnumerable<(float Distance, JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(JunctionBox[] positions)
    {
        var pairs = new HashSet<(JunctionBox, JunctionBox)>();
        
        foreach (var position in positions)
        {
            foreach (var other in positions)
            {
                var d = position.GetDistanceTo(other);

                if (other.Circuit != position.Circuit)
                {
                    if (pairs.Add((position, other)) && pairs.Add((other, position)))
                        yield return (d, position, other);
                }
            }
        }
    }

    public static List<Circuit> GetCircuits(string input, int steps)
    {
        var connectionsMade = 0;
        var positions = ParseInput(input).ToArray();

        foreach (var (_, p1, p2) in EnumeratePairs(positions).OrderBy(x => x.Distance))
        {
            if (p1.Circuit != p2.Circuit)
            {
                p1.MergeWith(p2);
                connectionsMade++;
            }
            
            if (connectionsMade == steps)
                break;
        }
        
        return positions.Select(x => x.Circuit).Distinct().ToList();
    }

    private static IEnumerable<JunctionBox> ParseInput(string input)
    {
        return InputHelper.GetLines(input).Select(ParseLine);
    }
    
    private static JunctionBox ParseLine(string line)
    {
        var parts = line.Split(',').Select(int.Parse).ToArray();
        return new JunctionBox(parts[0], parts[1], parts[2]);
    }

    public sealed class JunctionBox
    {
        public Vector3 Position { get; }

        public Circuit Circuit { get; set; }
        
        public JunctionBox(int x, int y, int z)
        {
            Position = new Vector3(x, y, z);
            Circuit = new Circuit(this);
        }
        
        public override string ToString() => Position.ToString();

        public float GetDistanceTo(JunctionBox other)
        {
            return Vector3.Distance(Position, other.Position);
        }

        public void MergeWith(JunctionBox other)
        {
            if (Circuit.Junctions.Count < other.Circuit.Junctions.Count)
            {
                Circuit.Junctions.Remove(this);
                Circuit = other.Circuit;
                other.Circuit.Junctions.Add(this);
            }
            else
            {
                other.Circuit.Junctions.Remove(other);
                other.Circuit = Circuit;
                Circuit.Junctions.Add(other);
            }
        }
    }

    public sealed class Circuit
    {
        public List<JunctionBox> Junctions { get; } = [];

        public Circuit()
        {
        }

        public Circuit(JunctionBox box)
        {
            Junctions.Add(box);
        }
    }
}
