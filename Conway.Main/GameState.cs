using System.Drawing;

namespace Conway.Main;

public record GameState
{
    public GameParameters Parameters { get; init; } = GameParameters.Initial;
    public List<Point> LiveCells { get; init; } = new();
}