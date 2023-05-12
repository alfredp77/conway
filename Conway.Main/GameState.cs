using System.Drawing;

namespace Conway.Main;

public record GameState
{
    public static readonly GameState NoLiveCells = new();
    public bool IsEnd { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public int NumberOfGeneration { get; init; }
    public List<Point> InitialLiveCells { get; init; } = new();
    
    public List<Point>? CurrentLiveCells { get; set; }
}