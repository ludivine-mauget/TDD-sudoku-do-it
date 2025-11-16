namespace Sudoku.Tests.TestData;

/// <summary>
/// Contains test data for Sudoku grids, including initial values and expected possibilities.
/// </summary>
public static class SudokuGridTestData
{
    /// <summary>
    /// Grid from the reference image with partially filled values.
    /// </summary>
    public static class TestGridPossibilitiesCheck
    {
        /// <summary>
        /// Initial values to populate the grid (row, col) -> value.
        /// </summary>
        public static readonly Dictionary<(int row, int col), int> InitialValues = new()
        {
            // Row 0: . . . . 3 . . 5 1
            { (0, 4), 3 }, { (0, 7), 5 }, { (0, 8), 1 },
            
            // Row 1: . . 3 6 . . . . .
            { (1, 2), 3 }, { (1, 3), 6 },
            
            // Row 2: . 2 . . 9 4 8 . .
            { (2, 1), 2 }, { (2, 4), 9 }, { (2, 5), 4 }, { (2, 6), 8 },
            
            // Row 3: . . . . 5 . . 7 .
            { (3, 4), 5 }, { (3, 7), 7 },
            
            // Row 4: 5 9 . . . . . 6 2
            { (4, 0), 5 }, { (4, 1), 9 }, { (4, 7), 6 }, { (4, 8), 2 },
            
            // Row 5: . 8 . . 2 . . . .
            { (5, 1), 8 }, { (5, 4), 2 },
            
            // Row 6: . . 4 9 1 . . 8 .
            { (6, 2), 4 }, { (6, 3), 9 }, { (6, 4), 1 }, { (6, 7), 8 },
            
            // Row 7: . . . . . 2 4 . .
            { (7, 5), 2 }, { (7, 6), 4 },
            
            // Row 8: 2 3 . . 8 . . . .
            { (8, 0), 2 }, { (8, 1), 3 }, { (8, 4), 8 }
        };

        /// <summary>
        /// Expected possibilities (pencil marks) for each empty cell.
        /// </summary>
        public static readonly Dictionary<(int row, int col), HashSet<int>> ExpectedPossibilities = new()
        {
            // Row 0
            { (0, 0), new HashSet<int> { 4, 6, 7, 8, 9 } },
            { (0, 1), new HashSet<int> { 4, 6, 7 } },
            { (0, 2), new HashSet<int> { 6, 7, 8, 9 } },
            { (0, 3), new HashSet<int> { 2, 7, 8 } },
            { (0, 5), new HashSet<int> { 7, 8 } },
            { (0, 6), new HashSet<int> { 2, 6, 7, 9 } },
            
            // Row 1
            { (1, 0), new HashSet<int> { 1, 4, 7, 8, 9 } },
            { (1, 1), new HashSet<int> { 1, 4, 5, 7 } },
            { (1, 4), new HashSet<int> { 7 } },
            { (1, 5), new HashSet<int> { 1, 5, 7, 8 } },
            { (1, 6), new HashSet<int> {  2, 7, 9 } },
            { (1, 7), new HashSet<int> { 2, 4, 9 } },
            { (1, 8), new HashSet<int> { 4, 7, 9 } },
            
            // Row 2
            { (2, 0), new HashSet<int> { 1, 6, 7 } },
            { (2, 2), new HashSet<int> { 1, 5, 6, 7 } },
            { (2, 3), new HashSet<int> { 1, 5, 7 } },
            { (2, 7), new HashSet<int> { 3 } },
            { (2, 8), new HashSet<int> { 3, 6, 7 } },
            
            // Row 3
            { (3, 0), new HashSet<int> { 1, 3, 4, 6 } },
            { (3, 1), new HashSet<int> { 1, 4, 6 } },
            { (3, 2), new HashSet<int> { 1, 2, 6 } },
            { (3, 3), new HashSet<int> { 1, 3, 4, 8 } },
            { (3, 5), new HashSet<int> { 1, 3, 6, 8, 9 } },
            { (3, 6), new HashSet<int> { 1, 3, 9 } },
            { (3, 8), new HashSet<int> { 3, 4, 8, 9 } },
            
            // Row 4
            { (4, 2), new HashSet<int> { 1, 7 } },
            { (4, 3), new HashSet<int> { 1, 3, 4, 7, 8 } },
            { (4, 4), new HashSet<int> { 4, 7 } },
            { (4, 5), new HashSet<int> { 1, 3, 7, 8 } },
            { (4, 6), new HashSet<int> { 1, 3 } },
            
            // Row 5
            { (5, 0), new HashSet<int> { 1, 3, 4, 6, 7 } },
            { (5, 2), new HashSet<int> { 1, 6, 7 } },
            { (5, 3), new HashSet<int> { 1, 3, 4, 7 } },
            { (5, 5), new HashSet<int> { 1, 3, 6, 7, 9 } },
            { (5, 6), new HashSet<int> { 1, 3, 5, 9 } },
            { (5, 7), new HashSet<int> { 1, 3, 4, 9 } },
            { (5, 8), new HashSet<int> { 3, 4, 5, 9 } },
            
            // Row 6
            { (6, 0), new HashSet<int> { 6, 7 } },
            { (6, 1), new HashSet<int> { 5, 6, 7 } },
            { (6, 5), new HashSet<int> { 3, 5, 6, 7 } },
            { (6, 6), new HashSet<int> { 2, 3, 5, 6, 7 } },
            { (6, 8), new HashSet<int> { 3, 5, 6, 7 } },
            
            // Row 7
            { (7, 0), new HashSet<int> { 1, 6, 7, 8, 9 } },
            { (7, 1), new HashSet<int> { 1, 5, 6, 7 } },
            { (7, 2), new HashSet<int> { 1, 5, 6, 7, 8, 9 } },
            { (7, 3), new HashSet<int> { 3, 5, 7 } },
            { (7, 4), new HashSet<int> { 6, 7 } },
            { (7, 7), new HashSet<int> { 1, 3, 9 } },
            { (7, 8), new HashSet<int> { 3, 5, 6, 7, 9 } },
            
            // Row 8
            { (8, 2), new HashSet<int> { 1, 5, 6, 7, 9 } },
            { (8, 3), new HashSet<int> { 4, 5, 7 } },
            { (8, 5), new HashSet<int> { 5, 6, 7 } },
            { (8, 6), new HashSet<int> { 1, 5, 6, 7, 9 } },
            { (8, 7), new HashSet<int> { 1, 9 } },
            { (8, 8), new HashSet<int> { 5, 6, 7, 9 } }
        };
    }

