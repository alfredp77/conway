namespace Conway.Main;

public interface IGameRunner
{
    GameState GenerateInitialState(GameParameters gameParameters);
    GameState GenerateNextState(GameState current);
}