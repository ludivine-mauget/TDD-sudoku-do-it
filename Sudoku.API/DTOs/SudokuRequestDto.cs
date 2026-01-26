using System.ComponentModel.DataAnnotations;

namespace Sudoku.API.DTOs;

public enum DifficultyLevel
{
    Easy,
    Medium,
    Hard,
    Expert
}

public class SudokuRequestDto
{
    /// <summary>
    /// Niveau de difficulté du puzzle
    /// </summary>
    [EnumDataType(typeof(DifficultyLevel))]
    public DifficultyLevel Difficulty { get; set; } = DifficultyLevel.Medium;
    
    /// <summary>
    /// Nombre de cellules à retirer (optionnel, surcharge la difficulté)
    /// </summary>
    [Range(1, 64, ErrorMessage = "Le nombre de cellules à retirer doit être entre 1 et 64")]
    public int? CellsToRemove { get; set; }
}

