using Sudoku.API.DTOs;
using Sudoku.Constants;
using Sudoku.Models;

namespace Sudoku.API.Extensions;

public static class GridExtensions
{
    public static GridDto ToGridDto(this Grid grid)
    {
        var gridDto = GridDto.CreateEmpty();
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
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

        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
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

