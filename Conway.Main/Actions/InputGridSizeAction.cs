using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class InputGridSizeAction : IAction
{
    public const string ID = "1";
    public const string Prompt = "Please enter grid size in w h format (example: 10 15) or # to go back to main menu:";
    private readonly IUserInputOutput _userInputOutput;

    public InputGridSizeAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => ID;
    public string Description => "Specify grid size";
    public GameParameters Execute(GameParameters gameParameters)
    {
        var processedInput = new ProcessedInput(gameParameters, true);
        while (processedInput.ShouldGetInput)
        {
            _userInputOutput.WriteLine(Prompt);
            var input = _userInputOutput.ReadLine();
            if (input == Command.Exit)
            {
                break;
            }

            processedInput = ProcessInput(input, processedInput.GameParameters);
            if (!processedInput.IsValid)
            {
                _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
            }
        }

        return processedInput.GameParameters;
    }

    private ProcessedInput ProcessInput(string input, GameParameters gameParameters)
    {
        var tokens = input.Split(' ');
        if (int.TryParse(tokens[0], out var parsedWidth) && int.TryParse(tokens[1], out var parsedHeight))
        {
            if ((gameParameters.MaxWidth == 0 || (parsedWidth <= gameParameters.MaxWidth && parsedWidth > 0)) &&
                (gameParameters.MaxHeight == 0 || (parsedHeight <= gameParameters.MaxHeight && parsedHeight > 0)))
            {
                return new ProcessedInput(gameParameters with {Width = parsedWidth, Height = parsedHeight}, false, true);
            }
        }
        
        return new ProcessedInput(gameParameters, true, false);
    }
}