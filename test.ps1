#!/usr/bin/env pwsh

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==============================="
Write-Host " Running Advent of Code Tests "
Write-Host "==============================="
Write-Host ""

$repoRoot = Resolve-Path $PSScriptRoot

# -----------------------------
# C# Tests
# -----------------------------
$csharpTests = Join-Path $repoRoot "tests/AdventOfCode.CSharp.Tests/AdventOfCode.CSharp.Tests.csproj"

if (Test-Path $csharpTests) {
    Write-Host ""
    Write-Host ">>> Running C# tests"
    dotnet test $csharpTests
} else {
    Write-Warning "C# tests not found, skipping"
}

# -----------------------------
# F# Tests
# -----------------------------
$fsharpTests = Join-Path $repoRoot "tests/AdventOfCode.FSharp.Tests/AdventOfCode.FSharp.Tests.fsproj"

if (Test-Path $fsharpTests) {
    Write-Host ""
    Write-Host ">>> Running F# tests"
    dotnet test $fsharpTests
} else {
    Write-Warning "F# tests not found, skipping"
}

# -----------------------------
# Python Tests
# -----------------------------
$pythonRoot = Join-Path $repoRoot "python"
$pytestPath = Join-Path $repoRoot "tests/python"

if ((Test-Path $pythonRoot) -and (Test-Path $pytestPath)) {
    Write-Host ""
    Write-Host ">>> Running Python tests"

    Push-Location $pythonRoot
    try {
        # Prefer invoking pytest via python -m to avoid PATH issues
        python -m pytest ../tests/python
    }
    finally {
        Pop-Location
    }
} else {
    Write-Warning "Python tests not found, skipping"
}

Write-Host ""
Write-Host "✅ All tests completed"
