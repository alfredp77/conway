using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class InputGridSizeAction : IAction
{
    public const string ID = "1";
    public const string Prompt = "Please enter grid size in w h format (example: 10 15) or # to go back to main menu:";
    private readonly IUserInputOutput _userInputOutput;
    private readonly IInputProcessor _inputProcessor;
    private readonly string _prompt;

    public InputGridSizeAction(IUserInputOutput userInputOutput, IInputProcessor inputProcessor)
    {
        _userInputOutput = userInputOutput;
        _inputProcessor = inputProcessor;
        _prompt = $"{_inputProcessor.Prompt} or {Command.Exit} to go back to main menu:";
    }

    public string Id => ID;
    public string Description => "Specify grid size";
    
    public GameParameters Execute(GameParameters gameParameters)
    {
        var processedInput = ProcessedInput.ValidAndContinue(gameParameters);
        while (processedInput.ShouldGetInput)
        {
            _userInputOutput.WriteLine(_prompt);
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