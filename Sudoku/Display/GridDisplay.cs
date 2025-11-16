using Sudoku.Models;

namespace Sudoku.Display;

public static class GridDisplay
{
    public static void DisplayGrid(Grid grid, string title = "")
    {
        if (!string.IsNullOrEmpty(title))
        {
            Console.WriteLine($"\n{title}");
            Console.WriteLine(new string('=', title.Length));
        }
        
        Console.WriteLine("\n┌───────┬───────┬───────┐");
        
        for (var row = 0; row < 9; row++)
        {
            Console.Write("│");
            
            for (var col = 0; col < 9; col++)
            {
                var value = grid.GetCellValue((row, col));

                Console.Write(value.HasValue ? $" {value.Value}" : " .");

                if ((col + 1) % 3 == 0)
                {
                    Console.Write(" │");
                }
            }
            
            Console.WriteLine();
            
            if ((row + 1) % 3 == 0 && row < 8)
            {
                Console.WriteLine("├───────┼───────┼───────┤");
            }
        }
        
        Console.WriteLine("└───────┴───────┴───────┘");
    }
    
    public static void DisplayGridComparison(Grid puzzle, Grid solution)
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("                    PUZZLE vs SOLUTION");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine("\n    PUZZLE                          SOLUTION");
        Console.WriteLine("┌───────┬───────┬───────┐      ┌───────┬───────┬───────┐");
        
        for (var row = 0; row < 9; row++)
        {
            Console.Write("│");
            
            for (var col = 0; col < 9; col++)
            {
                var value = puzzle.GetCellValue((row, col));
                
                if (value.HasValue)
                {
                    Console.Write($" {value.Value}");
                }
                else
                {
                    Console.Write(" .");
                }
                
                if ((col + 1) % 3 == 0)
                {
                    Console.Write(" │");
                }
            }
            
            Console.Write("      │");
            
            for (var col = 0; col < 9; col++)
            {
                var value = solution.GetCellValue((row, col));
                
                if (value.HasValue)
                {
                    Console.Write($" {value.Value}");
                }
                else
                {
                    Console.Write(" .");
                }
                
                if ((col + 1) % 3 == 0)
                {
                    Console.Write(" │");
                }
            }
            
            Console.WriteLine();
            
            if ((row + 1) % 3 == 0 && row < 8)
            {
                Console.WriteLine("├───────┼───────┼───────┤      ├───────┼───────┼───────┤");
            }
        }
        
        Console.WriteLine("└───────┴───────┴───────┘      └───────┴───────┴───────┘");
    }
    
    public static void DisplayStats(Grid grid)
    {
        var emptyCells = 0;
        var filledCells = 0;
        
        for (var row = 0; row < 9; row++)
        {
            for (var col = 0; col < 9; col++)
            {
                if (grid.GetCellValue((row, col)).HasValue)
                {
                    filledCells++;
                }
                else
                {
                    emptyCells++;
                }
            }
        }
        
        Console.WriteLine($"\n📊 Statistiques:");
        Console.WriteLine($"   - Cellules remplies: {filledCells}/81");
        Console.WriteLine($"   - Cellules vides: {emptyCells}/81");
        Console.WriteLine($"   - Pourcentage de remplissage: {(filledCells * 100.0 / 81):F1}%");
        
        var difficulty = emptyCells switch
        {
            <= 30 => "Facile",
            <= 40 => "Moyen",
            <= 50 => "Difficile",
            _ => "Expert"
        };
        
        Console.WriteLine($"   - Difficulté estimée: {difficulty}");
    }
}

