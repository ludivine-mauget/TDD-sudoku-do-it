namespace Sudoku.API.DTOs;

public class SolveResponseDto
{
    public GridDto Grid { get; init; } = new GridDto();
    public bool IsSolved { get; init; }
    public string Message { get; init; } = string.Empty;
}

