using MineSweeper.Models;

namespace MineSweeper.Game.Minesweeper
{
    public class MineGridModel : IMineGridModel
    {
        private int rows { get; }

        private int columns { get; }

        private int mineCount { get; }

        public MineGridModel()
        {
            rows = 15;
            columns = 15;
            mineCount = 50;
        }

        public int GetRows()
        {
            return this.rows;
        }

        public int GetColumns()
        {
            return this.columns;
        }

        public int GetMineCount()
        {
            return this.mineCount;
        }
    }
}