using System.Collections.Generic;
using MineSweeper.Models;

namespace MineSweeper.Game.Minesweeper
{
    public interface IMineGridView
    {
        void RevealAdjacentCells(List<MineCell> adjacentCells);

        void ShowWin();

        void ShowLost();

        void DisplayMineGrid();

        void PlaceMineCells();

        void UpdateMineCountForView();
    }
}