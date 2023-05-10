using System.Drawing;

namespace Conway.Main;

public record GameState
{
    public static GameState Initial => new();
    public bool IsEnd { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public int NumberOfGeneration { get; init; }
    public List<Point> LiveCells { get; init; } = new();
}