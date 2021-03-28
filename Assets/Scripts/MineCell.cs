namespace MineSweeper.Models
{
    public interface IMineCell
    {
        int Id { get; set; }
        int X { get; set; }
        int Y { get; set; }
        bool IsMine { get; set; }
        bool IsCellPlayed { get; set; }
        int MineCount { get; set; }
    }

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