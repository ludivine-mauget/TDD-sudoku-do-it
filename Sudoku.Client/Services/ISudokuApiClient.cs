using Sudoku.Models;

namespace Sudoku.Client.Services;

public interface ISudokuApiClient
{
    Task<Grid?> GenerateAsync(int difficulty);
    Task<(bool IsValid, Grid? Solution)> SolveAsync(Grid grid);
    Task<bool> ValidateAsync(Grid grid);
}

