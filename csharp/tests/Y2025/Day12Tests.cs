using AdventOfCode.Y2025;
using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day12Tests
{
    private readonly Day12 _solver = new();

    private const string Sample = """
    0:
    ###
    ##.
    ##.
    
    1:
    ###
    ##.
    .##
    
    2:
    .##
    ###
    ##.
    
    3:
    ##.
    ###
    ##.
    
    4:
    ###
    #..
    ###
    
    5:
    ###
    .#.
    ###
    
    4x4: 0 0 0 0 2 0
    12x5: 1 0 1 0 2 2
    12x5: 1 0 1 0 3 2
    """;

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("", p1);
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("", p2);
    }

    [Fact]
    public void Can_parse_instructions()
    {
        var (shapes, instructions) = Day12.ParseInput(Sample);
        
        Assert.Equal(6, shapes.Count);
        Assert.Equal(3, instructions.Length);

        var shape = shapes[0];
        
        // top row
        Assert.Contains(new GridCell(0, 0), shape.Cells);
        Assert.Contains(new GridCell(1, 0), shape.Cells);
        Assert.Contains(new GridCell(2, 0), shape.Cells);
        
        // middle row
        Assert.Contains(new GridCell(0, 1), shape.Cells);
        Assert.Contains(new GridCell(1, 1), shape.Cells);
        Assert.DoesNotContain(new GridCell(2, 1), shape.Cells);
        
        // bottom row
        Assert.Contains(new GridCell(0, 2), shape.Cells);
        Assert.Contains(new GridCell(1, 2), shape.Cells);
        Assert.DoesNotContain(new GridCell(2, 2), shape.Cells);
    }
    
    [Fact]
    public void CanRotateShape()
    {
        var shape = Day12.ParseInput(Sample).Shapes[0];
        var rotatedShape = shape.GetRotatedLeft();

        var expectedStart =
            """
            ###
            ##.
            ##.
            """;

        var expectedEnd =
            """
            ###
            ###
            ..#
            """;

        Assert.Equal(expectedStart, shape.ToString());
        Assert.Equal(expectedEnd, rotatedShape.ToString());
    }
}
