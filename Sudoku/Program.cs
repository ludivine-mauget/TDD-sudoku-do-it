﻿using Sudoku.Display;

namespace Sudoku;

class Program
{
    private static void Main()
    {
        Console.Clear();
        Console.WriteLine("╔═════════════════════════════════════════╗");
        Console.WriteLine("║     🎮 SUDOKU GENERATOR & SOLVER 🎮     ║");
        Console.WriteLine("╚═════════════════════════════════════════╝");
        
        while (true)
        {
            DisplayMenu();
            var choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    GenerateAndDisplayPuzzle();
                    break;
                case "2":
                    GenerateAndSolvePuzzle();
                    break;
                case "3":
                    GeneratePuzzleWithComparison();
                    break;
                case "4":
                    Console.WriteLine("\n👋 Merci d'avoir joué ! À bientôt !");
                    return;
                default:
                    Console.WriteLine("\n❌ Choix invalide. Veuillez réessayer.");
                    break;
            }
            
            Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
            Console.Clear();
        }
    }
    
    static void DisplayMenu()
    {
        Console.WriteLine("\n┌─────────────────────────────────────────┐");
        Console.WriteLine("│              MENU PRINCIPAL             │");
        Console.WriteLine("├─────────────────────────────────────────┤");
        Console.WriteLine("│ 1. Générer une grille de Sudoku         │");
        Console.WriteLine("│ 2. Générer et résoudre automatiquement  │");
        Console.WriteLine("│ 3. Comparaison Puzzle vs Solution       │");
        Console.WriteLine("│ 4. Quitter                              │");
        Console.WriteLine("└─────────────────────────────────────────┘");
        Console.Write("\nVotre choix (1-4): ");
    }
    
    static void GenerateAndDisplayPuzzle()
    {
        Console.Write("\n🎲 Combien de cellules voulez-vous retirer ? (20-60 recommandé): ");
        if (!int.TryParse(Console.ReadLine(), out var cellsToRemove) || cellsToRemove < 1 || cellsToRemove > 81)
        {
            Console.WriteLine("❌ Nombre invalide. Utilisation de 40 par défaut.");
            cellsToRemove = 40;
        }
        
        Console.WriteLine("\n⏳ Génération de la grille complète...");
        var fullGrid = Generator.Generator.GenerateFullGrid();
        
        Console.WriteLine("⏳ Création du puzzle avec solution unique...");
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
        
        GridDisplay.DisplayGrid(puzzleGrid, "🎯 VOTRE PUZZLE SUDOKU");
        GridDisplay.DisplayStats(puzzleGrid);
        
        Console.WriteLine("\n✅ Grille générée avec succès !");
    }
    
    static void GenerateAndSolvePuzzle()
    {
        Console.Write("\n🎲 Combien de cellules voulez-vous retirer ? (20-60 recommandé): ");
        if (!int.TryParse(Console.ReadLine(), out var cellsToRemove) || cellsToRemove < 1 || cellsToRemove > 81)
        {
            Console.WriteLine("❌ Nombre invalide. Utilisation de 40 par défaut.");
            cellsToRemove = 40;
        }
        
        Console.WriteLine("\n⏳ Génération de la grille complète...");
        var fullGrid = Generator.Generator.GenerateFullGrid();
        
        Console.WriteLine("⏳ Création du puzzle avec solution unique...");
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
        
        GridDisplay.DisplayGrid(puzzleGrid, "🎯 PUZZLE GÉNÉRÉ");
        GridDisplay.DisplayStats(puzzleGrid);
        
        Console.WriteLine("\n⏳ Résolution en cours...");
        var solvedGrid = Solver.Solver.Solve(puzzleGrid);
        
        if (solvedGrid.IsSolved())
        {
            GridDisplay.DisplayGrid(solvedGrid, "✅ SOLUTION TROUVÉE");
            Console.WriteLine("\n🎉 Puzzle résolu avec succès !");
        }
        else
        {
            Console.WriteLine("\n❌ Impossible de résoudre ce puzzle.");
        }
    }
    
    static void GeneratePuzzleWithComparison()
    {
        Console.Write("\n🎲 Combien de cellules voulez-vous retirer ? (20-60 recommandé): ");
        if (!int.TryParse(Console.ReadLine(), out var cellsToRemove) || cellsToRemove < 1 || cellsToRemove > 81)
        {
            Console.WriteLine("❌ Nombre invalide. Utilisation de 40 par défaut.");
            cellsToRemove = 40;
        }
        
        Console.WriteLine("\n⏳ Génération de la grille complète...");
        var fullGrid = Generator.Generator.GenerateFullGrid();
        
        Console.WriteLine("⏳ Création du puzzle avec solution unique...");
        var puzzleGrid = Generator.Generator.GeneratePuzzleWithUniqueSolution(fullGrid, cellsToRemove);
        
        Console.WriteLine("⏳ Résolution en cours...");
        var solvedGrid = Solver.Solver.Solve(puzzleGrid);
        
        GridDisplay.DisplayGridComparison(puzzleGrid, solvedGrid);
        GridDisplay.DisplayStats(puzzleGrid);
        
        Console.WriteLine("\n🎉 Génération et résolution terminées !");
    }
}

