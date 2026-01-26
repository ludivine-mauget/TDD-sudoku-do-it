namespace Sudoku.API.DTOs;

public class SolveResponseDto
{
    public GridDto Grid { get; init; } = GridDto.CreateEmpty();
    public bool IsSolved { get; init; }
    public string Message { get; init; } = string.Empty;
}

