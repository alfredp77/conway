using Conway.Main.Game;

namespace Conway.Main.Actions;

public interface IInputProcessor
{
    string Id { get; }
    string Description { get; }
    string Prompt { get; }
    ProcessedInput ProcessInput(string input, GameParameters gameParameters);
}