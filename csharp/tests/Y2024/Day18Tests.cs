using AdventOfCode.Y2024;

namespace AdventOfCode.Tests.Y2024;

public class Day18Tests(ITestOutputHelper output)
{
    private readonly Day18 _solver = new();

    private const string Sample = """
    5,4
    4,2
    4,5
    3,0
    2,1
    6,3
    2,4
    1,5
    0,6
    3,3
    2,6
    5,1
    1,2
    5,5
    2,5
    6,5
    1,4
    0,4
    6,4
    1,1
    6,1
    1,0
    0,5
    1,6
    2,0
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = Day18.Solve(Sample, 7, 12);
        Assert.Equal("22", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = Day18.Solve(Sample, 7, 12);
        Assert.Equal("6,1", p2);
    }

    [Fact]
    public void Test_parse_line()
    {
        var parsed = InputHelper.GetLines(Sample).Select(Day18.ParseLine).Take(3).ToArray();
        Assert.Equivalent(parsed, new[] { (5, 4), (4, 2), (4, 5) } );
    }

    [Fact]
    public void Test_map_to_string()
    {
        var map = new Day18.Map(7);

        foreach (var pos in InputHelper.GetLines(Sample).Select(Day18.ParseLine).Take(12))
        {
            map.CorruptedPositions.Add(pos);
        }

        const string expected = """
            ...#...
            ..#..#.
            ....#..
            ...#..#
            ..#..#.
            .#..#..
            #.#....
            """;

        var actual = map.ToString();

        output.WriteLine(actual);

        Assert.Equal(expected, actual);
    }
}
