using Sudoku.Models;

namespace Sudoku.Tests.Helpers;

/// <summary>
/// Helper methods for creating and testing Sudoku grids.
/// </summary>
public static class GridTestHelper
{
    /// <summary>
    /// Creates a grid and initializes it with the provided values.
    /// </summary>
    /// <param name="initialValues">Dictionary of (row, col) positions and their values.</param>
    /// <returns>An initialized Grid instance.</returns>
    public static Grid CreateGridWithValues(Dictionary<(int row, int col), int> initialValues)
    {
        var grid = new Grid();

        foreach (var (position, value) in initialValues)
        {
            grid.SetCellValue(position, value);
        }

        return grid;
    }

    /// <summary>
    /// Calculates the actual possibilities for a cell by intersecting
    /// subgrid, row, and column possibilities.
    /// </summary>
    /// <param name="grid">The grid to analyze.</param>
    /// <param name="position">The position of the cell.</param>
    /// <returns>A HashSet of possible values for the cell.</returns>
    public static HashSet<int> GetCellActualPossibilities(Grid grid, (int row, int col) position)
    {
        var subGridPossibilities = grid.GetSubGridPossibilities(position);
        var rowPossibilities = grid.GetRowPossibilities(position.row);
        var colPossibilities = grid.GetColumnPossibilities(position.col);

        // Intersection of all three bool arrays
        var actualPossibilities = new HashSet<int>();
        for (int i = 1; i <= 9; i++)
        {
            if (subGridPossibilities[i] && rowPossibilities[i] && colPossibilities[i])
            {
                actualPossibilities.Add(i);
            }
        }

        return actualPossibilities;
    }
}

