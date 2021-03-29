using System;
using Minesweeper.EventArgs;

namespace MineSweeper.Game.Minesweeper
{
    public interface IMineGridPresenterEvents
    {
        event EventHandler OnGameWin;

        event EventHandler OnMineClicked;

        event EventHandler<CellClickedEventArgs> OnCellClickHandled;
    }
}