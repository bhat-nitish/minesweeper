namespace Minesweeper.Model
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
}