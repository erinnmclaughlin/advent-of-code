# Advent of Code

This repository contains my Advent of Code solutions.

[![CI](https://github.com/erinnmclaughlin/advent-of-code/actions/workflows/ci.yml/badge.svg)](https://github.com/erinnmclaughlin/advent-of-code/actions/workflows/ci.yml)
[![codecov](https://codecov.io/gh/erinnmclaughlin/advent-of-code/graph/badge.svg?token=mErAFxucHi)](https://codecov.io/gh/erinnmclaughlin/advent-of-code)

### Languages
[C#](csharp) | [F#](fsharp) | [Python](python)

### Running (PowerShell)

#### `run.ps1` — Run Solutions

Executes a specific day's solution across available languages (C#, F#, Python).

**Parameters:**
- `-Year <int>` *(required)* — Advent of Code year (e.g., `2025`, `2024`)
- `-Day <int>` *(required)* — Day number (e.g., `1`–`25`)
- `-Lang <string>` *(optional)* — Specific language to run: `csharp`, `fsharp`, or `python`. If omitted, runs all available languages.
- `-DownloadInput` *(optional, switch)* — Force a fresh download of the puzzle input from Advent of Code.
- `-SetSessionCookie <string>` *(optional)* — Set or update your AoC session cookie (stored in `AOC_SESSION` environment variable, user scope).

**Examples:**
```ps
# Run Day 1, 2025 across all languages
.\run.ps1 -Year 2025 -Day 1

# Run Day 5, 2024 using only C#
.\run.ps1 -Year 2024 -Day 5 -Lang csharp

# Set session cookie and run
.\run.ps1 -Year 2025 -Day 1 -SetSessionCookie "your-cookie-here"

# Force download of input and run
.\run.ps1 -Year 2025 -Day 3 -DownloadInput
```

#### `test.ps1` — Run Tests

Runs all test suites across languages (C#, F#, Python) with code coverage reporting.

**Parameters:**
- None. This script automatically discovers and runs tests in:
  - `tests/csharp/AdventOfCode.CSharp.Tests.csproj`
  - `tests/fsharp/AdventOfCode.FSharp.Tests.fsproj`
  - `tests/python/` (via `pytest`)

**Examples:**
```ps
# Run all tests
.\test.ps1
```

#### `scaffold.ps1` — Scaffold a New Day

Creates boilerplate solution files, test files, and optionally downloads the puzzle input for a new day.

**Parameters:**
- `-Year <int>` *(required)* — Year (e.g., `2025`)
- `-Day <int>` *(required)* — Day number (e.g., `1`–`25`)
- `-Langs <string[]>` *(optional)* — Which languages to scaffold. Default: `@("csharp", "fsharp", "python")`. Pass a subset like `@("csharp", "python")` to skip F#.
- `-DownloadInput` *(optional, switch)* — Download puzzle input from Advent of Code (requires `AOC_SESSION` cookie to be set).

**Examples:**
```ps
# Scaffold Day 8, 2025 for all languages
.\scaffold.ps1 -Year 2025 -Day 8

# Scaffold for C# and Python only, skip F#
.\scaffold.ps1 -Year 2025 -Day 8 -Langs @("csharp", "python")

# Scaffold and download input
.\scaffold.ps1 -Year 2025 -Day 8 -DownloadInput
```
