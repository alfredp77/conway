namespace Conway.Main.Actions;

public class InputGridSizeAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;

    public InputGridSizeAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => "1";
    public string Description => "Specify grid size";
    public GameParameters Execute(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine("Please enter grid size in w h format (example: 10 15):");
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

        _userInputOutput.WriteLine("Invalid input. Please try again.");
        return gameParameters;
    }
}