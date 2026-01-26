using System.Diagnostics;
using Sudoku.Constants;
using Sudoku.Models;

namespace Sudoku.Client.Services;

public class GameStateService
{
    private Grid? _initialGrid;
    private readonly List<GameMove> _moveHistory = [];
    private int _currentMoveIndex = -1;
    private readonly Stopwatch _gameStopwatch = new();

    public event Action? OnStateChanged;

    public Grid? CurrentGrid { get; private set; }
    public bool[,]? FixedCells { get; private set; }
    public bool IsGameActive { get; private set; }

    public TimeSpan ElapsedTime => _gameStopwatch.Elapsed;
    public bool CanUndo => _currentMoveIndex >= 0;
    public bool CanRedo => _currentMoveIndex < _moveHistory.Count - 1;

    public void StartNewGame(Grid grid)
    {
        CurrentGrid = grid.Clone();
        _initialGrid = grid.Clone();
        FixedCells = new bool[GridConstants.GridSize, GridConstants.GridSize];
        
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                FixedCells[row, col] = grid.GetCellNumber(row, col) != 0;
            }
        }

        _moveHistory.Clear();
        _currentMoveIndex = -1;
        _gameStopwatch.Restart();
        IsGameActive = true;

        NotifyStateChanged();
    }

    public void SetCellValue(int row, int col, int value)
    {
        if (CurrentGrid == null || FixedCells == null || FixedCells[row, col])
            return;

        var oldValue = CurrentGrid.GetCellNumber(row, col);
        
        // Supprimer tous les mouvements après le mouvement actuel (si on a fait undo)
        if (_currentMoveIndex < _moveHistory.Count - 1)
        {
            _moveHistory.RemoveRange(_currentMoveIndex + 1, _moveHistory.Count - _currentMoveIndex - 1);
        }

        _moveHistory.Add(new GameMove(row, col, oldValue, value));
        _currentMoveIndex++;

        CurrentGrid.SetCellNumber(row, col, value);
        NotifyStateChanged();
    }

    public void Undo()
    {
        if (!CanUndo || CurrentGrid == null)
            return;

        var move = _moveHistory[_currentMoveIndex];
        CurrentGrid.SetCellNumber(move.Row, move.Col, move.OldValue);
        _currentMoveIndex--;

        NotifyStateChanged();
    }

    public void Redo()
    {
        if (!CanRedo || CurrentGrid == null)
            return;

        _currentMoveIndex++;
        var move = _moveHistory[_currentMoveIndex];
        CurrentGrid.SetCellNumber(move.Row, move.Col, move.NewValue);

        NotifyStateChanged();
    }

    public void Reset()
    {
        if (_initialGrid == null)
            return;

        CurrentGrid = _initialGrid.Clone();
        _moveHistory.Clear();
        _currentMoveIndex = -1;

        NotifyStateChanged();
    }

    public void ClearGame()
    {
        CurrentGrid = null;
        _initialGrid = null;
        FixedCells = null;
        _moveHistory.Clear();
        _currentMoveIndex = -1;
        _gameStopwatch.Stop();
        _gameStopwatch.Reset();
        IsGameActive = false;

        NotifyStateChanged();
    }

    public void SetSolution(Grid solution)
    {
        CurrentGrid = solution.Clone();
        IsGameActive = false;
        _gameStopwatch.Stop();
        NotifyStateChanged();
    }

    public bool IsCellFixed(int row, int col)
    {
        return FixedCells?[row, col] ?? false;
    }

    private void NotifyStateChanged()
    {
        OnStateChanged?.Invoke();
    }

    private record GameMove(int Row, int Col, int OldValue, int NewValue);
}

public static class GridExtensions
{
    public static Grid Clone(this Grid grid)
    {
        var clone = new Grid();
        for (var row = 0; row < GridConstants.GridSize; row++)
        {
            for (var col = 0; col < GridConstants.GridSize; col++)
            {
                clone.SetCellNumber(row, col, grid.GetCellNumber(row, col));
            }
        }
        return clone;
    }
}

