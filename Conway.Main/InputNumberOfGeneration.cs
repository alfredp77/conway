namespace Conway.Main;

public class InputNumberOfGeneration : IAction
{
    private readonly IUserInputOutput _userInputOutput;

    public InputNumberOfGeneration(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id { get; }
    public string Description { get; }
    public GameState Execute(GameState gameState)
    {
        _userInputOutput.WriteLine("Please enter number of generation (10-20):");
        var input = _userInputOutput.ReadLine();
        try
        {
            var numberOfGeneration = int.Parse(input);
            return gameState with { NumberOfGeneration = numberOfGeneration };
        }
        catch
        {
            // ignored
        }
        
        _userInputOutput.WriteLine("Invalid input. Please try again.");
        return gameState;
    }
}