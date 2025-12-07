import sys
from registry import get_solver


def main() -> int:
    if len(sys.argv) < 3:
        print("Usage: python -m advent_of_code.cli <year> <day>", file=sys.stderr)
        return 1

    year = int(sys.argv[1])
    day = int(sys.argv[2])

    try:
        solver = get_solver(year, day)
    except KeyError as ex:
        print(str(ex), file=sys.stderr)
        return 1

    input_data = sys.stdin.read()
    if not input_data.strip():
        print("No input provided on stdin.", file=sys.stderr)
        return 1

    part1, part2 = solver(input_data)

    print(f"Part 1: {part1}")
    print(f"Part 2: {part2}")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
