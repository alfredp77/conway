namespace Conway.Main.Actions;

public class InputGridSizeAction : IAction
{
    public const string ID = "1";
    public const string Prompt = "Please enter grid size in w h format (example: 10 15):";
    private readonly IUserInputOutput _userInputOutput;

    public InputGridSizeAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => ID;
    public string Description => "Specify grid size";
    public GameParameters Execute(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine(Prompt);
        var input = _userInputOutput.ReadLine();
        try
        {
            var tokens = input.Split(' ');
            if (int.TryParse(tokens[0], out var width) && int.TryParse(tokens[1], out var height))
            {
                return gameParameters with { Width = width, Height = height };
            }
        }
        catch
        {
            // ignored
        }

        _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
        return gameParameters;
    }
}