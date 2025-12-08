namespace AdventOfCode.Y2024;

public sealed class Day04 : IAdventDay
{
    public int Year => 2024;
    public int Day => 4;

    public AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);
        return (SolvePartOne(lines), SolvePartTwo(lines));
    }

    private static int SolvePartOne(string[] fileLines)
    {
        var horizontal = CountHorizontal(fileLines);
        var vertical = CountVertical(fileLines);
        var diagonal = CountDiagonal(fileLines) + CountDiagonal(fileLines.Reverse().ToArray());

        return horizontal + vertical + diagonal;
    }

    private static int SolvePartTwo(string[] fileLines)
    {
        var count = 0;
        
        // pad cols & rows by 1 to ensure there's room for the "X" shape
        for (var i = 1; i < fileLines.Length - 1; i++)
        {
            for (var j = 1; j < fileLines[i].Length - 1; j++)
            {
                if (fileLines[i][j] is not 'A')
                    continue;

                var letters = new
                {
                    TopLeft = fileLines[i - 1][j - 1],
                    TopRight = fileLines[i - 1][j + 1],
                    BottomLeft = fileLines[i + 1][j - 1],
                    BottomRight = fileLines[i + 1][j + 1]
                };
                
                // make sure top left to bottom right diagonal is "MAS" or "SAM"
                if (letters is not { TopLeft: 'M', BottomRight: 'S' } and not { TopLeft: 'S', BottomRight: 'M' })
                    continue;
                
                // make sure top right to bottom left diagonal is "MAS" or "SAM"
                if (letters is not { TopRight: 'M', BottomLeft: 'S' } and not { TopRight: 'S', BottomLeft: 'M' })
                    continue;
                
                count++;
            }
        }

        return count;
    }
    
    private static int CountHorizontal(string[] lines)
    {
        return lines.Sum(line => CountXmas(line));
    }

    private static int CountVertical(string[] lines)
    {
        var sum = 0;
        
        for (var i = 0; i < lines[0].Length; i++)
        {
            var newLine = new char[lines.Length];
            
            for (var j = 0; j < lines.Length; j++)
            {
                newLine[j] = lines[j][i];
            }

            sum += CountXmas(newLine);
        }

        return sum;
    }

    private static int CountDiagonal(string[] lines)
    {
        var sum = 0;

        for (var i = 0; i < lines.Length - 3; i++)
        {
            for (var j = 0; j < lines[i].Length - 3; j++)
            {
                // just need to grab 4 chars at a time; we'll loop around to the rest of the diagonal
                char[] chars =
                [
                    lines[i][j],
                    lines[i + 1][j + 1],
                    lines[i + 2][j + 2],
                    lines[i + 3][j + 3]
                ];
                
                sum += CountXmas(chars);
            }
        }
        
        return sum;
    }

    private static int CountXmas(ReadOnlySpan<char> line)
    {
        var count = 0;

        for (var i = 0; i < line.Length; i++)
        {
            if (line[i..].StartsWith("XMAS") || line[i..].StartsWith("SAMX"))
                count++;
        }
        
        return count;
    }
}
