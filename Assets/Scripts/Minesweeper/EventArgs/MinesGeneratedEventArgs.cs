using System;
using System.Collections.Generic;
using MineSweeper.Models;

namespace Minesweeper.EventArgs
{
    public class CellClickedEventArgs : System.EventArgs
    {
        public List<MineCell> AdjacentCells { get; set; }

        public bool IsFirstClick { get; set; }

        public int MineCount { get; set; }

        public int X { get; set; }

        public int Y { get; set; }
    }
}