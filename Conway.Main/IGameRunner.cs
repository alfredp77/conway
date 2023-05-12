namespace Conway.Main;

public interface IGameRunner
{
    GameState Run(GameState current);
}