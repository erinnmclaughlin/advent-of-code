using AdventOfCode.Y2025;

namespace AdventOfCode.Tests.Y2025;

public sealed class Day12Tests
{
    [Fact]
    public void Part_one_returns_false_when_region_is_obviously_too_small()
    {
        var canFit = Day12.SolveLine("2x2: 10 10 10 10 10 10");
        Assert.False(canFit);
    }

    [Fact]
    public void Part_one_returns_true_when_region_is_obviously_big_enough()
    {
        var canFit = Day12.SolveLine("100x100: 1 1 1 1 1 1");
        Assert.True(canFit);
    }
}
