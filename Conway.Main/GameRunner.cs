using System.Drawing;

namespace Conway.Main;

public class GameRunner : IGameRunner
{
    public GameState GenerateInitialState(GameParameters gameParameters)
    {
        return new GameState { Parameters = gameParameters, LiveCells = gameParameters.InitialLiveCells };
    }

    public GameState GenerateNextState(GameState current)
    {
        return current with { LiveCells = new List<Point>() };
    }
}