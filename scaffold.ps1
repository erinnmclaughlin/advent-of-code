#!/usr/bin/env pwsh
param(
    [Parameter(Mandatory = $true)]
    [int] $Year,

    [Parameter(Mandatory = $true)]
    [int] $Day,

    # Which languages to scaffold; default is all
    [string[]] $Langs = @("csharp", "fsharp", "python"),

    # Force a fresh download of the input
    [switch] $DownloadInput
)

$ErrorActionPreference = "Stop"

$dayPadded = "{0:00}" -f $Day
$repoRoot = Resolve-Path $PSScriptRoot

Write-Host "Scaffolding Year $Year Day $dayPadded..."
Write-Host ""


# ----------------------------
# AoC input download helper
# ----------------------------
function Get-AocInput {
    param(
        [int] $Year,
        [int] $Day,
        [string] $DestinationPath
    )

    $session = $env:AOC_SESSION
    if (-not $session) {
        throw @"
Advent of Code session cookie not found.

Set it once with:
  pwsh ./run.ps1 -SetSessionCookie "<your-cookie>" ...

Or manually set AOC_SESSION in your environment.
"@
    }

    $uri = "https://adventofcode.com/$Year/day/$Day/input"
    Write-Host "Downloading input from $uri..."

    if (-not (Test-Path (Split-Path $DestinationPath -Parent))) {
        New-Item -ItemType Directory -Force `
            -Path (Split-Path $DestinationPath -Parent) | Out-Null
    }

    $headers = @{
        Cookie = "session=$session"
    }

    try {
        Invoke-WebRequest `
            -Uri $uri `
            -Headers $headers `
            -UseBasicParsing |
            Select-Object -ExpandProperty Content |
            ForEach-Object { $_.TrimEnd("`r", "`n") } |
            Out-File -Encoding utf8 -FilePath $DestinationPath

        Write-Host "Saved input to $DestinationPath"
    }
    catch {
        throw "Failed to download input: $($_.Exception.Message)"
    }
}

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

if ($DownloadInput -or -not (Test-Path $inputFile)) {
    Get-AocInput -Year $Year -Day $Day -DestinationPath $inputFile
}

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

