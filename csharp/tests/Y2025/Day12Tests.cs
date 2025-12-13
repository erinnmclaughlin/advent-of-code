using AdventOfCode.Y2025;

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
        // because i'm using both Mac and Windows
        expectedStart = expectedStart.ReplaceLineEndings(Environment.NewLine);
        expectedEnd = expectedEnd.ReplaceLineEndings(Environment.NewLine);
        
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
        // because i'm using both Mac and Windows
        expectedStart = expectedStart.ReplaceLineEndings(Environment.NewLine);
        expectedEnd = expectedEnd.ReplaceLineEndings(Environment.NewLine);
        
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
        // because i'm using both Mac and Windows
        expectedStart = expectedStart.ReplaceLineEndings(Environment.NewLine);
        expectedEnd = expectedEnd.ReplaceLineEndings(Environment.NewLine);
        
        var shapes = Day12.ParseInput(Sample).Shapes;
        
        var shape = shapes[index];
        var flippedShape = shape.GetFlippedHorizontal();
        
        Assert.Equal(expectedStart, shape.ToString());
        Assert.Equal(expectedEnd, flippedShape.ToString());
    }

    [Fact]
    public void Try_add_shape_succeeds_when_shape_fits()
    {
        var shape = Day12.ParseInput(Sample).Shapes[0];
        var bigShape = new Day12.LargeGridShape(3, 3);

        Assert.True(bigShape.TryAdd(shape));
        Assert.All(shape.Cells, c => Assert.Contains(c, bigShape.Cells));
    }

    [Fact]
    public void Try_add_shape_fails_when_shape_does_not_fit()
    {
        var shape = Day12.ParseInput(Sample).Shapes[0];
        var bigShape = new Day12.LargeGridShape(2, 2);
        
        Assert.False(bigShape.TryAdd(shape));
        Assert.Empty(bigShape.Cells);
    }

    [Fact]
    public void Try_add_shape_fails_when_offset_places_out_of_bounds()
    {
        var shape = Day12.ParseInput(Sample).Shapes[0];
        var bigShape = new Day12.LargeGridShape(3, 3);
        
        Assert.False(bigShape.TryAdd(shape, 1, 1));
        Assert.Empty(bigShape.Cells);
    }

    [Fact]
    public void Try_add_shape_fails_when_big_shape_already_contains_cell()
    {
        var shape = Day12.ParseInput(Sample).Shapes[0];
        var bigShape = new Day12.LargeGridShape(3, 3);
        bigShape.Cells.Add(new GridCell(0, 0));
        Assert.False(bigShape.TryAdd(shape));
        Assert.Single(bigShape.Cells);
    }

    [Fact]
    public void Try_fit_can_find_position_for_basic_shape()
    {
        var bigShape = new Day12.LargeGridShape(3, 3);
        var smallShape1 = new Day12.SmallGridShape([new GridCell(0, 0), new GridCell(0, 1), new GridCell(0, 2)]);
        var smallShape2 = new Day12.SmallGridShape([new GridCell(1, 0), new GridCell(1, 1), new GridCell(1, 2)]);
        
        Assert.True(Day12.TryFit(bigShape, smallShape1));
        Assert.True(Day12.TryFit(bigShape, smallShape2));
        Assert.Equal(6, bigShape.Cells.Count);
    }

    [Fact]
    public void Enumerate_available_positions_returns_all_options()
    {
        var bigShape = new Day12.LargeGridShape(4, 4);
        
        // ###
        // #..
        // ###
        var shape = Day12.ParseInput(Sample).Shapes[4];
        bigShape.TryAdd(shape);
        
        var numberOfOptions = bigShape.EnumerateAvailablePositions(shape).Distinct().Count();
        Assert.Equal(1, numberOfOptions);
    }
}