    public static class TestBeginnerGridToSolve
    {
        public static readonly Dictionary<(int row, int col), int> InitialValues = new()
        {
            // Row 0
            { (0, 2), 6 }, { (0, 6), 5 }, { (0, 8), 8 },
            
            // Row 1
            { (1, 0), 1 }, { (1, 2), 2 }, { (1, 3), 3 }, { (1, 4), 8 }, { (1, 8), 4 },
            
            // Row 2
            { (2, 3), 2 }, { (2, 6), 1 }, { (2, 7), 9 },
            
            // Row 3
            { (3, 4), 6 }, { (3, 5), 3 }, { (3, 7), 4 }, { (3, 8), 5 },
            
            // Row 4
            { (4, 1), 6 }, { (4, 2), 3 }, { (4, 3), 4 }, { (4, 5), 5 }, { (4, 6), 8 }, { (4, 7), 7 },
            
            // Row 5
            { (5, 0), 5 }, { (5, 1), 4 }, { (5, 3), 9 }, { (5, 4), 2 },
            
            // Row 6
            { (6, 1), 8 }, { (6, 2), 7 }, { (6, 5), 4 },
            
            // Row 7
            { (7, 0), 2 }, { (7, 4), 9 }, { (7, 5), 8 }, { (7, 6), 4 }, { (7, 8), 7 },
            
            // Row 8
            { (8, 0), 4 }, { (8, 2), 9 }, { (8, 6), 3 }
        };

