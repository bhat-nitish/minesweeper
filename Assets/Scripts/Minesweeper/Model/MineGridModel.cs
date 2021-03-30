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
            // This will ideally call an api to fetch data and then set properties. 
            // A service should be injected in the model to fetch data from an external source.
            if (GameData.GetCurrentGameLevel() == GameLevel.Easy)
            {
                rows = 5;
                columns = 5;
                mineCount = 1;
            }
            else if (GameData.GetCurrentGameLevel() == GameLevel.Medium)
            {
                rows = 10;
                columns = 10;
                mineCount = 30;
            }
            else if (GameData.GetCurrentGameLevel() == GameLevel.Difficult)
            {
                rows = 15;
                columns = 20;
                mineCount = 100;
            }
            else
            {
                rows = 15;
                columns = 15;
                mineCount = 20;
            }
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