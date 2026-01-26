using Sudoku.Models;

namespace Sudoku.Client.Services;

/// <summary>
/// Résultat d'une opération API
/// </summary>
/// <typeparam name="T">Type de donnée retournée en cas de succès</typeparam>
public record ApiResult<T>(bool Success, T? Data, string? ErrorMessage)
{
    public static ApiResult<T> Ok(T data) => new(true, data, null);
    public static ApiResult<T> Error(string message) => new(false, default, message);
}

public interface ISudokuApiClient
{
    Task<ApiResult<Grid>> GenerateAsync(int difficulty);
    Task<ApiResult<(bool IsSolved, Grid Solution)>> SolveAsync(Grid grid);
    Task<ApiResult<bool>> ValidateAsync(Grid grid);
}

