using Sudoku.Constants;

namespace Sudoku.Models;

public class Grid
{

    private Cell[,] Cells { get; set; } = new Cell[GridConstants.GridSize, GridConstants.GridSize];

    public Grid()
    {
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                Cells[row, col] = new Cell();
            }
        }
    }

    public Grid(Grid originalGrid)
    {
        Cells = new Cell[GridConstants.GridSize, GridConstants.GridSize];
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                Cells[row, col] = new Cell(originalGrid.Cells[row, col]);
            }
        }
    }

    public int? GetCellValue((int row, int col) position)
    {
        return Cells[position.row, position.col].Number;
    }

    public bool[] GetCellPossibilities((int row, int col) position)
    {
        return Cells[position.row, position.col].Possibilities;
    }

    public bool[] GetSubGridPossibilities((int row, int col) position)
    {
        var subGridCells = GetSubgridCells(position);
        var possibilities = new bool[GridConstants.MaxValue + 1];

        for (var i = GridConstants.MinValue; i <= GridConstants.MaxValue; i++)
        {
            possibilities[i] = true;
        }

        for (var row = 0; row < GridConstants.SubGridSize; row++)
        {
            for (var col = 0; col < GridConstants.SubGridSize; col++)
            {
                if (subGridCells[row, col].Number.HasValue)
                {
                    possibilities[subGridCells[row, col].Number!.Value] = false;
                }
            }
        }

        return possibilities;
    }

    public bool[] GetRowPossibilities(int positionRow)
    {
        var possibilities = new bool[GridConstants.MaxValue + 1];

        for (var i = GridConstants.MinValue; i <= GridConstants.MaxValue; i++)
        {
            possibilities[i] = true;
        }

        for (var col = 0; col < GridConstants.GridSize; col++)
        {
            if (Cells[positionRow, col].Number.HasValue)
            {
                possibilities[Cells[positionRow, col].Number!.Value] = false;
            }
        }

        return possibilities;
    }

    public bool[] GetColumnPossibilities(int positionCol)
    {
        var possibilities = new bool[GridConstants.MaxValue + 1];

        for (var i = GridConstants.MinValue; i <= GridConstants.MaxValue; i++)
        {
            possibilities[i] = true;
        }

        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            if (Cells[row, positionCol].Number.HasValue)
            {
                possibilities[Cells[row, positionCol].Number!.Value] = false;
            }
        }

        return possibilities;
    }

    public void SetCellValue((int row, int col) position, int valueToSet)
    {
        if (Cells[position.row, position.col].Number.HasValue)
        {
            throw new InvalidOperationException("Cell already has a value.");
        }

        if (valueToSet < GridConstants.MinValue || valueToSet > GridConstants.MaxValue)
        {
            throw new ArgumentOutOfRangeException(nameof(valueToSet), $"Value must be between {GridConstants.MinValue} and {GridConstants.MaxValue}.");
        }

        Cells[position.row, position.col].Number = valueToSet;
        Cells[position.row, position.col].ClearPossibilities();
    }

    public void RemoveCellValue((int row, int col) position)
    {
        if (!Cells[position.row, position.col].Number.HasValue)
        {
            throw new InvalidOperationException("Cell is already empty.");
        }

        Cells[position.row, position.col].Number = null;
        Cells[position.row, position.col].InitializeAllPossibilities();
    }

    public void ResetCell((int row, int col) position)
    {
        Cells[position.row, position.col].Number = null;
        Cells[position.row, position.col].InitializeAllPossibilities();
    }

    public bool IsSameAs(Grid grid)
    {
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                if (Cells[row, col].Number != grid.Cells[row, col].Number)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Vérifie si la grille respecte les règles du Sudoku (pas de doublons dans les lignes, colonnes et sous-grilles).
    /// </summary>
    /// <returns>True si la grille est valide, False sinon.</returns>
    public bool IsValid()
    {
        // Vérifier toutes les lignes
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            var usedNumbers = new HashSet<int>();
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                var value = Cells[row, col].Number;
                if (value.HasValue)
                {
                    if (usedNumbers.Contains(value.Value))
                    {
                        return false; // Doublon trouvé dans la ligne
                    }
                    usedNumbers.Add(value.Value);
                }
            }
        }

        // Vérifier toutes les colonnes
        for (var col = 0; col < GridConstants.GridSize; col++)
        {
            var usedNumbers = new HashSet<int>();
            for (var row = 0; row < GridConstants.GridSize; row++)
            {
                var value = Cells[row, col].Number;
                if (value.HasValue)
                {
                    if (usedNumbers.Contains(value.Value))
                    {
                        return false; // Doublon trouvé dans la colonne
                    }
                    usedNumbers.Add(value.Value);
                }
            }
        }

        // Vérifier toutes les sous-grilles 3x3
        for (var boxRow = 0; boxRow < GridConstants.SubGridSize; boxRow++)
        {
            for (var boxCol = 0; boxCol < GridConstants.SubGridSize; boxCol++)
            {
                var usedNumbers = new HashSet<int>();
                for (var row = 0; row < GridConstants.SubGridSize; row++)
                {
                    for (var col = 0; col < GridConstants.SubGridSize; col++)
                    {
                        var actualRow = boxRow * GridConstants.SubGridSize + row;
                        var actualCol = boxCol * GridConstants.SubGridSize + col;
                        var value = Cells[actualRow, actualCol].Number;
                        if (value.HasValue)
                        {
                            if (usedNumbers.Contains(value.Value))
                            {
                                return false; // Doublon trouvé dans la sous-grille
                            }
                            usedNumbers.Add(value.Value);
                        }
                    }
                }
            }
        }

        return true;
    }

    /// <summary>
    /// Vérifie si toutes les cellules de la grille sont remplies.
    /// </summary>
    /// <returns>True si la grille est complète, False sinon.</returns>
    public bool IsComplete()
    {
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                if (!Cells[row, col].Number.HasValue)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Vérifie si la grille est résolue (complète ET valide).
    /// </summary>
    /// <returns>True si la grille est résolue, False sinon.</returns>
    public bool IsSolved()
    {
        return IsComplete() && IsValid();
    }

    private Cell[,] GetSubgridCells((int row, int col) position)
    {
        var subGridCells = new Cell[GridConstants.SubGridSize, GridConstants.SubGridSize];
        position = (position.row / GridConstants.SubGridSize, position.col / GridConstants.SubGridSize);
        for (var row = 0; row < GridConstants.SubGridSize; row++)
        {
            for (var col = 0; col < GridConstants.SubGridSize; col++)
            {
                subGridCells[row, col] = Cells[position.row * GridConstants.SubGridSize + row, position.col * GridConstants.SubGridSize + col];
            }
        }
        return subGridCells;
    }

    // Méthodes publiques pour l'accès depuis le client Blazor
    public void SetCellNumber(int row, int col, int value)
    {
        if (row < 0 || row >= GridConstants.GridSize)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between 0 and {GridConstants.GridSize - 1}.");

        if (col < 0 || col >= GridConstants.GridSize)
            throw new ArgumentOutOfRangeException(nameof(col), $"Column must be between 0 and {GridConstants.GridSize - 1}.");

        if (value < 0 || value > GridConstants.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(value), $"Value must be between 0 and {GridConstants.MaxValue}.");

        Cells[row, col].Number = value == 0 ? null : value;
    }

    public int GetCellNumber(int row, int col)
    {
        if (row < 0 || row >= GridConstants.GridSize)
            throw new ArgumentOutOfRangeException(nameof(row), $"Row must be between 0 and {GridConstants.GridSize - 1}.");

        if (col < 0 || col >= GridConstants.GridSize)
            throw new ArgumentOutOfRangeException(nameof(col), $"Column must be between 0 and {GridConstants.GridSize - 1}.");

        return Cells[row, col].Number ?? 0;
    }
}