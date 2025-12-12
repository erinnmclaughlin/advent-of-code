namespace AdventOfCode.Common;

public record AdventCell(int Row, int Col)
{
    public bool IsAdjacentTo(AdventCell other) =>
        Row == other.Row && Math.Abs(Col - other.Col) == 1 ||
        Col == other.Col && Math.Abs(Row - other.Row) == 1;
}