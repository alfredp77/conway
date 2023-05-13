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
        var nextLiveCells = new List<Point>();
        for (var x=1; x<=current.Parameters.Width; x++)
        {
            for (var y=1; y<=current.Parameters.Height; y++)
            {
                var cell = new Point(x, y);
                var neighbours = GetNeighbours(cell, current.LiveCells);
                switch (neighbours.Count)
                {
                    case 3:
                    case 2 when current.LiveCells.Contains(cell):
                        nextLiveCells.Add(cell);
                        break;
                }
            }
        }
        
        return current with { LiveCells = nextLiveCells };
    }

    private IReadOnlyList<Point> GetNeighbours(Point cell, ICollection<Point> currentLiveCells)
    {
        var neighbours = new List<Point>();
        for (var x = cell.X - 1; x <= cell.X + 1; x++)
        {
            for (var y = cell.Y - 1; y <= cell.Y + 1; y++)
            {
                if (x == cell.X && y == cell.Y)
                {
                    continue;
                }
                var neighbour = new Point(x, y);
                if (currentLiveCells.Contains(neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }
        }
        return neighbours;
    }
}