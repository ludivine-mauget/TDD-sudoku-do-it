using Sudoku.Constants;
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

        for (var num = GridConstants.MinValue; num <= GridConstants.MaxValue; num++)
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
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
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

    /// <summary>
    /// Compte le nombre de solutions d'une grille.
    /// </summary>
    /// <param name="grid">La grille à analyser</param>
    /// <param name="maxSolutions">Nombre maximum de solutions à chercher (optimisation early-exit)</param>
    /// <returns>Le nombre de solutions trouvées (plafonné à maxSolutions)</returns>
    public int CountSolutions(Grid grid, int maxSolutions = int.MaxValue)
    {
        var count = 0;
        var gridCopy = new Grid(grid);
        CountSolutionsRecursive(gridCopy, ref count, maxSolutions);
        return count;
    }

    private static void CountSolutionsRecursive(Grid gridCopy, ref int count, int maxSolutions)
    {
        // Early-exit : arrêter dès qu'on a atteint le maximum demandé
        if (count >= maxSolutions)
            return;

        var emptyCell = FindEmptyCell(gridCopy);

        if (emptyCell == null)
        {
            count++;
            return;
        }

        var (row, col) = emptyCell.Value;

        for (var num = 1; num <= GridConstants.GridSize; num++)
        {
            // Early-exit dans la boucle aussi
            if (count >= maxSolutions)
                return;

            if (!IsValidPlacement(gridCopy, (row, col), num)) continue;

            gridCopy.SetCellValue((row, col), num);
            CountSolutionsRecursive(gridCopy, ref count, maxSolutions);
            ResetCell(gridCopy, (row, col));
        }
    }

    /// <summary>
    /// Vérifie si la grille a exactement une solution unique.
    /// Optimisé avec early-exit dès qu'on trouve 2 solutions.
    /// </summary>
    public bool HasUniqueSolution(Grid grid)
    {
        return CountSolutions(grid, maxSolutions: 2) == 1;
    }
}