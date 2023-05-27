using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class RunProcessor : IInputProcessor
{
    public const string ID = "4";
    public const string NextGenerationPrompt = "Enter > to go to next generation";
    public const string EndOfGenerationPrompt = "End of generation. Press any key to return to main menu";

    private readonly IGameRunner _gameRunner;
    private readonly ILiveCellsPrinter _printer;

    public RunProcessor(IGameRunner gameRunner, ILiveCellsPrinter printer)
    {
        _gameRunner = gameRunner;
        _printer = printer;
        Prompt = NextGenerationPrompt;
    }

    public string Id => ID;
    public string Description => "Run";
    public string Prompt { get; private set; }
    public GameState? CurrentState { get; private set; }

    public ProcessedInput Initialize(GameParameters gameParameters)
    {
        CurrentState = _gameRunner.GenerateInitialState(gameParameters);
        _printer.Print("Initial position", CurrentState);
        return ProcessedInput.ValidAndContinue(gameParameters, NextGenerationPrompt);
    }

    public ProcessedInput ProcessInput(string input, GameParameters gameParameters)
    {
        if (input != Command.Next)
        {
            return ProcessedInput.Invalid(gameParameters);
        }
        CurrentState = _gameRunner.GenerateNextState(CurrentState);
        _printer.Print($"Generation {CurrentState.NumberOfGenerations}", CurrentState);

        if (CurrentState.NumberOfGenerations == gameParameters.NumberOfGeneration)
        {
            Prompt = EndOfGenerationPrompt;
            return ProcessedInput.ValidAndExit(gameParameters, EndOfGenerationPrompt);
        }
        return ProcessedInput.ValidAndContinue(gameParameters, NextGenerationPrompt);
    }
}