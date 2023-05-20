using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class MenuAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IInputProcessor _inputProcessor;

    public MenuAction(IUserInputOutput userInputOutput, IInputProcessor inputProcessor)
    {
        _userInputOutput = userInputOutput;
        _inputProcessor = inputProcessor;
        Prompt = GetPrompt(_inputProcessor.Prompt);
    }

    public static string GetPrompt(string inputProcessorPrompt)
    {
        return $"{inputProcessorPrompt} or {Command.Exit} to go back to main menu:";
    }

    public string Id => _inputProcessor.Id;
    public string Description => _inputProcessor.Description;
    
    public string Prompt { get; }

    public GameParameters Execute(GameParameters gameParameters)
    {
        var processedInput = ProcessedInput.ValidAndContinue(gameParameters);
        while (processedInput.ShouldGetInput)
        {
            _userInputOutput.WriteLine(Prompt);
            var input = _userInputOutput.ReadLine();
            if (input == Command.Exit)
            {
                break;
            }

            processedInput = _inputProcessor.ProcessInput(input, processedInput.GameParameters);
            if (!processedInput.IsValid)
            {
                _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
            }
        }

        return processedInput.GameParameters;
    }
}