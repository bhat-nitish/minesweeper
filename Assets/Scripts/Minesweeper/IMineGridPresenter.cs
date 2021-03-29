using System.Collections.Generic;
using MineSweeper.Models;

namespace MineSweeper.Game.Minesweeper
{
    public interface IMineGridPresenter
    {
        int GetRows();

        int GetColumns();

        int GetMineCount();

        MineCell[,] GetMineCells();

        MineCell GetMineCell(int x, int y);

        int GetTotalCells();

        int[,] GetMineLocations();

        bool HasGameEnded();

        int GetPlayedCellsCount();

        bool IncrementPlayedCellsCount();

        bool SetMineCellAsPlayed(int x, int y);

        void GenerateMineGrid();

        void GenerateMineLocations(int row, int column);

        List<MineCell> TraverseAdjacentCells(List<MineCell> cells, int row, int column);

        void UpdateMineCount();

        bool Checkwin();

        bool EndGame();
    }
}