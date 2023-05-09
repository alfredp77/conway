namespace Conway.Main;

public interface IAction
{
    string Id { get; }
    string Description { get; }
    GameState Execute(GameState gameState);
}