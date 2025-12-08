using System.Numerics;

namespace AdventOfCode.Y2025;

public sealed class Day08() : AdventDay(2025, 8)
{
    // TODO: Solve parts 1 and 2 together in same loop
    
    public override AdventDaySolution Solve(string input)
    {
        var circuits = ProcessCircuits(input, 1000);
        var part1 = GetProduct(circuits);
        var part2 = MergeIntoOneCircuit(input);
        
        return (part1, part2);
    }
    
    public static long MergeIntoOneCircuit(string input)
    {
        var junctionBoxes = ParseInput(input).ToArray();
        var circuits = junctionBoxes.Select(x => x.Circuit).ToHashSet();

        foreach (var (p1, p2) in EnumeratePairs(junctionBoxes).OrderBy(x => x.Box1.GetDistanceTo(x.Box2)))
        {
            p1.ConnectTo(p2);
            circuits.RemoveWhere(c => c.Junctions.Count == 0);
            
            if (circuits.Count == 1)
            {
                return Math.BigMul(p1.Position.X, p2.Position.X);
            }
        }

        return -1;
    }

    public static Circuit[] ProcessCircuits(string input, int steps)
    {
        var pairsProcessed = 0;
        var positions = ParseInput(input).ToArray();

        foreach (var (p1, p2) in EnumeratePairs(positions).OrderBy(x => x.Box1.GetDistanceTo(x.Box2)))
        {
            p1.ConnectTo(p2);
            pairsProcessed++;
            
            if (pairsProcessed == steps)
                break;
        }
        
        return positions.Select(x => x.Circuit).Distinct().ToArray();
    }

    public static long GetProduct(Circuit[] circuits, int take = 3)
    {
        var biggest = circuits.Select(x => x.Junctions.Count).OrderDescending().Take(take).ToList();
        return biggest.Skip(1).Aggregate(biggest[0], (current, b) => current * b);
    }

    public static IEnumerable<(JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(string input)
    {
        var positions = ParseInput(input).ToArray();
        return EnumeratePairs(positions);
    }
    
    private static IEnumerable<(JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(JunctionBox[] positions)
    {
        var pairs = new HashSet<(JunctionBox, JunctionBox)>();
        
        foreach (var position in positions)
        {
            foreach (var other in positions)
            {
                if (other.Circuit != position.Circuit)
                {
                    if (pairs.Add((position, other)) && pairs.Add((other, position)))
                        yield return (position, other);
                }
            }
        }
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
        public Circuit Circuit { get; set; }
        public (int X, int Y, int Z) Position { get; }
        
        public JunctionBox(int x, int y, int z)
        {
            Position = (x, y, z);
            Circuit = new Circuit();
            Circuit.AddJunction(this);
        }

        public override string ToString()
        {
            return GetVector3().ToString();
        }
        
        public Vector3 GetVector3() => new(Position.X, Position.Y, Position.Z);

        public float GetDistanceTo(JunctionBox other)
        {
            return Vector3.Distance(GetVector3(), other.GetVector3());
        }

        public void ConnectTo(JunctionBox other)
        {
            foreach (var junction in other.Circuit.Junctions.ToList())
            {
                Circuit.AddJunction(junction);
            }
        }
    }

    public sealed class Circuit
    {
        private readonly HashSet<JunctionBox> _junctions = [];
        public IReadOnlySet<JunctionBox> Junctions => _junctions;

        public void AddJunction(JunctionBox box)
        {
            box.Circuit._junctions.Remove(box);
            box.Circuit = this;
            _junctions.Add(box);
        }
    }
}
