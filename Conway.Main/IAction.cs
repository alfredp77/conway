namespace conway;

public interface IAction
{
    string Id { get; }
    string Description { get; }
    void Execute();
}