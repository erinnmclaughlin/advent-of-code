from __future__ import annotations

import importlib
import pkgutil
from typing import Callable, Dict, Tuple

from . import y2025  # import year packages explicitly

SolverFunc = Callable[[str], tuple[str, str]]

_registry: Dict[Tuple[int, int], SolverFunc] | None = None


def _build_registry() -> Dict[Tuple[int, int], SolverFunc]:
    reg: Dict[Tuple[int, int], SolverFunc] = {}

    year_packages = [y2025]

    for year_pkg in year_packages:
        for _, modname, ispkg in pkgutil.walk_packages(
            year_pkg.__path__, year_pkg.__name__ + "."
        ):
            if ispkg:
                continue

            module = importlib.import_module(modname)

            year = getattr(module, "YEAR", None)
            day = getattr(module, "DAY", None)
            solve = getattr(module, "solve", None)

            if isinstance(year, int) and isinstance(day, int) and callable(solve):
                reg[(year, day)] = solve  # type: ignore[assignment]

    return reg


def get_solver(year: int, day: int) -> SolverFunc:
    global _registry

    if _registry is None:
        _registry = _build_registry()

    try:
        return _registry[(year, day)]
    except KeyError:
        raise KeyError(f"No Python solver for {year} day {day}")
