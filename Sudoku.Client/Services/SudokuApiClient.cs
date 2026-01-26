using System.Net.Http.Json;
using System.Text.Json;
using Sudoku.Models;

namespace Sudoku.Client.Services;

public class SudokuApiClient(HttpClient httpClient) : ISudokuApiClient
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<Grid?> GenerateAsync(int difficulty)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/generate", new { cellsToRemove = difficulty });
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(_jsonOptions);
            return result?.Grid != null ? ConvertGridDtoToGrid(result.Grid) : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating sudoku: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool IsValid, Grid? Solution)> SolveAsync(Grid grid)
    {
        try
        {
            var gridDto = ConvertGridToGridDto(grid);
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/solve", gridDto);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<SolveResponse>(_jsonOptions);
            var solution = ConvertGridDtoToGrid(result?.Grid);
            return (result?.IsSolved ?? false, solution);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error solving sudoku: {ex.Message}");
            return (false, null);
        }
    }

    public async Task<bool> ValidateAsync(Grid grid)
    {
        try
        {
            var gridDto = ConvertGridToGridDto(grid);
            var response = await httpClient.PostAsJsonAsync("/api/sudoku/validate", gridDto);
            response.EnsureSuccessStatusCode();
            
            var result = await response.Content.ReadFromJsonAsync<ValidationResponse>(_jsonOptions);
            return result?.IsValid ?? false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating sudoku: {ex.Message}");
            return false;
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

