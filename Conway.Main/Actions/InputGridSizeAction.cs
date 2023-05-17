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
        GameParameters? newParameters = null;
        while (newParameters == null)
        {
            newParameters = GetInput(gameParameters);
        }
        return newParameters;
    }

    private GameParameters? GetInput(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine(Prompt);
        var input = _userInputOutput.ReadLine();
        if (input == Command.Exit)
        {
            return gameParameters;
        }
        
        var tokens = input.Split(' ');
        if (int.TryParse(tokens[0], out var parsedWidth) && int.TryParse(tokens[1], out var parsedHeight))
        {
            if ((gameParameters.MaxWidth == 0 || (parsedWidth <= gameParameters.MaxWidth && parsedWidth > 0)) &&
                (gameParameters.MaxHeight == 0 || (parsedHeight <= gameParameters.MaxHeight && parsedHeight > 0)))
            {
                return gameParameters with {Width = parsedWidth, Height = parsedHeight};
            }
        }
            
        _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
        return null;
    }

}