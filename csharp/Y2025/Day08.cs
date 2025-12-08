using System.Numerics;

namespace AdventOfCode.Y2025;

public sealed class Day08() : AdventDay(2025, 8)
{
    public override AdventDaySolution Solve(string input)
    {
        var circuits = ProcessCircuits(input, 1000);
        var part1 = GetProduct(circuits);
        
        return (part1, "");
    }

    public static Circuit[] ProcessCircuits(string input, int steps)
    {
        var pairsProcessed = 0;
        var positions = ParseInput(input).ToArray();

        foreach (var (_, p1, p2) in EnumeratePairs(positions).OrderBy(x => x.Distance).ToList())
        {
            p1.MergeWith(p2);
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

    public static IEnumerable<(float Distance, JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(string input)
    {
        var positions = ParseInput(input).ToArray();
        return EnumeratePairs(positions);
    }
    
    private static IEnumerable<(float Distance, JunctionBox Box1, JunctionBox Box2)> EnumeratePairs(JunctionBox[] positions)
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
        public HashSet<JunctionBox> DirectConnections { get; } = [];
        
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

        public bool MergeWith(JunctionBox other)
        {
            var c1 = DirectConnections.Add(other);
            var c2 = other.DirectConnections.Add(this);
            
            if (!c1 || !c2)
                return false;

            if (Circuit != other.Circuit)
            {
                var newCircuit = new Circuit();
                var allJunctions = Circuit.Junctions.Union(other.Circuit.Junctions).ToList();
                foreach (var junction in allJunctions)
                {
                    newCircuit.AddJunction(junction);
                }
            }

            return true;
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
            AddJunction(box);
        }
        
        public void AddJunction(JunctionBox box)
        {
            box.Circuit?.Junctions.Remove(box);
            box.Circuit = this;
            Junctions.Add(box);
        }
    }
}
