using Sudoku.Models;

namespace Sudoku.Tests;

public class ValidationTests
{
    [Fact]
    public void EmptyGrid_IsNotComplete()
    {
        // Arrange
        var grid = new Grid();

        // Act
        var isComplete = grid.IsComplete();

        // Assert
        Assert.False(isComplete);
    }

    [Fact]
    public void EmptyGrid_IsValid_ButNotSolved()
    {
        // Arrange
        var grid = new Grid();

        // Act
        var isValid = grid.IsValid();
        var isComplete = grid.IsComplete();
        var isSolved = grid.IsSolved();

        // Assert
        Assert.True(isValid); // Pas de conflits
        Assert.False(isComplete); // Pas remplie
        Assert.False(isSolved); // Pas résolue car pas complète
    }

    [Fact]
    public void PartiallyFilledGrid_WithoutConflicts_IsValid_ButNotSolved()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((0, 1), 2);
        grid.SetCellValue((1, 0), 3);

        // Act
        var isValid = grid.IsValid();
        var isComplete = grid.IsComplete();
        var isSolved = grid.IsSolved();

        // Assert
        Assert.True(isValid); // Pas de conflits
        Assert.False(isComplete); // Pas complètement remplie
        Assert.False(isSolved); // Pas résolue car pas complète
    }

    [Fact]
    public void PartiallyFilledGrid_WithConflicts_IsNotValid()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((0, 1), 1); // Conflit sur la ligne

        // Act
        var isValid = grid.IsValid();

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void CompleteValidGrid_IsSolved()
    {
        // Arrange - Créer une grille complète et valide
        var grid = Generator.Generator.GenerateFullGrid();

        // Act
        var isValid = grid.IsValid();
        var isComplete = grid.IsComplete();
        var isSolved = grid.IsSolved();

        // Assert
        Assert.True(isValid);
        Assert.True(isComplete);
        Assert.True(isSolved);
    }

    [Fact]
    public void CompleteInvalidGrid_IsNotSolved()
    {
        // Arrange - Créer une grille complète mais invalide
        var grid = new Grid();

        // Remplir toute la grille avec le chiffre 1 (invalide)
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                grid.SetCellNumber(row, col, 1);
            }
        }

        // Act
        var isValid = grid.IsValid();
        var isComplete = grid.IsComplete();
        var isSolved = grid.IsSolved();

        // Assert
        Assert.False(isValid); // Plein de conflits
        Assert.True(isComplete); // Toutes les cellules sont remplies
        Assert.False(isSolved); // Pas résolue car invalide
    }
}

