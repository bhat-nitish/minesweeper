using System;
using System.Collections.Generic;
using Minesweeper.EventArgs;
using MineSweeper.Models;

namespace MineSweeper.Game.Minesweeper
{
    public interface IMineGridPresenter : IMineGridPresenterEvents
    {
        int GetRows();

        int GetColumns();

        MineCell[,] GetMineCells();

        MineCell GetMineCell(int x, int y);

        bool HasGameEnded();

        void GenerateMineGrid();

        void CellClicked(int x, int y);

        int GetMineCount();
    }
}