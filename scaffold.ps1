#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory = $true)]
    [int] $Year,

    [Parameter(Mandatory = $true)]
    [int] $Day,

    # Which languages to scaffold; default is all
    [string[]] $Langs = @("csharp", "fsharp", "python")
)

$ErrorActionPreference = "Stop"

$dayPadded = "{0:00}" -f $Day
$repoRoot = Resolve-Path $PSScriptRoot

Write-Host "Scaffolding Year $Year Day $dayPadded..."
Write-Host ""

# ----------------------------
# Helper: create file if missing
# ----------------------------
function New-FileIfMissing {
    param(
        [string] $Path,
        [string] $Content
    )

    if (Test-Path $Path) {
        Write-Host "  - Skipping existing file: $Path"
    } else {
        $dir = Split-Path $Path -Parent
        if (-not (Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
        }
        $Content | Out-File -FilePath $Path -Encoding UTF8
        Write-Host "  - Created: $Path"
    }
}

# ----------------------------
# 1. Input file
# ----------------------------
$inputDir = Join-Path $repoRoot "inputs/$Year"
$inputFile = Join-Path $inputDir "day$dayPadded.txt"

New-FileIfMissing -Path $inputFile -Content ""

# ----------------------------
# 2. C# solution + tests
# ----------------------------
if ($Langs -contains "csharp") {
    Write-Host ""
    Write-Host "C#:"
    
    $csDir = Join-Path $repoRoot "csharp/Y$Year"
    $csFile = Join-Path $csDir "Day$dayPadded.cs"

    $csContent = @"
namespace AdventOfCode.Y$Year;

public sealed class Day$dayPadded : IAdventDay
{
    public int Year => $Year;
    public int Day => $Day;

    public AdventDaySolution Solve(string input)
    {
        var lines = input.Split(
            ['\r', '\n'],
            StringSplitOptions.RemoveEmptyEntries
        );

        // TODO: implement puzzle logic here

        var part1 = "";
        var part2 = "";

        return (part1, part2);
    }
}
"@

    New-FileIfMissing -Path $csFile -Content $csContent

    # C# tests
    $csTestDir = Join-Path $repoRoot "tests/csharp/Y$Year"
    $csTestFile = Join-Path $csTestDir "Day${dayPadded}Tests.cs"

    $csTestContent = @"
namespace AdventOfCode.Tests.Y$Year;

public class Day${dayPadded}Tests
{
    private readonly IAdventDay _solver = SolverRegistry.Get($Year, $Day);

    private const string Sample = "REPLACE THIS WITH SAMPLE INPUT";

    [Fact]
    public void Sample_matches_part_one_problem_statement()
    {
        var (p1, _) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
    }

    [Fact]
    public void Sample_matches_part_two_problem_statement()
    {
        var (_, p2) = _solver.Solve(Sample);
        Assert.Skip("Not implemented");
    }
}
"@

    New-FileIfMissing -Path $csTestFile -Content $csTestContent
}

# ----------------------------
# 3. F# solution + tests
# ----------------------------
if ($Langs -contains "fsharp") {
    Write-Host ""
    Write-Host "F#:"

    $fsDir = Join-Path $repoRoot "fsharp/Y$Year"
    $fsFile = Join-Path $fsDir "Day$dayPadded.fs"

    $fsContent = @"
namespace AdventOfCode.Y$Year

open AdventOfCode
open System

type Day$dayPadded() =
    interface IAdventDay with
        member _.Year = $Year
        member _.Day = $Day

        member _.Solve(input: string) : string * string =
            let lines =
                input.Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries |||
                    StringSplitOptions.TrimEntries
                )
                |> Array.toList

            // TODO: implement puzzle logic here

            let part1 = ""
            let part2 = ""
            part1, part2
"@

    New-FileIfMissing -Path $fsFile -Content $fsContent

    # Add Compile include to fsharp/AdventOfCode.FSharp.fsproj
    $fsprojPath = Join-Path $repoRoot "fsharp/AdventOfCode.FSharp.fsproj"
    $fsprojContent = Get-Content $fsprojPath -Raw
    
    $compileEntry = "    <Compile Include=""Y$Year\Day$dayPadded.fs"" />"
    
    # Check if entry already exists
    if ($fsprojContent -notmatch [regex]::Escape($compileEntry)) {
        # Find the location to insert: before <Compile Include="Program.fs" />
        $programfsLine = '    <Compile Include="Program.fs" />'
        
        if ($fsprojContent -match [regex]::Escape($programfsLine)) {
            $fsprojContent = $fsprojContent -replace [regex]::Escape($programfsLine), "$compileEntry`r`n`r`n$programfsLine"
            $fsprojContent | Out-File -FilePath $fsprojPath -Encoding UTF8 -NoNewline
            Write-Host "  - Added entry to .fsproj: Y$Year\Day$dayPadded.fs"
        } else {
            Write-Host "  - Warning: Could not find Program.fs line in .fsproj to add entry"
        }
    }

    # F# tests
    $fsTestDir = Join-Path $repoRoot "tests/fsharp/Y$Year"
    $fsTestFile = Join-Path $fsTestDir "Day${dayPadded}Tests.fs"

    $fsTestContent = @"
namespace AdventOfCode.Tests.Y$Year

open AdventOfCode
open System
open Xunit

type Day${dayPadded}Tests() =

    let solver : IAdventDay = SolverRegistry.get $Year $Day

    let sample = "REPLACE THIS WITH SAMPLE INPUT"

    [<Fact>]
    member _.Sample_matches_part_one_problem_statement() =
        Assert.Skip("Not implemented")

    [<Fact>]
    member _.Sample_matches_part_two_problem_statement() =
        Assert.Skip("Not implemented")
"@

    New-FileIfMissing -Path $fsTestFile -Content $fsTestContent
}

# ----------------------------
# 4. Python solution + tests
# ----------------------------
if ($Langs -contains "python") {
    Write-Host ""
    Write-Host "Python:"

    $pyYearDir = Join-Path $repoRoot "python/y$Year"
    $pyFile = Join-Path $pyYearDir "day$dayPadded.py"

    $pyContent = @"
YEAR = $Year
DAY = $Day


def solve(input_data: str) -> tuple[str, str]:
    lines = [line for line in input_data.splitlines() if line.strip()]

    # TODO: implement puzzle logic here

    part1 = ""
    part2 = ""
    return part1, part2
"@

    New-FileIfMissing -Path $pyFile -Content $pyContent

    # Python tests
    $pyTestDir = Join-Path $repoRoot "tests/python"
    $pyTestFile = Join-Path $pyTestDir "test_y$Year`_day$dayPadded.py"

    $pyTestContent = @"
from pathlib import Path

from registry import get_solver


def repo_root() -> Path:
    return Path(__file__).resolve().parents[2]


def read_input(year: int, day: int) -> str:
    path = repo_root() / "inputs" / str(year) / f"day{day:02}.txt"
    return path.read_text()


def test_sample_matches_problem_statement():
    solver = get_solver($Year, $Day)

    sample = "REPLACE THIS WITH SAMPLE INPUT"

    part1, part2 = solver(sample)

    # TODO: replace expected values
    assert part1 == ""
    assert part2 == ""


def test_real_input_runs():
    solver = get_solver($Year, $Day)
    data = read_input($Year, $Day)

    part1, part2 = solver(data)

    # TODO: once solved, lock in answers:
    # assert part1 == ""
    # assert part2 == ""
"@

    New-FileIfMissing -Path $pyTestFile -Content $pyTestContent
}

Write-Host ""
Write-Host "Done."
