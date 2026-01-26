using Microsoft.AspNetCore.Mvc;
using Sudoku.API.DTOs;
using Sudoku.API.Services;

namespace Sudoku.API.Controllers;

[ApiController]
[Route("api/sudoku")]
[Produces("application/json")]
public class SudokuController(ISudokuService sudokuService, ILogger<SudokuController> logger)
    : ControllerBase
{
    /// <summary>
    /// Génère un nouveau puzzle Sudoku
    /// </summary>
    /// <param name="request">Paramètres de génération du puzzle</param>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(SudokuResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SudokuResponseDto> GeneratePuzzle([FromBody] SudokuRequestDto request)
    {
        try
        {
            logger.LogInformation("Requête de génération de puzzle reçue");
            var result = sudokuService.GenerateSudoku(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la génération du puzzle");
            return StatusCode(500, new { message = "Erreur lors de la génération du puzzle", error = ex.Message });
        }
    }

    /// <summary>
    /// Résout un puzzle Sudoku
    /// </summary>
    /// <param name="gridDto">Grille à résoudre</param>
    [HttpPost("solve")]
    [ProducesResponseType(typeof(SolveResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<SolveResponseDto> SolvePuzzle([FromBody] GridDto gridDto)
    {
        try
        {
            logger.LogInformation("Requête de résolution de puzzle reçue");
            var result = sudokuService.SolveSudoku(gridDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la résolution du puzzle");
            return StatusCode(500, new { message = "Erreur lors de la résolution du puzzle", error = ex.Message });
        }
    }

    /// <summary>
    /// Valide une solution de Sudoku
    /// </summary>
    /// <param name="gridDto">Grille à valider</param>
    [HttpPost("validate")]
    [ProducesResponseType(typeof(ValidationResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ValidationResultDto> ValidateSolution([FromBody] GridDto gridDto)
    {
        try
        {
            logger.LogInformation("Requête de validation de solution reçue");
            var result = sudokuService.ValidateSolution(gridDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erreur lors de la validation de la solution");
            return StatusCode(500, new { message = "Erreur lors de la validation", error = ex.Message });
        }
    }

    /// <summary>
    /// Point d'entrée de test pour vérifier que l'API fonctionne
    /// </summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<object> HealthCheck()
    {
        return Ok(new 
        { 
            status = "healthy", 
            message = "L'API Sudoku fonctionne correctement",
            timestamp = DateTime.UtcNow
        });
    }
}

