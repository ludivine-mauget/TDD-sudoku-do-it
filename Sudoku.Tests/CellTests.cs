using Sudoku.Models;

namespace Sudoku.Tests;

public class CellTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitialize_WithNullAndAllPossibilities()
    {
        // Act
        var cell = new Cell();

        // Assert
        Assert.Null(cell.Number);
        for (var i = 1; i <= 9; i++)
        {
            Assert.True(cell.Possibilities[i]);
        }
        Assert.False(cell.Possibilities[0]);
    }

    [Fact]
    public void CopyConstructor_ShouldCreate_IndependentCopy()
    {
        // Arrange
        var originalCell = new Cell
        {
            Number = 5
        };
        originalCell.ClearPossibilities();

        // Act
        var copiedCell = new Cell(originalCell);
        copiedCell.Number = 3;
        copiedCell.Possibilities[1] = true;

        // Assert
        Assert.Equal(5, originalCell.Number);
        Assert.Equal(3, copiedCell.Number);
        Assert.False(originalCell.Possibilities[1]);
        Assert.True(copiedCell.Possibilities[1]);
    }

    [Fact]
    public void InitializeAllPossibilities_ShouldSet_AllToTrue()
    {
        // Arrange
        var cell = new Cell();
        cell.ClearPossibilities();

        // Act
        cell.InitializeAllPossibilities();

        // Assert
        for (var i = 1; i <= 9; i++)
        {
            Assert.True(cell.Possibilities[i]);
        }
    }

    [Fact]
    public void ClearPossibilities_ShouldSet_AllToFalse()
    {
        // Arrange
        var cell = new Cell();

        // Act
        cell.ClearPossibilities();

        // Assert
        for (var i = 0; i <= 9; i++)
        {
            Assert.False(cell.Possibilities[i]);
        }
    }

    [Fact]
    public void Possibilities_ShouldAllow_IndividualModification()
    {
        // Arrange
        var cell = new Cell();

        // Act
        cell.Possibilities[5] = false;
        cell.Possibilities[3] = false;

        // Assert
        Assert.False(cell.Possibilities[5]);
        Assert.False(cell.Possibilities[3]);
        Assert.True(cell.Possibilities[1]);
        Assert.True(cell.Possibilities[2]);
        Assert.True(cell.Possibilities[4]);
    }
}

