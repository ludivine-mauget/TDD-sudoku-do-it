using Sudoku.API.DTOs;
using Sudoku.Models;

namespace Sudoku.API.Extensions;

public static class GridExtensions
{
    public static GridDto ToGridDto(this Grid grid)
    {
        var gridDto = new GridDto();
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var cellValue = grid.GetCellValue((row, col));
                gridDto.Cells[row][col] = new CellDto
                {
                    Value = cellValue,
                    IsFixed = cellValue.HasValue
                };
            }
        }

        return gridDto;
    }

    public static Grid ToGrid(this GridDto gridDto)
    {
        var grid = new Grid();

        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                var cellDto = gridDto.Cells[row][col];
                if (cellDto.Value.HasValue)
                {
                    grid.SetCellValue((row, col), cellDto.Value.Value);
                }
            }
        }

        return grid;
    }
}

