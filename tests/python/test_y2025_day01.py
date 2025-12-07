# tests/python/test_y2025_day01.py
from registry import get_solver

solver = get_solver(2025, 1)

sample = """\
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
"""

part1, part2 = solver(sample)

def test_sample_matches_part_one_problem_statement():
    assert part1 == 3

def test_sample_matches_part_two_problem_statement():
    assert part2 == 6
