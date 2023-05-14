namespace Conway.Main.Actions;

public class InputNumberOfGenerationAction : IAction
{
    public const string ID = "2";
    public const string Prompt = "Please enter number of generation (10-20):";
    private readonly IUserInputOutput _userInputOutput;

    public InputNumberOfGenerationAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => ID;
    public string Description => "Specify number of generation";
    public GameParameters Execute(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine(Prompt);
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
        
        _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
        return gameParameters;
    }
}