using Sudoku.API.DTOs;
using Sudoku.API.Extensions;
using Sudoku.Models;

namespace Sudoku.API.Services;

public class SudokuService(ILogger<SudokuService> logger) : ISudokuService
{
    public SudokuResponseDto GenerateSudoku(SudokuRequestDto request)
    {
        try
        {
            logger.LogInformation("Génération d'un puzzle avec difficulté {Difficulty}", request.Difficulty);

            var cellsToRemove = request.CellsToRemove ?? GetCellsToRemoveForDifficulty(request.Difficulty);
            var fullGrid = Generator.Generator.GenerateFullGrid();
            var sudokuGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
            var gridDto = sudokuGrid.ToGridDto();

            return new SudokuResponseDto
            {
                Grid = gridDto,
                Difficulty = request.Difficulty,
                CellsRemoved = cellsToRemove,
                CellsFilled = 81 - cellsToRemove,
                GeneratedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la génération du puzzle");
            throw;
        }
    }

    public SolveResponseDto SolveSudoku(GridDto gridDto)
    {
        try
        {
            logger.LogInformation("Résolution d'un puzzle");
            var grid = gridDto.ToGrid();
            var solvedGrid = Solver.Solver.Solve(grid);
            var isSolved = solvedGrid.IsSolved();

            return new SolveResponseDto
            {
                Grid = solvedGrid.ToGridDto(),
                IsSolved = isSolved,
                Message = isSolved ? "Puzzle résolu avec succès" : "Impossible de résoudre ce puzzle"
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la résolution du puzzle");
            throw;
        }
    }

    public ValidationResultDto ValidateSolution(GridDto gridDto)
    {
        try
        {
            logger.LogInformation("Validation d'une solution");

            var grid = gridDto.ToGrid();
            var isValid = grid.IsValid();
            var isComplete = grid.IsComplete();
            var errors = new List<CellErrorDto>();

            if (!isValid)
            {
                errors = FindErrors(grid);
            }

            string message;
            if (isComplete && isValid)
            {
                message = "Félicitations ! Le puzzle est résolu correctement.";
            }
            else if (!isValid)
            {
                message = $"La solution contient {errors.Count} erreur(s).";
            }
            else
            {
                message = "Le puzzle n'est pas encore terminé.";
            }

            return new ValidationResultDto
            {
                IsValid = isValid,
                IsComplete = isComplete,
                Errors = errors,
                Message = message
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la validation");
            throw;
        }
    }

    #region Méthodes privées

    private static int GetCellsToRemoveForDifficulty(DifficultyLevel difficulty)
    {
        var random = new Random();
        return difficulty switch
        {
            DifficultyLevel.Easy => random.Next(30, 36),      // 30-35
            DifficultyLevel.Medium => random.Next(40, 46),    // 40-45
            DifficultyLevel.Hard => random.Next(50, 56),      // 50-55
            DifficultyLevel.Expert => random.Next(56, 61),    // 56-60
            _ => 40
        };
    }


    private static List<CellErrorDto> FindErrors(Grid grid)
    {
        var errors = new List<CellErrorDto>();

        for (var row = 0; row < 9; row++)
        {
            var seen = new Dictionary<int, int>();
            for (var col = 0; col < 9; col++)
            {
                var value = grid.GetCellValue((row, col));
                if (!value.HasValue) continue;
                if (!seen.TryAdd(value.Value, col))
                {
                    errors.Add(new CellErrorDto
                    {
                        Row = row,
                        Col = col,
                        Description = $"Doublon dans la ligne {row + 1}: {value.Value}"
                    });
                }
            }
        }

        for (var col = 0; col < 9; col++)
        {
            var seen = new Dictionary<int, int>();
            for (var row = 0; row < 9; row++)
            {
                var value = grid.GetCellValue((row, col));
                if (!value.HasValue) continue;
                if (!seen.TryAdd(value.Value, row))
                {
                    errors.Add(new CellErrorDto
                    {
                        Row = row,
                        Col = col,
                        Description = $"Doublon dans la colonne {col + 1}: {value.Value}"
                    });
                }
            }
        }

        for (var boxRow = 0; boxRow < 3; boxRow++)
        {
            for (var boxCol = 0; boxCol < 3; boxCol++)
            {
                var seen = new Dictionary<int, (int, int)>();
                for (var row = 0; row < 3; row++)
                {
                    for (var col = 0; col < 3; col++)
                    {
                        var actualRow = boxRow * 3 + row;
                        var actualCol = boxCol * 3 + col;
                        var value = grid.GetCellValue((actualRow, actualCol));
                        if (!value.HasValue) continue;
                        if (seen.ContainsKey(value.Value))
                        {
                            errors.Add(new CellErrorDto
                            {
                                Row = actualRow,
                                Col = actualCol,
                                Description = $"Doublon dans la sous-grille: {value.Value}"
                            });
                        }
                        else
                        {
                            seen[value.Value] = (actualRow, actualCol);
                        }
                    }
                }
            }
        }

        return errors;
    }

    #endregion
}

