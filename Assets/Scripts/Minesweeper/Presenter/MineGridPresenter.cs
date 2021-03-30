using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Minesweeper.EventArgs;
using MineSweeper.Models;
using Random = System.Random;

namespace MineSweeper.Game.Minesweeper
{
    public class MineGridPresenter : IMineGridPresenter
    {
        #region Properties

        private MineCell[,] MineCells { get; }

        private int[,] MineLocations;

        private int TotalCells { get; }

        private bool GameEnd;

        private int CellsPlayedCount;

        private IMineGridModel Model { get; }

        #endregion

        #region Constructor

        private MineGridPresenter(IMineGridModel mineGridModel)
        {
            Model = mineGridModel;
            MineCells = new MineCell[Model.GetRows(), Model.GetColumns()];
            MineLocations = new int[Model.GetRows(), Model.GetColumns()];
            TotalCells = Model.GetRows() * Model.GetColumns();
        }

        #endregion

        #region Event Handlers

        public event EventHandler OnGameWin;

        public event EventHandler OnMineClicked;

        public event EventHandler<CellClickedEventArgs> OnCellClickHandled;

        #endregion

        #region Helpers
        
        private bool InGrid(int x, int y) => (x >= 0 && x < Model.GetRows()) && (y >= 0 && y < Model.GetColumns());

        private int CalculateMineCountForAdjacentCells(List<MineCell> adjacentCells) =>
            adjacentCells.Count(c => c.IsMine);

        public int GetRows() => Model.GetRows();

        public int GetColumns() => Model.GetColumns();

        public MineCell[,] GetMineCells() => MineCells;

        public MineCell GetMineCell(int x, int y) => MineCells[x, y];
        
        public bool HasGameEnded() => GameEnd;

        #endregion

        #region MineSweeper Logic

        public void GenerateMineGrid()
        {
            int id = 1;
            for (int i = 0; i < Model.GetRows(); i++)
            {
                for (int j = 0; j < Model.GetRows(); j++)
                {
                    MineCells[i, j] = new MineCell()
                    {
                        Id = id++,
                        X = i,
                        Y = j,
                        IsMine = MineLocations[i, j] == 1
                    };
                }
            }
        }

        private List<MineCell> TraverseAdjacentCells(List<MineCell> cells, int row, int column)
        {
            if (MineCells[row, column].IsMine || MineCells[row, column].IsCellPlayed)
            {
                return cells;
            }

            Stack<MineCell> traversedCells = new Stack<MineCell>();
            traversedCells.Push(MineCells[row, column]);
            cells.Add(MineCells[row, column]);
            List<MineCell> adjacentCells = new List<MineCell>();
            while (traversedCells.Any())
            {
                MineCell cell = traversedCells.Pop();
                MineCells[row, column].IsCellPlayed = true;
                if (cell.MineCount == 0)
                {
                    adjacentCells = GetAdjacentMineCells(cell.X, cell.Y);
                    adjacentCells = adjacentCells.Where(a => !a.IsMine).ToList();
                    foreach (var adjacentCell in adjacentCells)
                    {
                        if (!adjacentCell.IsCellPlayed)
                        {
                            CellsPlayedCount++;
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
                    if ((i != row || j != column) && (InGrid(i, j) && !MineCells[i, j].IsCellPlayed))
                    {
                        adjacentCells.Add(MineCells[i, j]);
                    }
                }
            }

            return adjacentCells;
        }

        private void UpdateMineCount()
        {
            for (int i = 0; i < Model.GetRows(); i++)
            {
                for (int j = 0; j < Model.GetColumns(); j++)
                {
                    var adjacentCells = GetAdjacentMineCells(i, j);
                    MineCells[i, j].MineCount = CalculateMineCountForAdjacentCells(adjacentCells);
                }
            }
        }

        private void GenerateMineLocations(int row, int column)
        {
            MineLocations = new int[Model.GetRows(), Model.GetColumns()];
            int generatedMines = 0;
            Random r = new Random();
            while (generatedMines < Model.GetMineCount())
            {
                int randomMineRow = r.Next(0, Model.GetRows());
                int randomMineColumn = r.Next(0, Model.GetColumns());
                if (MineLocations[randomMineRow, randomMineColumn] == 1 ||
                    (randomMineRow == row && randomMineColumn == column))
                {
                    // mine aleady present
                    continue;
                }
                else
                {
                    MineLocations[randomMineRow, randomMineColumn] = 1;
                    MineCells[randomMineRow, randomMineColumn].IsMine = true;
                    generatedMines++;
                }
            }
        }

        #endregion

        #region Event Invocation

        private void MineClicked()
        {
            GameEnd = true;
            OnMineClicked.Trigger(this);
        }

        private void CellClicked(CellClickedEventArgs eventArgs)
        {
            OnCellClickHandled.TriggerWithData(this, eventArgs);
        }

        private void MarkWin()
        {
            OnGameWin.Trigger(this);
        }

        #endregion

        #region Game Play

        private bool CheckWin()
        {
            return CellsPlayedCount == (TotalCells - Model.GetMineCount());
        }

        private bool EndGame()
        {
            GameEnd = true;
            return true;
        }

        public void CellClicked(int x, int y)
        {
            var mineCellCLicked = MineCells[x, y];
            if (mineCellCLicked == null || mineCellCLicked.IsCellPlayed || GameEnd) return;
            if (CellsPlayedCount == 0)
            {
                FirstCellClicked(mineCellCLicked);
            }
            else
            {
                if (mineCellCLicked.IsMine)
                {
                    MineClicked();
                }
                else
                {
                    if (mineCellCLicked.MineCount > 0)
                    {
                        CellWithAdjacentMineClicked(mineCellCLicked);
                    }
                    else
                    {
                        CellWithNoAdjacentMineClicked(mineCellCLicked);
                    }
                }
            }

            if (!CheckWin()) return;
            EndGame();
            MarkWin();
        }

        private void CellWithAdjacentMineClicked(MineCell mineCellCLicked)
        {
            IncrementPlayedCellsCount();
            CellClicked(new CellClickedEventArgs()
                {MineCount = mineCellCLicked.MineCount, X = mineCellCLicked.X, Y = mineCellCLicked.Y});
        }

        private void CellWithNoAdjacentMineClicked(MineCell mineCellCLicked)
        {
            IncrementPlayedCellsCount();
            var adjacentCellsTraversed = TraverseAdjacentCells(new List<MineCell>(),
                mineCellCLicked.X, mineCellCLicked.Y);
            CellClicked(new CellClickedEventArgs()
            {
                AdjacentCells = adjacentCellsTraversed, X = mineCellCLicked.X,
                Y = mineCellCLicked.Y
            });
        }

        private void FirstCellClicked(MineCell mineCellCLicked)
        {
            GenerateMineLocations(mineCellCLicked.X, mineCellCLicked.Y);
            UpdateMineCount();
            IncrementPlayedCellsCount();
            var adjacentCellsTraversed = TraverseAdjacentCells(new List<MineCell>(),
                mineCellCLicked.X, mineCellCLicked.Y);
            CellClicked(new CellClickedEventArgs()
            {
                AdjacentCells = adjacentCellsTraversed, IsFirstClick = true, X = mineCellCLicked.X,
                Y = mineCellCLicked.Y
            });
        }

        private bool IncrementPlayedCellsCount()
        {
            CellsPlayedCount++;
            return true;
        }

        #endregion
    }
}