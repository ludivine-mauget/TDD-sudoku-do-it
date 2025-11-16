namespace Sudoku.Models;

public class Grid
{

    private Cell[,] Cells { get; set; } = new Cell[9, 9];
    
    public Grid()
    {
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                Cells[row, col] = new Cell();
            }
        }
    }

    public Grid(Grid originalGrid)
    {
        Cells = new Cell[9, 9];
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
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
        var possibilities = new bool[10];
        
        for (var i = 1; i <= 9; i++)
        {
            possibilities[i] = true;
        }
        
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
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
        var possibilities = new bool[10];
        
        for (var i = 1; i <= 9; i++)
        {
            possibilities[i] = true;
        }
        
        for (var col = 0; col < 9; col++)
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
        var possibilities = new bool[10];
        
        for (var i = 1; i <= 9; i++)
        {
            possibilities[i] = true;
        }
        
        for (var row = 0; row < 9; row++)
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
        
        if (valueToSet < 1 || valueToSet > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(valueToSet), "Value must be between 1 and 9.");
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
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
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
        for (var row = 0; row < 9; row++)
        {
            var usedNumbers = new HashSet<int>();
            for (var col = 0; col < 9; col++)
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
        for (var col = 0; col < 9; col++)
        {
            var usedNumbers = new HashSet<int>();
            for (var row = 0; row < 9; row++)
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
        for (var boxRow = 0; boxRow < 3; boxRow++)
        {
            for (var boxCol = 0; boxCol < 3; boxCol++)
            {
                var usedNumbers = new HashSet<int>();
                for (var row = 0; row < 3; row++)
                {
                    for (var col = 0; col < 3; col++)
                    {
                        var actualRow = boxRow * 3 + row;
                        var actualCol = boxCol * 3 + col;
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
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
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
        var subGridCells = new Cell[3, 3];
        position = (position.row / 3, position.col / 3);
        for (var row = 0; row < 3; row++)
        {
            for (var col = 0; col < 3; col++)
            {
                subGridCells[row, col] = Cells[position.row * 3 + row, position.col * 3 + col];
            }
        }
        return subGridCells;
    }
}