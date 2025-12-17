namespace AdventOfCode.Common;

public record GridCell(int Col, int Row)
{
    public bool IsAdjacentTo(GridCell other) =>
        Row == other.Row && Math.Abs(Col - other.Col) == 1 ||
        Col == other.Col && Math.Abs(Row - other.Row) == 1;
}