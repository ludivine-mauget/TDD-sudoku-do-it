using Sudoku.Models;

namespace Sudoku.Solver;

public class Solver
{
    public static Grid Solve(Grid grid)
    {
        var solvedGrid = new Grid(grid);
        return SolveRecursive(solvedGrid) ? solvedGrid : grid;
    }

    private static bool SolveRecursive(Grid grid)
    {
        var emptyCell = FindEmptyCell(grid);
        
        if (emptyCell == null)
        {
            return true;
        }

        var (row, col) = emptyCell.Value;

        for (var num = 1; num <= 9; num++)
        {
            if (!IsValidPlacement(grid, (row, col), num)) continue;
            
            grid.SetCellValue((row, col), num);
            if (SolveRecursive(grid))
            {
                return true;
            }
            ResetCell(grid, (row, col));
        }
        return false;
    }

    public static (int row, int col)? FindEmptyCell(Grid grid)
    {
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                if (!grid.GetCellValue((row, col)).HasValue)
                {
                    return (row, col);
                }
            }
        }
        return null;
    }

    public static bool IsValidPlacement(Grid grid, (int row, int col) position, int num)
    {
        var rowPossibilities = grid.GetRowPossibilities(position.row);
        if (!rowPossibilities[num])
        {
            return false;
        }

        var colPossibilities = grid.GetColumnPossibilities(position.col);
        if (!colPossibilities[num])
        {
            return false;
        }

        var subGridPossibilities = grid.GetSubGridPossibilities(position);
        if (!subGridPossibilities[num])
        {
            return false;
        }

        return true;
    }

    public static void ResetCell(Grid grid, (int row, int col) position)
    {
        grid.ResetCell(position);
    }

    public int CountSolutions(Grid grid)
    {
        var count = 0;
        var gridCopy = new Grid(grid);
        CountSolutionsRecursive(gridCopy, ref count);
        return count;
    }

    private static void CountSolutionsRecursive(Grid gridCopy, ref int count)
    {
        var emptyCell = FindEmptyCell(gridCopy);
        
        if (emptyCell == null)
        {
            count++;
            return;
        }

        var (row, col) = emptyCell.Value;

        for (var num = 1; num <= 9; num++)
        {
            if (!IsValidPlacement(gridCopy, (row, col), num)) continue;
            
            gridCopy.SetCellValue((row, col), num);
            CountSolutionsRecursive(gridCopy, ref count);
            ResetCell(gridCopy, (row, col));
        }
    }

    public bool HasUniqueSolution(Grid grid)
    {
        return CountSolutions(grid) == 1;
    }
}