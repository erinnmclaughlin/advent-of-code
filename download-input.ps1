#!/usr/bin/env pwsh

param(
    [Parameter(Mandatory = $true)]
    [int] $Year,

    [Parameter(Mandatory = $true)]
    [int] $Day,
    
    [string] $SessionCookie
)

$ErrorActionPreference = "Stop"

if ($SessionCookie) {
    Write-Host "Storing Advent of Code session cookie in AOC_SESSION (user scope)..."

    [Environment]::SetEnvironmentVariable(
        "AOC_SESSION",
        $SessionCookie,
        "User"
    )

    # Make it available in this run
    $env:AOC_SESSION = $SessionCookie
}

$dayPadded = $Day.ToString("00")
$repoRoot = Resolve-Path (Join-Path $PSScriptRoot ".")
$inputDir = Join-Path $repoRoot "inputs/$Year"
$inputFile = Join-Path $inputDir "day$dayPadded.txt"

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

if (-not (Test-Path (Split-Path $inputFile -Parent))) {
    New-Item -ItemType Directory -Force `
        -Path (Split-Path $inputFile -Parent) | Out-Null
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
        Out-File -Encoding utf8 -FilePath $inputFile

    Write-Host "Saved input to $inputFile"
}
catch {
    throw "Failed to download input: $($_.Exception.Message)"
}

