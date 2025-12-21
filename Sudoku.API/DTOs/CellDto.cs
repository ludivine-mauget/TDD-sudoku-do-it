namespace Sudoku.API.DTOs;

public class CellDto
{
    public int? Value { get; init; }
    public bool IsFixed { get; set; }
}

