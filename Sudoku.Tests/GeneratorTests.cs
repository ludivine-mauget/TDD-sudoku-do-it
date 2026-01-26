using Sudoku.Generator;
using Sudoku.Tests.Helpers;

namespace Sudoku.Tests;

public class GeneratorTests
{
    [Fact]
    public void GenerateFullGrid_ShouldCreate_CompletedGrid()
    {
        // Act
        var fullGrid = Generator.Generator.GenerateFullGrid();

        // Assert
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var cellValue = fullGrid.GetCellValue((row, col));
                Assert.True(cellValue.HasValue);
            }
        }
    }

    [Fact]
    public void GenerateFullGrid_ShouldReturn_DifferentGrids_OnMultipleCalls()
    {
        // Act
        var fullGrid1 = Generator.Generator.GenerateFullGrid();
        var fullGrid2 = Generator.Generator.GenerateFullGrid();

        // Assert
        Assert.False(fullGrid1.IsSameAs(fullGrid2));
    }

    [Fact]
    public void GeneratePuzzle_ShouldCreate_SolvableGrid()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzle(fullGrid, 40);
        var solvedGrid = Solver.Solver.Solve(puzzleGrid);

        // Assert
        Assert.False(solvedGrid.IsSameAs(puzzleGrid));
    }

    [Fact]
    public void GeneratePuzzle_ShouldCreate_GridWithCorrectNumberOfEmptyCells()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        var cellsToRemove = 50;

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzle(fullGrid, cellsToRemove);

        // Assert
        var emptyCellCount = 0;
        for (var row = 0; row < 9; row++)
            for (var col = 0; col < 9; col++)
                if (!puzzleGrid.GetCellValue((row, col)).HasValue)
                    emptyCellCount++;
        Assert.Equal(cellsToRemove, emptyCellCount);
    }

    [Fact]
    public void GeneratePuzzleWithUniqueSolution_ShouldCreate_GridWithUniqueSolution()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        var cellsToRemove = 45;

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
        var solver = new Solver.Solver();
        var solutionCount = solver.CountSolutions(puzzleGrid);

        // Assert
        Assert.Equal(1, solutionCount);
    }

    [Fact]
    public void GeneratePuzzle_WithZeroCellsToRemove_ShouldReturn_FullGrid()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzle(fullGrid, 0);

        // Assert
        Assert.True(puzzleGrid.IsSameAs(fullGrid));
    }

    [Fact]
    public void GenerateAndSolve_ShouldReturn_OriginalFullGrid()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, 40);
        var solvedGrid = Solver.Solver.Solve(puzzleGrid);

        // Assert
        Assert.True(solvedGrid.IsSameAs(fullGrid));
    }

    [Fact]
    public void GenerateFullGrid_ShouldCreate_ValidSudokuGrid()
    {
        // Act
        var fullGrid = Generator.Generator.GenerateFullGrid();

        for (var row = 0; row < 9; row++)
        {
            var numbers = new HashSet<int>();
            for (var col = 0; col < 9; col++)
            {
                var value = fullGrid.GetCellValue((row, col));
                Assert.True(value.HasValue);
                numbers.Add(value.Value);
            }
            Assert.Equal(9, numbers.Count);
        }

        for (var col = 0; col < 9; col++)
        {
            var numbers = new HashSet<int>();
            for (var row = 0; row < 9; row++)
            {
                var value = fullGrid.GetCellValue((row, col));
                Assert.True(value.HasValue);
                numbers.Add(value.Value);
            }
            Assert.Equal(9, numbers.Count);
        }
    }

    [Fact]
    public void GeneratePuzzleWithUniqueSolution_ShouldBe_Solvable()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        var cellsToRemove = 50;

        // Act
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
        var solvedGrid = Solver.Solver.Solve(puzzleGrid);

        // Assert
        Assert.True(solvedGrid.IsSameAs(fullGrid));
    }
}

