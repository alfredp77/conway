namespace conway;

public interface IAction
{
    string Id { get; }
    string Description { get; }
    GameState Execute();
}