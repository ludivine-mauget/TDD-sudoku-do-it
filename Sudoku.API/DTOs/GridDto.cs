using System.ComponentModel.DataAnnotations;

namespace Sudoku.API.DTOs;

public class GridDto : IValidatableObject
{
    /// <summary>
    /// Constructeur par défaut requis pour la désérialisation JSON.
    /// Préférez utiliser <see cref="CreateEmpty"/> pour créer une grille initialisée.
    /// </summary>
    public GridDto() { }

    /// <summary>
    /// Matrice 9x9 des cellules du Sudoku
    /// </summary>
    [Required(ErrorMessage = "La grille est requise")]
    public CellDto[][] Cells { get; set; } = new CellDto[9][];

    /// <summary>
    /// Crée une grille vide avec toutes les cellules initialisées.
    /// </summary>
    public static GridDto CreateEmpty()
    {
        var grid = new GridDto();
        for (var i = 0; i < 9; i++)
        {
            grid.Cells[i] = new CellDto[9];
            for (var j = 0; j < 9; j++)
            {
                grid.Cells[i][j] = new CellDto();
            }
        }
        return grid;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Cells.Length != 9)
        {
            yield return new ValidationResult("La grille doit avoir exactement 9 lignes", new[] { nameof(Cells) });
            yield break;
        }

        for (var i = 0; i < 9; i++)
        {
            if (Cells[i] == null || Cells[i].Length != 9)
            {
                yield return new ValidationResult($"La ligne {i} doit avoir exactement 9 colonnes", new[] { nameof(Cells) });
            }
        }
    }
}

