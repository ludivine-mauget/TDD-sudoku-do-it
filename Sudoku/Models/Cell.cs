using Sudoku.Constants;

namespace Sudoku.Models;

public class Cell
{
    public int? Number { get; set; }
    public bool[] Possibilities { get; set; } = new bool[GridConstants.MaxValue + 1];

    public Cell()
    {
        Number = null;
        InitializeAllPossibilities();
    }

    public Cell(Cell cell)
    {
        Number = cell.Number;
        Possibilities = (bool[])cell.Possibilities.Clone();
    }

    public void InitializeAllPossibilities()
    {
        for (var i = GridConstants.MinValue; i <= GridConstants.MaxValue; i++)
        {
            Possibilities[i] = true;
        }
    }

    public void ClearPossibilities()
    {
        Array.Clear(Possibilities, 0, GridConstants.MaxValue + 1);
    }
}