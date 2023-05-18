using Conway.Main.Game;
using Conway.Main.Tools;

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
        
        if (int.TryParse(input, out var numberOfGeneration) && 
            numberOfGeneration <= gameParameters.MaxNumberOfGeneration &&
            numberOfGeneration >= gameParameters.MinNumberOfGeneration)
        {
            return gameParameters with {NumberOfGeneration = numberOfGeneration};
        }

        _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
        return null;
    }
}