namespace Conway.Main;

public class InputGridSizeAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;

    public InputGridSizeAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id { get; }
    public string Description { get; }
    public GameState Execute(GameState gameState)
    {
        _userInputOutput.WriteLine("Please enter grid size in w h format (example: 10 15):");
        var input = _userInputOutput.ReadLine();
        try
        {
            var tokens = input.Split(' ');
            if (int.TryParse(tokens[0], out var width) && int.TryParse(tokens[1], out var height))
            {
                return gameState with { Width = width, Height = height };
            }
        }
        catch (Exception e)
        {
            // ignored
        }

        _userInputOutput.WriteLine("Invalid input. Please try again.");
        return gameState;
    }
}