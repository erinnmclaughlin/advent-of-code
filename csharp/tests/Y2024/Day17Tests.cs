using AdventOfCode.Y2024;

namespace AdventOfCode.Tests.Y2024;

public sealed class Day17Tests
{
    private readonly Day17 _solver = new();

    private string Sample =
        """
        Register A: 729
        Register B: 0
        Register C: 0

        Program: 0,1,5,4,3,0
        """;
    
    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var program = Day17.AocProgram.Parse(Sample);
        var result = string.Join(',', program.Execute());
        Assert.Equal("4,6,3,5,6,3,5,2,1,0", result);
    }
    
    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
        Assert.Equal("117440", p2);
    }
    
    [Fact]
    public void Test1()
    {
        var program = new Day17.AocProgram(2,6) { RegisterC = 9 };
        _ = program.Execute();
        Assert.Equal(1, program.RegisterB);
    }

    [Fact]
    public void Test2()
    {
        var program = new Day17.AocProgram(5, 0, 5, 1, 5, 4) { RegisterA = 18 };
        Assert.Equivalent(new List<int> { 0,1,2 }, program.Execute());
    }
    
    [Fact]
    public void Test3()
    {
        var program = new Day17.AocProgram(0,1,5,4,3,0) { RegisterA = 2024 };
        Assert.Equivalent(new List<int> { 4,2,5,6,7,7,7,7,3,1,0 }, program.Execute());
        Assert.Equal(0, program.RegisterA);
    }
    
    [Fact]
    public void Test4()
    {
        var program = new Day17.AocProgram(1,7) { RegisterB = 29 };
        program.Execute();
        Assert.Equal(26, program.RegisterB);
    }

    [Fact]
    public void Example()
    {
        var program = new Day17.AocProgram(0, 1, 5, 4, 3, 0) { RegisterA = 729 };
        var output = program.Execute();
        Assert.Equivalent(new List<int>{ 4,6,3,5,6,3,5,2,1,0 }, output);
    }
}
