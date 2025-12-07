#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true)]
    [int] $Year,

    [Parameter(Mandatory = $true)]
    [int] $Day,

    [Parameter(Mandatory = $true)]
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
  pwsh ./scripts/run.ps1 -SetSessionCookie "<your-cookie>" ...

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
# Run selected language
# ----------------------------
switch ($Lang.ToLower()) {

    "csharp" {
        
        $csProject = Join-Path $repoRoot "csharp/AdventOfCode.csproj"

        if (-not (Test-Path $csProject)) {
            Write-Error "C# project not found: $csProject"
            exit 1
        }

        Get-Content $inputFile -Raw |
            dotnet run `
                --project $csProject `
                -- $Year $Day
    }

    "fsharp" {
        $fsProject = Join-Path $repoRoot "fsharp/AdventOfCode.fsproj"

        if (-not (Test-Path $fsProject)) {
            Write-Error "F# project not found: $fsProject"
            exit 1
        }

        Get-Content $inputFile -Raw |
            dotnet run `
                --project $fsProject `
                -- $Year $Day
    }

    "python" {
        $pythonRoot = Join-Path $repoRoot "python"

        if (-not (Test-Path $pythonRoot)) {
            Write-Error "Python folder not found: $pythonRoot"
            exit 1
        }

        Push-Location $pythonRoot
        try {
            Get-Content $inputFile -Raw |
                python -m advent_of_code.cli $Year $Day
        }
        finally {
            Pop-Location
        }
    }


    default {
        Write-Error "Unknown language: $Lang"
        exit 1
    }
}
