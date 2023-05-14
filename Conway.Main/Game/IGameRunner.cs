namespace Conway.Main.Game;

public interface IGameRunner
{
    GameState GenerateInitialState(GameParameters gameParameters);
    GameState GenerateNextState(GameState current);
}