        /// <summary>
        /// Expected solution for the beginner grid.
        /// </summary>
        public static readonly Dictionary<(int row, int col), int> ExpectedSolution = new()
        {
            // Row 0: 3 9 6 7 4 1 5 2 8
            { (0, 0), 3 }, { (0, 1), 9 }, { (0, 2), 6 }, { (0, 3), 7 }, { (0, 4), 4 }, { (0, 5), 1 }, { (0, 6), 5 }, { (0, 7), 2 }, { (0, 8), 8 },
            
            // Row 1: 1 5 2 3 8 9 7 6 4
            { (1, 0), 1 }, { (1, 1), 5 }, { (1, 2), 2 }, { (1, 3), 3 }, { (1, 4), 8 }, { (1, 5), 9 }, { (1, 6), 7 }, { (1, 7), 6 }, { (1, 8), 4 },
            
            // Row 2: 8 7 4 2 5 6 1 9 3
            { (2, 0), 8 }, { (2, 1), 7 }, { (2, 2), 4 }, { (2, 3), 2 }, { (2, 4), 5 }, { (2, 5), 6 }, { (2, 6), 1 }, { (2, 7), 9 }, { (2, 8), 3 },
            
            // Row 3: 7 2 1 8 6 3 9 4 5
            { (3, 0), 7 }, { (3, 1), 2 }, { (3, 2), 1 }, { (3, 3), 8 }, { (3, 4), 6 }, { (3, 5), 3 }, { (3, 6), 9 }, { (3, 7), 4 }, { (3, 8), 5 },
            
            // Row 4: 9 6 3 4 1 5 8 7 2
            { (4, 0), 9 }, { (4, 1), 6 }, { (4, 2), 3 }, { (4, 3), 4 }, { (4, 4), 1 }, { (4, 5), 5 }, { (4, 6), 8 }, { (4, 7), 7 }, { (4, 8), 2 },
            
            // Row 5: 5 4 8 9 2 7 6 3 1
            { (5, 0), 5 }, { (5, 1), 4 }, { (5, 2), 8 }, { (5, 3), 9 }, { (5, 4), 2 }, { (5, 5), 7 }, { (5, 6), 6 }, { (5, 7), 3 }, { (5, 8), 1 },
            
            // Row 6: 6 8 7 1 3 4 2 5 9
            { (6, 0), 6 }, { (6, 1), 8 }, { (6, 2), 7 }, { (6, 3), 1 }, { (6, 4), 3 }, { (6, 5), 4 }, { (6, 6), 2 }, { (6, 7), 5 }, { (6, 8), 9 },
            
            // Row 7: 2 3 5 6 9 8 4 1 7
            { (7, 0), 2 }, { (7, 1), 3 }, { (7, 2), 5 }, { (7, 3), 6 }, { (7, 4), 9 }, { (7, 5), 8 }, { (7, 6), 4 }, { (7, 7), 1 }, { (7, 8), 7 },
            
            // Row 8: 4 1 9 5 7 2 3 8 6
            { (8, 0), 4 }, { (8, 1), 1 }, { (8, 2), 9 }, { (8, 3), 5 }, { (8, 4), 7 }, { (8, 5), 2 }, { (8, 6), 3 }, { (8, 7), 8 }, { (8, 8), 6 }
        };
    }
    
    public static class TestHardGridToSolve
    {
        public static readonly Dictionary<(int row, int col), int> InitialValues = new()
        {
            // Row 0: . . 1 . . 8 . 7 3 (solution: 421958673)
            { (0, 2), 1 }, { (0, 5), 8 }, { (0, 7), 7 }, { (0, 8), 3 },
            
            // Row 1: . . 5 6 . . . . 1 (solution: 385674291)
            { (1, 2), 5 }, { (1, 3), 6 }, { (1, 8), 1 },
            
            // Row 2: 7 . . . . 1 . . . (solution: 769321458)
            { (2, 0), 7 }, { (2, 5), 1 },
            
            // Row 3: . 9 . 8 1 . . . . (solution: 694813527)
            { (3, 1), 9 }, { (3, 3), 8 }, { (3, 4), 1 },
            
            // Row 4: 5 3 . . . . . 4 6 (solution: 538792146)
            { (4, 0), 5 }, { (4, 1), 3 }, { (4, 7), 4 }, { (4, 8), 6 },
            
            // Row 5: . . . . 6 5 . 3 . (solution: 172465893)
            { (5, 4), 6 }, { (5, 5), 5 }, { (5, 7), 3 },
            
            // Row 6: . . . 1 . . . . 4 (solution: 256137984)
            { (6, 3), 1 }, { (6, 8), 4 },
            
            // Row 7: 8 . . . . 9 3 . . (solution: 817249365)
            { (7, 0), 8 }, { (7, 5), 9 }, { (7, 6), 3 },
            
            // Row 8: 9 4 . 5 . . 7 . . (solution: 943586712)
            { (8, 0), 9 }, { (8, 1), 4 }, { (8, 3), 5 }, { (8, 6), 7 }
        };

