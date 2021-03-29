using System.Collections.Generic;
using System.Linq;
using MineSweeper.Models;
using Random = System.Random;

namespace MineSweeper.Game.Minesweeper
{
    public class MineGridPresenter : IMineGridPresenter
    {
        private MineCell[,] mineCells;

        private int[,] mineLocations;

        private int totalCells;

        private bool gameEnd;

        private int cellsPlayedCount;

        public IMineGridModel _model { get; }

        private MineGridPresenter(IMineGridModel mineGridModel)
        {
            _model = mineGridModel;
            mineCells = new MineCell[_model.GetRows(), _model.GetColumns()];
            mineLocations = new int[_model.GetRows(), _model.GetColumns()];
            totalCells = _model.GetRows() * _model.GetColumns();
        }

        public bool Checkwin()
        {
            return cellsPlayedCount == (totalCells - _model.GetMineCount());
        }

        private bool InGrid(int x, int y) => (x >= 0 && x < _model.GetRows()) && (y >= 0 && y < _model.GetColumns());

        private int CalculateMineCountForAdjacentCells(List<MineCell> adjacentCells) =>
            adjacentCells.Where(c => c.IsMine).Count();

        public int GetRows() => _model.GetRows();

        public int GetColumns() => _model.GetColumns();

        public int GetMineCount() => _model.GetMineCount();
        public MineCell[,] GetMineCells() => mineCells;

        public MineCell GetMineCell(int x, int y) => mineCells[x, y];

        public int GetTotalCells() => totalCells;

        public int[,] GetMineLocations() => mineLocations;

        public bool HasGameEnded() => gameEnd;

        public bool EndGame()
        {
            gameEnd = true;
            return true;
        }

        public int GetPlayedCellsCount() => cellsPlayedCount;

        public bool IncrementPlayedCellsCount()
        {
            cellsPlayedCount++;
            return true;
        }

        public bool SetMineCellAsPlayed(int x, int y)
        {
            mineCells[x, y].IsCellPlayed = true;
            return true;
        }

        public void GenerateMineGrid()
        {
            int id = 1;
            for (int i = 0; i < _model.GetRows(); i++)
            {
                for (int j = 0; j < _model.GetRows(); j++)
                {
                    mineCells[i, j] = new MineCell()
                    {
                        Id = id++,
                        X = i,
                        Y = j,
                        IsMine = mineLocations[i, j] == 1
                    };
                }
            }
        }

        public void GenerateMineLocations(int row, int column)
        {
            mineLocations = new int[_model.GetRows(), _model.GetColumns()];
            int generatedMines = 0;
            Random r = new Random();
            while (generatedMines < _model.GetMineCount())
            {
                int randomMineRow = r.Next(0, _model.GetRows());
                int randomMineColumn = r.Next(0, _model.GetColumns());
                if (mineLocations[randomMineRow, randomMineColumn] == 1 ||
                    (randomMineRow == row && randomMineColumn == column))
                {
                    // mine aleady present
                    continue;
                }
                else
                {
                    mineLocations[randomMineRow, randomMineColumn] = 1;
                    mineCells[randomMineRow, randomMineColumn].IsMine = true;
                    generatedMines++;
                }
            }
        }

        public List<MineCell> TraverseAdjacentCells(List<MineCell> cells, int row, int column)
        {
            if (mineCells[row, column].IsMine || mineCells[row, column].IsCellPlayed)
            {
                return cells;
            }

            Stack<MineCell> traversedCells = new Stack<MineCell>();
            traversedCells.Push(mineCells[row, column]);
            cells.Add(mineCells[row, column]);
            List<MineCell> adjacentCells = new List<MineCell>();
            while (traversedCells.Any())
            {
                MineCell cell = traversedCells.Pop();
                mineCells[row, column].IsCellPlayed = true;
                if (cell.MineCount == 0)
                {
                    adjacentCells = GetAdjacentMineCells(cell.X, cell.Y);
                    adjacentCells = adjacentCells.Where(a => !a.IsMine).ToList();
                    foreach (var adjacentCell in adjacentCells)
                    {
                        if (!adjacentCell.IsCellPlayed)
                        {
                            cellsPlayedCount++;
                            TraverseAdjacentCells(cells, adjacentCell.X, adjacentCell.Y);
                        }
                    }
                }
            }

            if (adjacentCells.Count > 0)
                cells.AddRange(adjacentCells);

            return cells;
        }

        private List<MineCell> GetAdjacentMineCells(int row, int column)
        {
            List<MineCell> adjacentCells = new List<MineCell>();

            for (int i = row - 1; i <= row + 1; i++)
            {
                for (int j = column - 1; j <= column + 1; j++)
                {
                    if ((i != row || j != column) && (InGrid(i, j) && !mineCells[i, j].IsCellPlayed))
                    {
                        adjacentCells.Add(mineCells[i, j]);
                    }
                }
            }

            return adjacentCells;
        }

        public void UpdateMineCount()
        {
            for (int i = 0; i < _model.GetRows(); i++)
            {
                for (int j = 0; j < _model.GetColumns(); j++)
                {
                    var adjacentCells = GetAdjacentMineCells(i, j);
                    mineCells[i, j].MineCount = CalculateMineCountForAdjacentCells(adjacentCells);
                }
            }
        }
    }
}