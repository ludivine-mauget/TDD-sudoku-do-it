using Microsoft.Extensions.Logging;
using Moq;
using Sudoku.API.DTOs;
using Sudoku.API.Services;
using Sudoku.Models;

namespace Sudoku.Tests;

public class SudokuServiceValidationTests
{
    private readonly SudokuService _sudokuService;
    private readonly Mock<ILogger<SudokuService>> _loggerMock;

    public SudokuServiceValidationTests()
    {
        _loggerMock = new Mock<ILogger<SudokuService>>();
        _sudokuService = new SudokuService(_loggerMock.Object);
    }

    [Fact]
    public void ValidateSolution_EmptyGrid_ReturnsInvalid()
    {
        // Arrange
        var gridDto = CreateEmptyGridDto();

        // Act
        var result = _sudokuService.ValidateSolution(gridDto);

        // Assert
        Assert.False(result.IsValid, "Une grille vide ne devrait pas être valide");
        Assert.False(result.IsComplete, "Une grille vide n'est pas complète");
        Assert.Empty(result.Errors);
        Assert.Equal("Le puzzle n'est pas encore terminé.", result.Message);
    }

    [Fact]
    public void ValidateSolution_PartiallyFilledGrid_WithoutConflicts_ReturnsInvalid()
    {
        // Arrange
        var gridDto = CreateEmptyGridDto();
        gridDto.Cells[0][0] = new CellDto { Value = 1, IsFixed = false };
        gridDto.Cells[0][1] = new CellDto { Value = 2, IsFixed = false };
        gridDto.Cells[1][0] = new CellDto { Value = 3, IsFixed = false };

        // Act
        var result = _sudokuService.ValidateSolution(gridDto);

        // Assert
        Assert.False(result.IsValid, "Une grille partiellement remplie ne devrait pas être valide");
        Assert.False(result.IsComplete, "Une grille partiellement remplie n'est pas complète");
        Assert.Empty(result.Errors);
        Assert.Equal("Le puzzle n'est pas encore terminé.", result.Message);
    }

    [Fact]
    public void ValidateSolution_PartiallyFilledGrid_WithConflicts_ReturnsInvalidWithErrors()
    {
        // Arrange
        var gridDto = CreateEmptyGridDto();
        gridDto.Cells[0][0] = new CellDto { Value = 1, IsFixed = false };
        gridDto.Cells[0][1] = new CellDto { Value = 1, IsFixed = false }; // Conflit sur la ligne

        // Act
        var result = _sudokuService.ValidateSolution(gridDto);

        // Assert
        Assert.False(result.IsValid, "Une grille avec des conflits ne devrait pas être valide");
        Assert.False(result.IsComplete, "Une grille partiellement remplie n'est pas complète");
        Assert.NotEmpty(result.Errors);
        Assert.Contains("erreur", result.Message.ToLower());
    }

    [Fact]
    public void ValidateSolution_CompleteValidGrid_ReturnsValid()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        var gridDto = ConvertGridToDto(fullGrid);

        // Act
        var result = _sudokuService.ValidateSolution(gridDto);

        // Assert
        Assert.True(result.IsValid, "Une grille complète et correcte devrait être valide");
        Assert.True(result.IsComplete, "Une grille remplie est complète");
        Assert.Empty(result.Errors);
        Assert.Equal("Félicitations ! Le puzzle est résolu correctement.", result.Message);
    }

    [Fact]
    public void ValidateSolution_CompleteInvalidGrid_ReturnsInvalidWithErrors()
    {
        // Arrange - Créer une grille complète mais invalide
        var gridDto = CreateEmptyGridDto();

        // Remplir toute la grille avec le chiffre 1 (invalide)
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                gridDto.Cells[row][col] = new CellDto { Value = 1, IsFixed = false };
            }
        }

        // Act
        var result = _sudokuService.ValidateSolution(gridDto);

        // Assert
        Assert.False(result.IsValid, "Une grille invalide ne devrait pas être valide même si complète");
        Assert.True(result.IsComplete, "Une grille remplie est complète");
        Assert.NotEmpty(result.Errors);
        Assert.Contains("erreur", result.Message.ToLower());
    }

    private static GridDto CreateEmptyGridDto()
    {
        return new GridDto();
    }

    private static GridDto ConvertGridToDto(Grid grid)
    {
        var gridDto = new GridDto();
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var value = grid.GetCellValue((row, col));
                gridDto.Cells[row][col] = new CellDto { Value = value, IsFixed = value.HasValue };
            }
        }
        return gridDto;
    }
}

