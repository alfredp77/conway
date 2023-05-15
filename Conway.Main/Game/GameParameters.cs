using System.Drawing;

namespace Conway.Main.Game;

public record GameParameters
{
    public static readonly GameParameters Initial = new();
    public bool IsEnd { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public int NumberOfGeneration { get; init; }
    public List<Point> InitialLiveCells { get; init; } = new();
}