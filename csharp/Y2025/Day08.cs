using System.Numerics;

namespace AdventOfCode.Y2025;

public sealed class Day08() : AdventDay(2025, 8)
{
    public sealed class Circuit
    {
        private readonly HashSet<JunctionBox> _junctions = [];
        public IReadOnlySet<JunctionBox> Junctions => _junctions;

        public void Attach(JunctionBox box)
        {
            box.Circuit._junctions.Remove(box);
            box.Circuit = this;
            _junctions.Add(box);
        }
    }
    
    public sealed class JunctionBox
    {
        public Circuit Circuit { get; set; }
        public (int X, int Y, int Z) Position { get; }
        
        public JunctionBox(int x, int y, int z)
        {
            Position = (x, y, z);
            Circuit = new Circuit();
            Circuit.Attach(this);
        }

        public void ConnectTo(JunctionBox other)
        {
            Circuit.Junctions.ToList().ForEach(other.Circuit.Attach);
        }
        
        public float GetDistanceTo(JunctionBox other)
        {
            return Vector3.Distance(GetVector3(), other.GetVector3());
        }
        
        public override string ToString()
        {
            return GetVector3().ToString();
        }

        private Vector3 GetVector3()
        {
            return new Vector3(Position.X, Position.Y, Position.Z);
        }
    }
    
    public override AdventDaySolution Solve(string input)
    {
        // TODO: Solve parts 1 and 2 together in same loop

        var positions = ParseInput(input).ToArray();
        var circuits = ProcessCircuits(positions, 1000);
        var part1 = GetProduct(circuits);
        var part2 = MergeIntoOneCircuit(input);
        
        return (part1, part2);
    }
    
    public static IEnumerable<JunctionBox> ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(line =>
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return new JunctionBox(parts[0], parts[1], parts[2]);
        });
    
    public static long MergeIntoOneCircuit(string input)
    {
        var junctionBoxes = ParseInput(input).ToArray();
        var circuits = junctionBoxes.Select(x => x.Circuit).ToHashSet();
        long product = -1;

        foreach (var (p1, p2) in EnumerateOrderedPairs(junctionBoxes))
        {
            p1.ConnectTo(p2);
            circuits.RemoveWhere(c => c.Junctions.Count == 0);
            
            if (circuits.Count == 1)
            {
                product = Math.BigMul(p1.Position.X, p2.Position.X);
                break;
            }
        }

        return product;
    }

    public static Circuit[] ProcessCircuits(JunctionBox[] positions, int steps)
    {
        var pairsProcessed = 0;

        foreach (var (p1, p2) in EnumerateOrderedPairs(positions))
        {
            p1.ConnectTo(p2);
            
            if (++pairsProcessed == steps)
                break;
        }
        
        return positions.Select(x => x.Circuit).Distinct().ToArray();
    }

    public static long GetProduct(Circuit[] circuits, int take = 3)
    {
        var biggest = circuits.Select(x => x.Junctions.Count).OrderDescending().Take(take).ToList();
        return biggest.Skip(1).Aggregate(biggest[0], (current, b) => current * b);
    }

    public static IOrderedEnumerable<(JunctionBox Box1, JunctionBox Box2)> EnumerateOrderedPairs(JunctionBox[] positions)
    {
        return EnumeratePairs(positions).OrderBy(x => x.Box1.GetDistanceTo(x.Box2));
    }
    
    private static IEnumerable<(JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(JunctionBox[] positions)
    {
        var pairs = new HashSet<(JunctionBox, JunctionBox)>();
        
        foreach (var (p1, p2) in positions.SelectMany(p1 => positions.Select(p2 => (p1, p2))))
        {
            if (p1 != p2 && pairs.Add((p1, p2)) && pairs.Add((p2, p1)))
                yield return (p1, p2);
        }
    }
}
