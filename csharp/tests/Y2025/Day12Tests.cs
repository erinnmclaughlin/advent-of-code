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
        Assert.Equal(3, instructions.Count);

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
        
        // first instruction
        Assert.Equal(4, instructions[0].BigShape.Height);
        Assert.Equal(4, instructions[0].BigShape.Width);
        Assert.Equal(6, instructions[0].Target.Length);
        Assert.Equivalent(new[] { 0, 0, 0, 0, 2, 0 }, instructions[0].Target);
        
        // second instruction
        Assert.Equal(12, instructions[1].BigShape.Width);
        Assert.Equal(5, instructions[1].BigShape.Height);
    }
    
    [Theory]
    [InlineData(0, "###\n##.\n##.", "###\n###\n..#")]
    [InlineData(1, "###\n##.\n.##", ".##\n###\n#.#")]
    [InlineData(2, ".##\n###\n##.", "##.\n###\n.##")]
    [InlineData(3, "##.\n###\n##.", "###\n###\n.#.")]
    [InlineData(4, "###\n#..\n###", "###\n#.#\n#.#")]
    [InlineData(5, "###\n.#.\n###", "#.#\n###\n#.#")]
    public void Can_rotate_shape(int index, string expectedStart, string expectedEnd)
    {
        var shapes = Day12.ParseInput(Sample).Shapes;

        var shape = shapes[index];
        var rotatedShape = shape.GetRotatedLeft();

        Assert.Equal(expectedStart, shape.ToString());
        Assert.Equal(expectedEnd, rotatedShape.ToString());
    }
    
    [Theory]
    [InlineData(0, "###\n##.\n##.", "###\n.##\n.##")]
    [InlineData(1, "###\n##.\n.##", "###\n.##\n##.")]
    [InlineData(2, ".##\n###\n##.", "##.\n###\n.##")]
    [InlineData(3, "##.\n###\n##.", ".##\n###\n.##")]
    [InlineData(4, "###\n#..\n###", "###\n..#\n###")]
    [InlineData(5, "###\n.#.\n###", "###\n.#.\n###")]
    public void Can_vertically_flip_shape(int index, string expectedStart, string expectedEnd)
    {
        var shapes = Day12.ParseInput(Sample).Shapes;

        var shape = shapes[index];
        var flippedShape = shape.GetFlippedVertical();

        Assert.Equal(expectedStart, shape.ToString());
        Assert.Equal(expectedEnd, flippedShape.ToString());
    }

    [Theory]
    [InlineData(0, "###\n##.\n##.", "##.\n##.\n###")]
    [InlineData(1, "###\n##.\n.##", ".##\n##.\n###")]
    [InlineData(2, ".##\n###\n##.", "##.\n###\n.##")]
    [InlineData(3, "##.\n###\n##.", "##.\n###\n##.")]
    [InlineData(4, "###\n#..\n###", "###\n#..\n###")]
    [InlineData(5, "###\n.#.\n###", "###\n.#.\n###")]
    public void Can_horizontally_flip_shape(int index, string expectedStart, string expectedEnd)
    {
        var shapes = Day12.ParseInput(Sample).Shapes;
        
        var shape = shapes[index];
        var flippedShape = shape.GetFlippedHorizontal();
        
        Assert.Equal(expectedStart, shape.ToString());
        Assert.Equal(expectedEnd, flippedShape.ToString());
    }
}
