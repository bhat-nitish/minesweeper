using Minesweeper.Model;

namespace MineSweeper.Models
{
    public class MineCell : IMineCell
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsMine { get; set; }
        public bool IsCellPlayed { get; set; }

        public int MineCount { get; set; }
    }
}