public sealed class Day$dayPadded() : AdventDay($Year, $Day)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);

        // TODO: implement puzzle logic here

        return ("", "");
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

    private const string Sample = """
    REPLACE THIS WITH SAMPLE INPUT
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

    # Add + sort Compile includes for puzzle sources in fsharp/AdventOfCode.FSharp.fsproj
    $fsprojPath = Join-Path $repoRoot "fsharp/AdventOfCode.FSharp.fsproj"
    $fsprojContent = Get-Content $fsprojPath -Raw


    # Collect existing <Compile Include="Y...Day...fs" /> lines (accept either slash)
    $compileTagMatches = [regex]::Matches($fsprojContent, '    <Compile Include="Y\d{4}[\\/]Day\d{2}\.fs" />')
    $compileLines = @()
    foreach ($m in $compileTagMatches) {
        $compileLines += $m.Value
    }
    # Add the new entry as a string (always)
    $newTag = '    <Compile Include="Y' + $Year + '\Day' + $dayPadded + '.fs" />'
    $compileLines += $newTag
    # Deduplicate and sort alphabetically
    $sortedCompileLines = ($compileLines | Sort-Object | Select-Object -Unique) -join "`r`n"

    # Remove any existing Y... Day... lines so we can re-insert sorted block
    $fsprojContentClean = [regex]::Replace($fsprojContent, '(?m)^\s*<Compile Include="Y\d{4}[\\/]Day\d{2}\.fs" />\s*\r?\n', '')

    # Insert before Program.fs if present, else fallback to insert before the first </ItemGroup>
    $programfsLine = '    <Compile Include="Program.fs" />'
    if ($fsprojContentClean -match [regex]::Escape($programfsLine)) {
        $fsprojContentNew = $fsprojContentClean -replace [regex]::Escape($programfsLine), "$sortedCompileLines`r`n`r`n$programfsLine"
        $fsprojContentNew | Out-File -FilePath $fsprojPath -Encoding UTF8 -NoNewline
        Write-Host "  - Updated .fsproj with sorted puzzle entries"
    } else {
        # Fallback: insert into the first ItemGroup that contains other Compile entries
        if ($fsprojContentClean -match '(?s)<ItemGroup>.*?</ItemGroup>') {
            $closingTag = '</ItemGroup>'
            $idx = $fsprojContentClean.IndexOf($closingTag)
            if ($idx -ge 0) {
                $before = $fsprojContentClean.Substring(0, $idx)
                $after = $fsprojContentClean.Substring($idx)
                $fsprojContentNew = $before + "`r`n" + $sortedCompileLines + "`r`n`r`n" + $after
                $fsprojContentNew | Out-File -FilePath $fsprojPath -Encoding UTF8 -NoNewline
                Write-Host "  - Updated .fsproj (fallback insertion) with sorted puzzle entries"
            } else {
                Write-Host "  - Warning: Could not find closing </ItemGroup> to insert entries"
            }
        } else {
            Write-Host "  - Warning: Could not find insertion point to update .fsproj"
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

    # Add + sort Compile includes for test files in tests/fsharp/AdventOfCode.FSharp.Tests.fsproj
    $fsTestProjPath = Join-Path $repoRoot "tests/fsharp/AdventOfCode.FSharp.Tests.fsproj"
    $fsTestProjContent = Get-Content $fsTestProjPath -Raw


    # Collect existing <Compile Include="Y...Day...Tests.fs" /> lines (accept either slash)
    $testTagMatches = [regex]::Matches($fsTestProjContent, '    <Compile Include="Y\d{4}[\\/]Day\d{2}Tests\.fs" />')
    $testLines = @()
    foreach ($m in $testTagMatches) {
        $testLines += $m.Value
    }
    # Add the new entry as a string (always)
    $newTestTag = '    <Compile Include="Y' + $Year + '\Day' + $dayPadded + 'Tests.fs" />'
    $testLines += $newTestTag
    # Deduplicate and sort alphabetically
    $sortedTestLines = ($testLines | Sort-Object | Select-Object -Unique) -join "`r`n"

    # Remove existing test Day lines then insert sorted block before xunit runner content if possible
    $fsTestProjContentClean = [regex]::Replace($fsTestProjContent, '(?m)^\s*<Compile Include="Y\d{4}[\\/]Day\d{2}Tests\.fs" />\s*\r?\n', '')

    $xunitLine = '    <Content Include="xunit.runner.json" CopyToOutputDirectory="PreserveNewest" />'
    if ($fsTestProjContentClean -match [regex]::Escape($xunitLine)) {
        $fsTestProjContentNew = $fsTestProjContentClean -replace [regex]::Escape($xunitLine), "$sortedTestLines`r`n$xunitLine"
        $fsTestProjContentNew | Out-File -FilePath $fsTestProjPath -Encoding UTF8 -NoNewline
        Write-Host "  - Updated test .fsproj with sorted test entries"
    } else {
        # Fallback to inserting into first ItemGroup
        if ($fsTestProjContentClean -match '(?s)<ItemGroup>.*?</ItemGroup>') {
            $closingTag = '</ItemGroup>'
            $idx = $fsTestProjContentClean.IndexOf($closingTag)
            if ($idx -ge 0) {
                $before = $fsTestProjContentClean.Substring(0, $idx)
                $after = $fsTestProjContentClean.Substring($idx)
                $fsTestProjContentNew = $before + "`r`n" + $sortedTestLines + "`r`n" + $after
                $fsTestProjContentNew | Out-File -FilePath $fsTestProjPath -Encoding UTF8 -NoNewline
                Write-Host "  - Updated test .fsproj (fallback insertion) with sorted test entries"
            } else {
                Write-Host "  - Warning: Could not find closing </ItemGroup> to insert test entries"
            }
        } else {
            Write-Host "  - Warning: Could not find insertion point to update test .fsproj"
        }
    }
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
