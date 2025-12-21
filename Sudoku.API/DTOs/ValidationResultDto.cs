namespace Sudoku.API.DTOs;

public class ValidationResultDto
{
    public bool IsValid { get; init; }
    public bool IsComplete { get; init; }
    public List<CellErrorDto> Errors { get; init; } = [];
    public string Message { get; init; } = string.Empty;
}

public class CellErrorDto
{
    public int Row { get; set; }
    public int Col { get; set; }

    public string Description { get; set; } = string.Empty;
}

