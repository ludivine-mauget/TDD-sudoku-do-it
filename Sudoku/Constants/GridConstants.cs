namespace Sudoku.Constants;

/// <summary>
/// Constantes globales pour le Sudoku.
/// </summary>
public static class GridConstants
{
    /// <summary>
    /// Taille de la grille (9x9).
    /// </summary>
    public const int GridSize = 9;

    /// <summary>
    /// Taille d'une sous-grille (3x3).
    /// </summary>
    public const int SubGridSize = 3;

    /// <summary>
    /// Nombre total de cellules dans la grille.
    /// </summary>
    public const int TotalCells = GridSize * GridSize; // 81

    /// <summary>
    /// Valeur minimale d'une cellule.
    /// </summary>
    public const int MinValue = 1;

    /// <summary>
    /// Valeur maximale d'une cellule.
    /// </summary>
    public const int MaxValue = 9;
}

