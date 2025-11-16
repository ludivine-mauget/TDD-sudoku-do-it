namespace Sudoku.Models;

public class Cell
{
    public int? Number { get; set; }
    public bool[] Possibilities { get; set; } = new bool[10];
    
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
        for (var i = 1; i <= 9; i++)
        {
            Possibilities[i] = true;
        }
    }
    
    public void ClearPossibilities()
    {
        Array.Clear(Possibilities, 0, 10);
    }
}