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
# Repo paths
# ----------------------------
$dayPadded = $Day.ToString("00")
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot ".")
$inputDir = Join-Path $repoRoot "inputs/$Year"
$inputFile = Join-Path $inputDir "day$dayPadded.txt"

# ----------------------------
# Ensure input exists
# ----------------------------
if ($DownloadInput -or -not (Test-Path $inputFile)) {
    $aocDownloader = Join-Path $PSScriptRoot "download-input.ps1"
    & $aocDownloader -Year $Year -Day $Day -SessionCookie $SetSessionCookie
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
    $languagesToRun = @("csharp", "fsharp"<#, "python"#>)
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
        #"python" { Invoke-Python -RepoRoot $repoRoot -InputFile $inputFile -Year $Year -Day $Day -RunningAll $runningAll }
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
