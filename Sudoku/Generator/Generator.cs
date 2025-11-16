using Sudoku.Models;
using Sudoku.Solver;

namespace Sudoku.Generator;

public static class Generator
{
    public static Grid  GenerateFullGrid()
    {
        var grid = new Grid();
        FillGridRecursive(grid);
        return grid;
    }

    private static void FillGridRecursive(Grid grid)
    {
        var emptyCell = Solver.Solver.FindEmptyCell(grid);
        
        if (emptyCell == null)
        {
            return;
        }

        var (row, col) = emptyCell.Value;
        var numbers = Enumerable.Range(1, 9).OrderBy(_ => Guid.NewGuid()).ToList();

        foreach (var num in numbers.Where(num => Solver.Solver.IsValidPlacement(grid, (row, col), num)))
        {
            grid.SetCellValue((row, col), num);
            FillGridRecursive(grid);
            if (Solver.Solver.FindEmptyCell(grid) == null)
            {
                return;
            }
            Solver.Solver.ResetCell(grid, (row, col));
        }
    }

    public static Grid GeneratePuzzle(Grid fullGrid, int i)
    {
        var puzzleGrid = new Grid(fullGrid);
        var cellsToRemove = i;
        var rand = new Random();

        while (cellsToRemove > 0)
        {
            var row = rand.Next(0, 9);
            var col = rand.Next(0, 9);

            if (!puzzleGrid.GetCellValue((row, col)).HasValue) continue;
            puzzleGrid.RemoveCellValue((row, col));
            cellsToRemove--;
        }

        return puzzleGrid;
    }
    
    public static Grid GeneratePuzzleWithUniqueSolution(Grid fullGrid, int cellsToRemove)
    {
        var puzzleGrid = new Grid(fullGrid);
        var rand = new Random();

        while (cellsToRemove > 0)
        {
            var row = rand.Next(0, 9);
            var col = rand.Next(0, 9);

            if (!puzzleGrid.GetCellValue((row, col)).HasValue) continue;

            var backupValue = puzzleGrid.GetCellValue((row, col))!.Value;
            puzzleGrid.RemoveCellValue((row, col));

            var countSolutions = new Solver.Solver().CountSolutions(puzzleGrid);
            if (countSolutions != 1)
            {
                puzzleGrid.SetCellValue((row, col), backupValue);
            }
            else
            {
                cellsToRemove--;
            }
        }

        return puzzleGrid;
    }
}