#!/usr/bin/env pwsh

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==============================="
Write-Host " Running Advent of Code Tests "
Write-Host "==============================="

$repoRoot = Resolve-Path $PSScriptRoot

# -----------------------------
# C# Tests
# -----------------------------
$csharpTests = Join-Path $repoRoot "tests/csharp/AdventOfCode.CSharp.Tests.csproj"

if (Test-Path $csharpTests) {
    Write-Host ""
    Write-Host ">>> Running C# tests"
    dotnet test $csharpTests `
        --collect:"XPlat Code Coverage"
} else {
    Write-Warning "C# tests not found, skipping"
}

# -----------------------------
# F# Tests
# -----------------------------
$fsharpTests = Join-Path $repoRoot "tests/fsharp/AdventOfCode.FSharp.Tests.fsproj"

if (Test-Path $fsharpTests) {
    Write-Host ""
    Write-Host ">>> Running F# tests"
    dotnet test $fsharpTests `
        --collect:"XPlat Code Coverage"
} else {
    Write-Warning "F# tests not found, skipping"
}

# -----------------------------
# Python Tests
# -----------------------------
$pythonRoot = Join-Path $repoRoot "python"
$pytestPath = Join-Path $repoRoot "tests/python"

$hasPython = Test-Path $pythonRoot
$hasPyTests = Test-Path $pytestPath

if ($hasPython -and $hasPyTests) {
    Write-Host ""
    Write-Host ">>> Running Python tests"

    Push-Location $pythonRoot
    try {
        python -m pytest ../tests/python `
          --cov=. `
          --cov-report=xml `
          --cov-report=term
    }
    finally {
        Pop-Location
    }
} else {
    Write-Warning "Python tests not found, skipping"
}

Write-Host ""
Write-Host "✅ All tests completed"
