# tests/python/conftest.py
import sys
from pathlib import Path

# Add python/ folder to sys.path
repo_root = Path(__file__).resolve().parents[2]
python_root = repo_root / "python"

sys.path.insert(0, str(python_root))
