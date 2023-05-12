namespace Conway.Main;

public class InputNumberOfGenerationAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;

    public InputNumberOfGenerationAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => "2";
    public string Description => "Specify number of generation";
    public GameParameters Execute(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine("Please enter number of generation (10-20):");
        var input = _userInputOutput.ReadLine();
        try
        {
            var numberOfGeneration = int.Parse(input);
            return gameParameters with { NumberOfGeneration = numberOfGeneration };
        }
        catch
        {
            // ignored
        }
        
        _userInputOutput.WriteLine("Invalid input. Please try again.");
        return gameParameters;
    }
}