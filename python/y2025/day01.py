YEAR = 2025
DAY = 1

def solve(input_data: str) -> tuple[int, int]:
    current, part1, part2 = 50, 0, 0

    for instruction in [l for l in input_data.splitlines() if l.strip()]:
        inc = -1 if instruction[0] == 'L' else 1
        ticks = int(instruction[1:])
        
        for _ in range(ticks):
            current = (current + inc + 100) % 100
            if current == 0:
                part2 += 1
            
        if current == 0:
            part1 += 1

    return part1, part2