        /// <summary>
        /// Expected solution for the hard grid.
        /// </summary>
        public static readonly Dictionary<(int row, int col), int> ExpectedSolution = new()
        {
            // Row 0: 4 2 1 9 5 8 6 7 3
            { (0, 0), 4 }, { (0, 1), 2 }, { (0, 2), 1 }, { (0, 3), 9 }, { (0, 4), 5 }, { (0, 5), 8 }, { (0, 6), 6 }, { (0, 7), 7 }, { (0, 8), 3 },
            
            // Row 1: 3 8 5 6 7 4 2 9 1
            { (1, 0), 3 }, { (1, 1), 8 }, { (1, 2), 5 }, { (1, 3), 6 }, { (1, 4), 7 }, { (1, 5), 4 }, { (1, 6), 2 }, { (1, 7), 9 }, { (1, 8), 1 },
            
            // Row 2: 7 6 9 3 2 1 4 5 8
            { (2, 0), 7 }, { (2, 1), 6 }, { (2, 2), 9 }, { (2, 3), 3 }, { (2, 4), 2 }, { (2, 5), 1 }, { (2, 6), 4 }, { (2, 7), 5 }, { (2, 8), 8 },
            
            // Row 3: 6 9 4 8 1 3 5 2 7
            { (3, 0), 6 }, { (3, 1), 9 }, { (3, 2), 4 }, { (3, 3), 8 }, { (3, 4), 1 }, { (3, 5), 3 }, { (3, 6), 5 }, { (3, 7), 2 }, { (3, 8), 7 },
            
            // Row 4: 5 3 8 7 9 2 1 4 6
            { (4, 0), 5 }, { (4, 1), 3 }, { (4, 2), 8 }, { (4, 3), 7 }, { (4, 4), 9 }, { (4, 5), 2 }, { (4, 6), 1 }, { (4, 7), 4 }, { (4, 8), 6 },
            
            // Row 5: 1 7 2 4 6 5 8 3 9
            { (5, 0), 1 }, { (5, 1), 7 }, { (5, 2), 2 }, { (5, 3), 4 }, { (5, 4), 6 }, { (5, 5), 5 }, { (5, 6), 8 }, { (5, 7), 3 }, { (5, 8), 9 },
            
            // Row 6: 2 5 6 1 3 7 9 8 4
            { (6, 0), 2 }, { (6, 1), 5 }, { (6, 2), 6 }, { (6, 3), 1 }, { (6, 4), 3 }, { (6, 5), 7 }, { (6, 6), 9 }, { (6, 7), 8 }, { (6, 8), 4 },
            
            // Row 7: 8 1 7 2 4 9 3 6 5
            { (7, 0), 8 }, { (7, 1), 1 }, { (7, 2), 7 }, { (7, 3), 2 }, { (7, 4), 4 }, { (7, 5), 9 }, { (7, 6), 3 }, { (7, 7), 6 }, { (7, 8), 5 },
            
            // Row 8: 9 4 3 5 8 6 7 1 2
            { (8, 0), 9 }, { (8, 1), 4 }, { (8, 2), 3 }, { (8, 3), 5 }, { (8, 4), 8 }, { (8, 5), 6 }, { (8, 6), 7 }, { (8, 7), 1 }, { (8, 8), 2 }
        };
    }
    
    public static class TestGridWithNoSolution
    {
        public static readonly Dictionary<(int row, int col), int> InitialValues = new()
        {
            // Row 0
            { (0, 0), 1 }, { (0, 1), 2 }, { (0, 2), 3 }, { (0, 3), 4 }, { (0, 4), 5 }, { (0, 5), 6 }, { (0, 6), 7 }, { (0, 7), 8 }, { (0, 8), 9 },
            
            // Row 1
            { (1, 0), 1 } // Duplicate '1' in the first column to create no solution scenario
        };
    }
    
    public static class TestGridWithMultipleSolutions
    {
        /// <summary>
        /// A grid that has multiple solutions.
        /// Initial grid:
        /// 0 8 0 | 0 0 9 | 7 4 3
        /// 0 5 0 | 0 0 8 | 0 1 0
        /// 0 1 0 | 0 0 0 | 0 0 0
        /// ------+-------+------
        /// 8 0 0 | 0 0 5 | 0 0 0
        /// 0 0 0 | 8 0 4 | 0 0 0
        /// 0 0 0 | 3 0 0 | 0 0 6
        /// ------+-------+------
        /// 0 0 0 | 0 0 0 | 0 7 0
        /// 0 3 0 | 5 0 0 | 0 8 0
        /// 9 7 2 | 4 0 0 | 0 5 0
        /// </summary>
        public static readonly Dictionary<(int row, int col), int> InitialValues = new()
        {
            // Row 0: 0 8 0 0 0 9 7 4 3
            { (0, 1), 8 }, { (0, 5), 9 }, { (0, 6), 7 }, { (0, 7), 4 }, { (0, 8), 3 },
            
            // Row 1: 0 5 0 0 0 8 0 1 0
            { (1, 1), 5 }, { (1, 5), 8 }, { (1, 7), 1 },
            
            // Row 2: 0 1 0 0 0 0 0 0 0
            { (2, 1), 1 },
            
            // Row 3: 8 0 0 0 0 5 0 0 0
            { (3, 0), 8 }, { (3, 5), 5 },
            
            // Row 4: 0 0 0 8 0 4 0 0 0
            { (4, 3), 8 }, { (4, 5), 4 },
            
            // Row 5: 0 0 0 3 0 0 0 0 6
            { (5, 3), 3 }, { (5, 8), 6 },
            
            // Row 6: 0 0 0 0 0 0 0 7 0
            { (6, 7), 7 },
            
            // Row 7: 0 3 0 5 0 0 0 8 0
            { (7, 1), 3 }, { (7, 3), 5 }, { (7, 7), 8 },
            
            // Row 8: 9 7 2 4 0 0 0 5 0
            { (8, 0), 9 }, { (8, 1), 7 }, { (8, 2), 2 }, { (8, 3), 4 }, { (8, 7), 5 }
        };
    }
}

