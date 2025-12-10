using System.Text;
using AdventOfCode.Y2025;
using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day09Tests(ITestOutputHelper output)
{
    private readonly Day09 _solver = new();

    private const string Sample = """
    7,1
    11,1
    11,7
    9,7
    9,5
    2,5
    2,3
    7,3
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Equal("50", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Equal("24", p2);
    }

    [Fact]
    public void Test_parse_input()
    {
        var cells = Day09.ParseInput(Sample).ToList();
        var sb = new StringBuilder();
        foreach (var cell in cells)
        {
            sb.AppendLine($"{cell.Col},{cell.Row}");
        }

        var value = sb.ToString().TrimEnd();
        Assert.Equal(Sample, value);
    }

    [Fact]
    public void Test_print_grid_shape()
    {
        var grid = new GridShape(Day09.ParseInput(Sample).ToList());
        output.WriteLine(grid.ToString());
    }

    [Fact]
    public void Test_print_grid_shape_with_rectangle()
    {
        var cornerPoints = Day09.ParseInput(Sample).ToList();
        var shape = new GridShape(cornerPoints);
        
        var rectangles = cornerPoints.SelectMany(c1 => cornerPoints.Where(c2 => c1 != c2).Select(c2 => Day09.BuildRectangle(c1, c2)));
        
        output.WriteLine(shape.ToString());
        output.WriteLine("");
        
        foreach (var rect in rectangles.Where(r => r.GetArea() == 40))
        {
            output.WriteLine(shape.ToString(rect));
            output.WriteLine("");
        }
    }
}
