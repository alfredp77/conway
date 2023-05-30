using Conway.Main.Actions;
using Conway.Main.Game;

namespace Conway.Main.InputProcessors;

public interface IInputProcessor
{
    string Id { get; }
    string Description { get; }
    string Prompt { get; }
    ProcessedInput Initialize(GameParameters gameParameters) => ProcessedInput.ValidAndContinue(gameParameters);
    ProcessedInput ProcessInput(string input, GameParameters gameParameters);
}