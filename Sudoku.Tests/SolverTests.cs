using Sudoku.Models;
using Sudoku.Tests.Helpers;
using Sudoku.Tests.TestData;

namespace Sudoku.Tests;

public class SolverTests
{
    [Fact]
    public void Solve_ShouldComplete_GridCorrectly()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestBeginnerGridToSolve.InitialValues;
        var expectedSolution = SudokuGridTestData.TestBeginnerGridToSolve.ExpectedSolution;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        var expectedGrid = GridTestHelper.CreateGridWithValues(expectedSolution);
        
        // Act
        var solvedGrid = Solver.Solver.Solve(grid);

        // Assert
        Assert.True(solvedGrid.IsSameAs(expectedGrid));
    }
    
    [Fact] // 6 seconds
    public void Solve_ShouldReturn_InitialGrid_WhenNoSolutionExists()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestGridWithNoSolution.InitialValues;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        
        // Act
        var solvedGrid = Solver.Solver.Solve(grid);

        // Assert
        Assert.True(solvedGrid.IsSameAs(grid));
    }
    
    [Fact]
    public void Solve_ShouldComplete_HardGridCorrectly()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestHardGridToSolve.InitialValues;
        var expectedSolution = SudokuGridTestData.TestHardGridToSolve.ExpectedSolution;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        var expectedGrid = GridTestHelper.CreateGridWithValues(expectedSolution);

        // Act
        var solvedGrid = Solver.Solver.Solve(grid);

        // Assert
        Assert.True(solvedGrid.IsSameAs(expectedGrid));
    }
    
    [Fact]
    public void CountSolutions_ShouldReturn_OneSolutionForUniqueSolutionGrid()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestBeginnerGridToSolve.InitialValues;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        var solver = new Solver.Solver();

        // Act
        var solutionCount = solver.CountSolutions(grid);
        var hasUniqueSolution = solver.HasUniqueSolution(grid);

        // Assert
        Assert.Equal(1, solutionCount);
        Assert.True(hasUniqueSolution);
    }

    [Fact]
    public void CountSolutions_ShouldReturn_MultipleSolutionsForNonUniqueSolutionGrid()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestGridWithMultipleSolutions.InitialValues;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        var solver = new Solver.Solver();

        // Act
        var solutionCount = solver.CountSolutions(grid);
        var hasUniqueSolution = solver.HasUniqueSolution(grid);

        // Assert
        Assert.True(solutionCount > 1);
        Assert.False(hasUniqueSolution);
    }

    [Fact]
    public void CountSolutions_ShouldReturn_ZeroSolutionsForNoSolutionGrid()
    {
        // Arrange
        var initialValues = SudokuGridTestData.TestGridWithNoSolution.InitialValues;
        var grid = GridTestHelper.CreateGridWithValues(initialValues);
        var solver = new Solver.Solver();

        // Act
        var solutionCount = solver.CountSolutions(grid);
        var hasUniqueSolution = solver.HasUniqueSolution(grid);

        // Assert
        Assert.Equal(0, solutionCount);
        Assert.False(hasUniqueSolution);
    }
    
    [Fact]
    public void FindEmptyCell_ShouldReturn_FirstEmptyCell()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((0, 1), 2);
        
        // Act
        var emptyCell = Solver.Solver.FindEmptyCell(grid);
        
        // Assert
        Assert.NotNull(emptyCell);
        Assert.Equal((0, 2), emptyCell.Value);
    }
    
    [Fact]
    public void FindEmptyCell_ShouldReturn_NullForCompleteGrid()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        
        // Act
        var emptyCell = Solver.Solver.FindEmptyCell(fullGrid);
        
        // Assert
        Assert.Null(emptyCell);
    }
    
    [Fact]
    public void IsValidPlacement_ShouldReturn_TrueForValidPlacement()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        
        // Act
        var isValid = Solver.Solver.IsValidPlacement(grid, (0, 1), 2);
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsValidPlacement_ShouldReturn_FalseForInvalidRow()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        
        // Act
        var isValid = Solver.Solver.IsValidPlacement(grid, (0, 1), 1);
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValidPlacement_ShouldReturn_FalseForInvalidColumn()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        
        // Act
        var isValid = Solver.Solver.IsValidPlacement(grid, (1, 0), 1);
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValidPlacement_ShouldReturn_FalseForInvalidSubGrid()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        
        // Act
        var isValid = Solver.Solver.IsValidPlacement(grid, (1, 1), 1);
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void ResetCell_ShouldClear_CellValue()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 5);
        
        // Act
        Solver.Solver.ResetCell(grid, (0, 0));
        
        // Assert
        Assert.Null(grid.GetCellValue((0, 0)));
    }
    
    [Fact]
    public void Solve_ShouldReturn_SameGridWhenAlreadyComplete()
    {
        // Arrange
        var fullGrid = Generator.Generator.GenerateFullGrid();
        var fullGridCopy = new Grid(fullGrid);
        
        // Act
        var solvedGrid = Solver.Solver.Solve(fullGrid);
        
        // Assert
        Assert.True(solvedGrid.IsSameAs(fullGridCopy));
    }
}

