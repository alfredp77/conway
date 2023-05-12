namespace Conway.Main;

public interface IAction
{
    string Id { get; }
    string Description { get; }
    GameParameters Execute(GameParameters gameParameters);
}