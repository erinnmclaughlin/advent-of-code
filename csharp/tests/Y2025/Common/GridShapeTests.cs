using AdventOfCode.Y2025.Common;

namespace AdventOfCode.Tests.Y2025.Common;

public sealed class GridShapeTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Bounding_box_is_correctly_created_for_grid_shape()
    {
        var shape = GridShape.CreateFromCorners(
            new GridCell(7, 1),
            new GridCell(11, 1),
            new GridCell(11, 7),
            new GridCell(9, 7),
            new GridCell(9, 5),
            new GridCell(2, 5),
            new GridCell(2, 3),
            new GridCell(7, 3)
        );

        outputHelper.WriteLine(shape.ToString());
        
        Assert.Equal(
            """
            .....#####
            .....#####
            ##########
            ##########
            ##########
            .......###
            .......###
            """,
            shape.ToString()
        );
    }
}