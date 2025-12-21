namespace Sudoku.API.DTOs;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    Expert
}

public class SudokuRequestDto
{
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;
    public int? CellsToRemove { get; set; }
}

