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
        public Circuit Circuit { get; set; } = new();
        public (long X, long Y, long Z) Position { get; }
        
        public JunctionBox(long x, long y, long z)
        {
            Position = (x, y, z);
            Circuit.Attach(this);
        }

        public void ConnectTo(JunctionBox other)
        {
            var junctionsToTransfer = Circuit.Junctions.ToList();
            junctionsToTransfer.ForEach(other.Circuit.Attach);
        }

        public float GetDistanceTo(JunctionBox other) => (AsVector3() - other.AsVector3()).Length();
        
        private Vector3 AsVector3() => new(Position.X, Position.Y, Position.Z);
    }

    public override AdventDaySolution Solve(string input) => Solve(input, 1000);

    public static AdventDaySolution Solve(string input, int partOneConnectionCount)
    {
        var junctionBoxes = ParseInput(input);
        var circuits = junctionBoxes.Select(x => x.Circuit).ToList();
        
        var (part1, part2) = (0L, 0L);
        
        foreach (var (p1, p2) in junctionBoxes.EnumerateOrderedPairs())
        {
            p1.ConnectTo(p2);
            circuits.RemoveAll(c => c.Junctions.Count == 0);

            if (--partOneConnectionCount == 0)
            {
                part1 = circuits
                    .Select(x => x.Junctions.Count)
                    .OrderDescending()
                    .Take(3)
                    .Aggregate(1L, (current, next) => current * next);
            }
            
            if (circuits.Count == 1)
            {
                part2 = p1.Position.X * p2.Position.X;
                break;
            }
        }
        
        return (part1, part2);
    }
    
    public static JunctionBox[] ParseInput(string input) => InputHelper
        .GetLines(input)
        .Select(line =>
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return new JunctionBox(parts[0], parts[1], parts[2]);
        })
        .ToArray();
}

public static class Day08Extensions
{
    extension(Day08.JunctionBox[] positions)
    {
        public IOrderedEnumerable<(Day08.JunctionBox Box1, Day08.JunctionBox Box2)> EnumerateOrderedPairs()
        {
            return positions.EnumeratePairs().OrderBy(x => x.Box1.GetDistanceTo(x.Box2));
        }

        private IEnumerable<(Day08.JunctionBox Box1, Day08.JunctionBox Box2)> EnumeratePairs()
        {
            var pairs = new HashSet<(Day08.JunctionBox, Day08.JunctionBox)>();
        
            foreach (var (p1, p2) in positions.SelectMany(p1 => positions.Select(p2 => (p1, p2))))
            {
                if (p1 != p2 && pairs.Add((p1, p2)) && pairs.Add((p2, p1)))
                    yield return (p1, p2);
            }
        }
    }
}