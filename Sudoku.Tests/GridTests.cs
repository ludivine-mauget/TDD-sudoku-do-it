using Sudoku.Models;
using Sudoku.Tests.Helpers;
using Sudoku.Tests.TestData;

namespace Sudoku.Tests;

public class GridTests
{
    [Fact]
    public void EmptyGrid_ShouldReturn_AllPossibilities_NoValues()
    {
        // Arrange & Act
        var grid = new Grid();

        // Assert
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var cellValue = grid.GetCellValue((row, col));
                Assert.Null(cellValue);
                
                var possibilities = grid.GetCellPossibilities((row, col));
                for (var i = 1; i <= 9; i++)
                {
                    Assert.True(possibilities[i]);
                }
            }
        }
    }
    
    [Fact]
    public void SetCellValue_ShouldUpdate_CellValue_And_UpdatePossibilities()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        var valueToSet = 5;

        // Act
        grid.SetCellValue(position, valueToSet);

        // Assert
        var cellValue = grid.GetCellValue(position);
        Assert.Equal(valueToSet, cellValue);
        
        var cellPossibilities = grid.GetCellPossibilities(position);
        for (var i = 1; i <= 9; i++)
        {
            Assert.False(cellPossibilities[i]);
        }
        
        var subGridPossibilities = grid.GetSubGridPossibilities(position);
        Assert.False(subGridPossibilities[valueToSet]);
        
        var rowPossibilities = grid.GetRowPossibilities(position.row);
        Assert.False(rowPossibilities[valueToSet]);
        
        var colPossibilities = grid.GetColumnPossibilities(position.col);
        Assert.False(colPossibilities[valueToSet]);
    }
    
    [Fact]
    public void GridFromImage_ShouldCalculate_CorrectPossibilities()
    {
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestGridPossibilitiesCheck.InitialValues);
        
        foreach (var (position, expected) in SudokuGridTestData.TestGridPossibilitiesCheck.ExpectedPossibilities)
        {
            var actual = GridTestHelper.GetCellActualPossibilities(grid, position);
            Assert.Equal(expected, actual);
        }
    }
    
    [Fact]
    public void CopyConstructor_ShouldCreate_IdenticalGrid()
    {
        // Arrange
        var originalGrid = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (1, 1), 2 },
            { (2, 2), 3 }
        });
        
        // Act
        var copiedGrid = new Grid(originalGrid);
        copiedGrid.SetCellValue((0, 1), 4); 
        
        // Assert
        Assert.Equal(originalGrid.GetCellValue((0, 0)), copiedGrid.GetCellValue((0, 0)));
        Assert.Equal(originalGrid.GetCellPossibilities((0,0)), copiedGrid.GetCellPossibilities((0,0)));
        
        Assert.Equal(originalGrid.GetCellValue((1, 1)), copiedGrid.GetCellValue((1, 1)));
        Assert.Equal(originalGrid.GetCellPossibilities((1,1)), copiedGrid.GetCellPossibilities((1,1)));
        
        Assert.Equal(originalGrid.GetCellValue((2, 2)), copiedGrid.GetCellValue((2, 2)));
        Assert.Equal(originalGrid.GetCellPossibilities((2,2)), copiedGrid.GetCellPossibilities((2,2)));
        
        Assert.NotEqual(originalGrid.GetCellValue((0, 1)), copiedGrid.GetCellValue((0, 1))); 
        var originalPoss = originalGrid.GetCellPossibilities((0,1));
        var copiedPoss = copiedGrid.GetCellPossibilities((0,1));
        Assert.NotEqual(originalPoss, copiedPoss);
    }
    
    [Fact]
    public void SetCellValue_InvalidValue_ShouldThrowException()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        var invalidValue = 10; 
        
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => grid.SetCellValue(position, invalidValue));
    }

    [Fact]
    public void SetCellValue_AlreadySetCell_ShouldThrowException()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        grid.SetCellValue(position, 5);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => grid.SetCellValue(position, 3));
    }
    
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(9, 0)]
    [InlineData(0, 9)]
    [InlineData(-1, -1)]
    [InlineData(10, 10)]
    public void GetCellValue_InvalidPosition_ShouldThrowException(int row, int col)
    {
        // Arrange
        var grid = new Grid();
        
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.GetCellValue((row, col)));
    }
    
    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, -1)]
    [InlineData(9, 0)]
    [InlineData(0, 9)]
    public void SetCellValue_InvalidPosition_ShouldThrowException(int row, int col)
    {
        // Arrange
        var grid = new Grid();
        
        // Act & Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.SetCellValue((row, col), 5));
    }
    
    [Fact]
    public void RemoveCellValue_ShouldClear_CellAndResetPossibilities()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        grid.SetCellValue(position, 5);
        
        // Act
        grid.RemoveCellValue(position);
        
        // Assert
        Assert.Null(grid.GetCellValue(position));
        var possibilities = grid.GetCellPossibilities(position);
        for (var i = 1; i <= 9; i++)
        {
            Assert.True(possibilities[i]);
        }
    }
    
    [Fact]
    public void RemoveCellValue_EmptyCell_ShouldThrowException()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        
        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => grid.RemoveCellValue(position));
    }
    
    [Fact]
    public void ResetCell_ShouldClear_CellAndResetPossibilities()
    {
        // Arrange
        var grid = new Grid();
        var position = (row: 0, col: 0);
        grid.SetCellValue(position, 7);
        
        // Act
        grid.ResetCell(position);
        
        // Assert
        Assert.Null(grid.GetCellValue(position));
        var possibilities = grid.GetCellPossibilities(position);
        for (var i = 1; i <= 9; i++)
        {
            Assert.True(possibilities[i]);
        }
    }
    
    [Fact]
    public void GetSubGridPossibilities_ShouldReturn_CorrectPossibilities()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((1, 1), 5);
        grid.SetCellValue((2, 2), 9);
        
        // Act
        var possibilities = grid.GetSubGridPossibilities((0, 0));
        
        // Assert
        Assert.False(possibilities[1]);
        Assert.False(possibilities[5]);
        Assert.False(possibilities[9]);
        Assert.True(possibilities[2]);
        Assert.True(possibilities[3]);
        Assert.True(possibilities[4]);
        Assert.True(possibilities[6]);
        Assert.True(possibilities[7]);
        Assert.True(possibilities[8]);
    }
    
    [Fact]
    public void GetRowPossibilities_ShouldReturn_CorrectPossibilities()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((0, 4), 5);
        grid.SetCellValue((0, 8), 9);
        
        // Act
        var possibilities = grid.GetRowPossibilities(0);
        
        // Assert
        Assert.False(possibilities[1]);
        Assert.False(possibilities[5]);
        Assert.False(possibilities[9]);
        Assert.True(possibilities[2]);
        Assert.True(possibilities[3]);
        Assert.True(possibilities[4]);
        Assert.True(possibilities[6]);
        Assert.True(possibilities[7]);
        Assert.True(possibilities[8]);
    }
    
    [Fact]
    public void GetColumnPossibilities_ShouldReturn_CorrectPossibilities()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 1);
        grid.SetCellValue((4, 0), 5);
        grid.SetCellValue((8, 0), 9);
        
        // Act
        var possibilities = grid.GetColumnPossibilities(0);
        
        // Assert
        Assert.False(possibilities[1]);
        Assert.False(possibilities[5]);
        Assert.False(possibilities[9]);
        Assert.True(possibilities[2]);
        Assert.True(possibilities[3]);
        Assert.True(possibilities[4]);
        Assert.True(possibilities[6]);
        Assert.True(possibilities[7]);
        Assert.True(possibilities[8]);
    }
    
    [Fact]
    public void IsSameAs_ShouldReturn_TrueForIdenticalGrids()
    {
        // Arrange
        var grid1 = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (1, 1), 5 },
            { (2, 2), 9 }
        });
        var grid2 = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (1, 1), 5 },
            { (2, 2), 9 }
        });
        
        // Act
        var result = grid1.IsSameAs(grid2);
        
        // Assert
        Assert.True(result);
    }
    
    [Fact]
    public void IsSameAs_ShouldReturn_FalseForDifferentGrids()
    {
        // Arrange
        var grid1 = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (1, 1), 5 }
        });
        var grid2 = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (1, 1), 6 }
        });
        
        // Act
        var result = grid1.IsSameAs(grid2);
        
        // Assert
        Assert.False(result);
    }
    
    [Fact]
    public void IsValid_EmptyGrid_ShouldReturn_True()
    {
        // Arrange
        var grid = new Grid();
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsValid_ValidPartialGrid_ShouldReturn_True()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(new Dictionary<(int row, int col), int>
        {
            { (0, 0), 1 },
            { (0, 1), 2 },
            { (1, 0), 3 },
            { (3, 3), 5 }
        });
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsValid_DuplicateInRow_ShouldReturn_False()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 5);
        grid.ResetCell((0, 8));
        grid.SetCellValue((0, 8), 5); 
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValid_DuplicateInColumn_ShouldReturn_False()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 5);
        grid.ResetCell((8, 0));
        grid.SetCellValue((8, 0), 5); 
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValid_DuplicateInSubGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = new Grid();
        grid.SetCellValue((0, 0), 5);
        grid.ResetCell((1, 1));
        grid.SetCellValue((1, 1), 5); 
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.False(isValid);
    }
    
    [Fact]
    public void IsValid_CompleteValidGrid_ShouldReturn_True()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestBeginnerGridToSolve.ExpectedSolution);
        
        // Act
        var isValid = grid.IsValid();
        
        // Assert
        Assert.True(isValid);
    }
    
    [Fact]
    public void IsComplete_EmptyGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = new Grid();
        
        // Act
        var isComplete = grid.IsComplete();
        
        // Assert
        Assert.False(isComplete);
    }
    
    [Fact]
    public void IsComplete_PartialGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestBeginnerGridToSolve.InitialValues);
        
        // Act
        var isComplete = grid.IsComplete();
        
        // Assert
        Assert.False(isComplete);
    }
    
    [Fact]
    public void IsComplete_FullGrid_ShouldReturn_True()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestBeginnerGridToSolve.ExpectedSolution);
        
        // Act
        var isComplete = grid.IsComplete();
        
        // Assert
        Assert.True(isComplete);
    }
    
    [Fact]
    public void IsSolved_EmptyGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = new Grid();
        
        // Act
        var isSolved = grid.IsSolved();
        
        // Assert
        Assert.False(isSolved);
    }
    
    [Fact]
    public void IsSolved_PartialValidGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestBeginnerGridToSolve.InitialValues);
        
        // Act
        var isSolved = grid.IsSolved();
        
        // Assert
        Assert.False(isSolved);
    }
    
    [Fact]
    public void IsSolved_CompleteValidGrid_ShouldReturn_True()
    {
        // Arrange
        var grid = GridTestHelper.CreateGridWithValues(SudokuGridTestData.TestBeginnerGridToSolve.ExpectedSolution);
        
        // Act
        var isSolved = grid.IsSolved();
        
        // Assert
        Assert.True(isSolved);
    }
    
    [Fact]
    public void IsSolved_CompleteInvalidGrid_ShouldReturn_False()
    {
        // Arrange
        var grid = Generator.Generator.GenerateFullGrid();
        
        var val1 = grid.GetCellValue((0, 0));
        var val2 = grid.GetCellValue((0, 1));
        grid.ResetCell((0, 0));
        grid.ResetCell((0, 1));
        grid.SetCellValue((0, 0), val2!.Value);
        grid.SetCellValue((0, 1), val1!.Value);
        
        grid.ResetCell((0, 1));
        grid.SetCellValue((0, 1), val2.Value); 
        
        // Act
        var isSolved = grid.IsSolved();
        
        // Assert
        Assert.False(isSolved);
    }
}