using System.ComponentModel.DataAnnotations;

namespace Sudoku.API.DTOs;

public class CellDto
{
    /// <summary>
    /// Valeur de la cellule (1-9, ou null si vide)
    /// </summary>
    [Range(1, 9, ErrorMessage = "La valeur doit être entre 1 et 9")]
    public int? Value { get; init; }
    
    /// <summary>
    /// Indique si la cellule fait partie du puzzle initial (non modifiable)
    /// </summary>
    public bool IsFixed { get; set; }
}

