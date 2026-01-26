namespace Sudoku.API.DTOs;

public class SudokuResponseDto
{
    public GridDto Grid { get; init; } = new GridDto();
    public DifficultyLevel Difficulty { get; init; }
    public int CellsRemoved { get; init; }
    public int CellsFilled { get; init; }
    public DateTime GeneratedAt { get; init; } = DateTime.UtcNow;
}

