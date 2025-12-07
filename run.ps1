#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true)]
    [int] $Year,

    [Parameter(Mandatory = $true)]
    [int] $Day,

    # If omitted or empty, we'll run all languages that exist (csharp, fsharp, python)
    [string] $Lang,

    # Force a fresh download of the input
    [switch] $DownloadInput,

    # Set AoC session cookie and persist it
    [string] $SetSessionCookie
)

$ErrorActionPreference = "Stop"

# ----------------------------
# Save session cookie (if supplied)
# ----------------------------
if ($SetSessionCookie) {
    Write-Host "Storing Advent of Code session cookie in AOC_SESSION (user scope)..."

    [Environment]::SetEnvironmentVariable(
        "AOC_SESSION",
        $SetSessionCookie,
        "User"
    )

    # Make it available in this run
    $env:AOC_SESSION = $SetSessionCookie
}

# ----------------------------
# Repo paths
# ----------------------------
$dayPadded = $Day.ToString("00")

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot ".")
$inputDir = Join-Path $repoRoot "inputs/$Year"
$inputFile = Join-Path $inputDir "day$dayPadded.txt"

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
# Ensure input exists
# ----------------------------
if ($DownloadInput -or -not (Test-Path $inputFile)) {
    Get-AocInput -Year $Year -Day $Day -DestinationPath $inputFile
}

if (-not (Test-Path $inputFile)) {
    Write-Error "Input file not found: $inputFile"
    exit 1
}

# ----------------------------
# Determine which languages to run
# ----------------------------
$runningAll = [string]::IsNullOrWhiteSpace($Lang)

if ($runningAll) {
    # Default: attempt all three
    $languagesToRun = @("csharp", "fsharp", "python")
} else {
    $languagesToRun = @($Lang.ToLower())
}

# ----------------------------
# Helpers for per-language execution
# ----------------------------
function Invoke-CSharp {
    param(
        [string] $RepoRoot,
        [string] $InputFile,
        [int] $Year,
        [int] $Day,
        [bool] $RunningAll
    )

    $csProject = Join-Path $RepoRoot "csharp/AdventOfCode.CSharp.csproj"

    if (-not (Test-Path $csProject)) {
        if ($RunningAll) {
            Write-Warning "Skipping C# – project not found: $csProject"
            return
        } else {
            Write-Error "C# project not found: $csProject"
            exit 1
        }
    }

    Write-Host ""
    Write-Host "=== C# ($Year Day $Day) ==="
    Get-Content $InputFile -Raw |
        dotnet run `
            --project $csProject `
            -- $Year $Day
}

function Invoke-FSharp {
    param(
        [string] $RepoRoot,
        [string] $InputFile,
        [int] $Year,
        [int] $Day,
        [bool] $RunningAll
    )

    $fsProject = Join-Path $RepoRoot "fsharp/AdventOfCode.FSharp.fsproj"

    if (-not (Test-Path $fsProject)) {
        if ($RunningAll) {
            Write-Warning "Skipping F# – project not found: $fsProject"
            return
        } else {
            Write-Error "F# project not found: $fsProject"
            exit 1
        }
    }

    Write-Host ""
    Write-Host "=== F# ($Year Day $Day) ==="
    Get-Content $InputFile -Raw |
        dotnet run `
            --project $fsProject `
            -- $Year $Day
}

function Invoke-Python {
    param(
        [string] $RepoRoot,
        [string] $InputFile,
        [int] $Year,
        [int] $Day,
        [bool] $RunningAll
    )

    $pythonRoot = Join-Path $RepoRoot "python"

    if (-not (Test-Path $pythonRoot)) {
        if ($RunningAll) {
            Write-Warning "Skipping Python – folder not found: $pythonRoot"
            return
        } else {
            Write-Error "Python folder not found: $pythonRoot"
            exit 1
        }
    }

    Write-Host ""
    Write-Host "=== Python ($Year Day $Day) ==="

    Push-Location $pythonRoot
    try {
        Get-Content $InputFile -Raw |
            python cli.py $Year $Day
    }
    finally {
        Pop-Location
    }
}

# ----------------------------
# Run one or more languages
# ----------------------------
foreach ($language in $languagesToRun) {
    switch ($language) {
        "csharp" { Invoke-CSharp -RepoRoot $repoRoot -InputFile $inputFile -Year $Year -Day $Day -RunningAll $runningAll }
        "fsharp" { Invoke-FSharp -RepoRoot $repoRoot -InputFile $inputFile -Year $Year -Day $Day -RunningAll $runningAll }
        "python" { Invoke-Python -RepoRoot $repoRoot -InputFile $inputFile -Year $Year -Day $Day -RunningAll $runningAll }
        default {
            if ($runningAll) {
                Write-Warning "Unknown language in list: $language"
            } else {
                Write-Error "Unknown language: $language"
                exit 1
            }
        }
    }
}

Write-Host ""
