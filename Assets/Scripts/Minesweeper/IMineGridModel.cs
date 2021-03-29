using MineSweeper.Models;

namespace MineSweeper.Game.Minesweeper
{
    public interface IMineGridModel
    {
        int GetRows();

        int GetColumns();

        int GetMineCount();
    }
}