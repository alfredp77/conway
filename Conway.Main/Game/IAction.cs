namespace Conway.Main.Game;

public interface IAction
{
    string Id { get; }
    string Description { get; }
    GameParameters Execute(GameParameters gameParameters);
}