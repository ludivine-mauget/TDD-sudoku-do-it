using Sudoku.API.DTOs;

namespace Sudoku.API.Services;

public interface ISudokuService
{
    SudokuResponseDto GenerateSudoku(SudokuRequestDto request);
    SolveResponseDto SolveSudoku(GridDto gridDto);
    ValidationResultDto ValidateSolution(GridDto gridDto);
}

