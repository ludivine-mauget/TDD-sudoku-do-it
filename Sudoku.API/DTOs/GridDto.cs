namespace Sudoku.API.DTOs;

public class GridDto
{
    public CellDto[][] Cells { get; set; } = new CellDto[9][];
    public GridDto()
    {
        for (var i = 0; i < 9; i++)
        {
            Cells[i] = new CellDto[9];
            for (var j = 0; j < 9; j++)
            {
                Cells[i][j] = new CellDto();
            }
        }
    }
}

