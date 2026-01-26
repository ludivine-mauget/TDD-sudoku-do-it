using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Sudoku.Models;

namespace Sudoku.Client.Services;

public class SudokuApiClient(HttpClient httpClient, ILogger<SudokuApiClient> logger) : ISudokuApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<ApiResult<Grid>> GenerateAsync(int difficulty)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/generate", new { cellsToRemove = difficulty });
            
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Erreur serveur: {response.StatusCode}";
                logger.LogWarning("Échec de génération: {StatusCode}", response.StatusCode);
                return ApiResult<Grid>.Error(errorMessage);
            }
            
            var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(_jsonOptions);
            var grid = result?.Grid != null ? ConvertGridDtoToGrid(result.Grid) : null;
            
            if (grid == null)
            {
                return ApiResult<Grid>.Error("La réponse du serveur est invalide");
            }
            
            return ApiResult<Grid>.Ok(grid);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Erreur de connexion lors de la génération");
            return ApiResult<Grid>.Error("Impossible de se connecter au serveur. Vérifiez votre connexion.");
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Timeout lors de la génération");
            return ApiResult<Grid>.Error("La requête a pris trop de temps. Réessayez.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur inattendue lors de la génération");
            return ApiResult<Grid>.Error("Une erreur inattendue s'est produite.");
        }
    }

    public async Task<ApiResult<(bool IsSolved, Grid Solution)>> SolveAsync(Grid grid)
    {
        try
        {
            var gridDto = ConvertGridToGridDto(grid);
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/solve", gridDto);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = $"Erreur serveur: {response.StatusCode}";
                logger.LogWarning("Échec de résolution: {StatusCode}", response.StatusCode);
                return ApiResult<(bool, Grid)>.Error(errorMessage);
            }
            
            var result = await response.Content.ReadFromJsonAsync<SolveResponse>(_jsonOptions);
            
            if (result == null)
            {
                return ApiResult<(bool, Grid)>.Error("La réponse du serveur est invalide");
            }
            
            var solution = ConvertGridDtoToGrid(result.Grid);
            
            if (solution == null)
            {
                return ApiResult<(bool, Grid)>.Error("La réponse du serveur est invalide");
            }
            
            return ApiResult<(bool, Grid)>.Ok((result.IsSolved, solution));
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Erreur de connexion lors de la résolution");
            return ApiResult<(bool, Grid)>.Error("Impossible de se connecter au serveur.");
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Timeout lors de la résolution");
            return ApiResult<(bool, Grid)>.Error("La requête a pris trop de temps.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur inattendue lors de la résolution");
            return ApiResult<(bool, Grid)>.Error("Une erreur inattendue s'est produite.");
        }
    }

    public async Task<ApiResult<bool>> ValidateAsync(Grid grid)
    {
        try
        {
            var gridDto = ConvertGridToGridDto(grid);
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/validate", gridDto);
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Échec de validation: {StatusCode}", response.StatusCode);
                return ApiResult<bool>.Error($"Erreur serveur: {response.StatusCode}");
            }
            
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse>(_jsonOptions);
            return ApiResult<bool>.Ok(result?.IsValid ?? false);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "Erreur de connexion lors de la validation");
            return ApiResult<bool>.Error("Impossible de se connecter au serveur.");
        }
        catch (TaskCanceledException ex)
        {
            logger.LogWarning(ex, "Timeout lors de la validation");
            return ApiResult<bool>.Error("La requête a pris trop de temps.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur inattendue lors de la validation");
            return ApiResult<bool>.Error("Une erreur inattendue s'est produite.");
        }
    }

    private Grid? ConvertGridDtoToGrid(GridDto? gridDto)
    {
        if (gridDto?.Cells == null)
            return null;

        var grid = new Grid();
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var cellValue = gridDto.Cells[row][col]?.Value;
                if (cellValue.HasValue && cellValue.Value > 0)
                {
                    grid.SetCellNumber(row, col, cellValue.Value);
                }
            }
        }
        return grid;
    }

    private GridDto ConvertGridToGridDto(Grid grid)
    {
        var gridDto = new GridDto
        {
            Cells = new CellDto[9][]
        };

        for (var row = 0; row < 9; row++)
        {
            gridDto.Cells[row] = new CellDto[9];
            for (var col = 0; col < 9; col++)
            {
                var cellValue = grid.GetCellNumber(row, col);
                gridDto.Cells[row][col] = new CellDto
                {
                    Value = cellValue == 0 ? null : cellValue,
                    IsFixed = cellValue != 0
                };
            }
        }

        return gridDto;
    }

    private class GenerateResponse
    {
        public GridDto? Grid { get; set; }
    }

    private class GridDto
    {
        public CellDto[][]? Cells { get; set; }
    }

    private class CellDto
    {
        public int? Value { get; set; }
        public bool IsFixed { get; set; }
    }

    private class SolveResponse
    {
        public bool IsSolved { get; set; }
        public GridDto? Grid { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    private class ValidationResponse
    {
        public bool IsValid { get; set; }
    }
}

