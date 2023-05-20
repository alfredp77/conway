using Conway.Main.Game;

namespace Conway.Main.Actions;

public class InputNumberOfGenerationProcessor : IInputProcessor
{
    public const string ID = "2";
    public const string PROMPT = "Please enter number of generation (10-20)";

    public string Id => ID;
    public string Description => "Specify number of generation";
    public string Prompt => PROMPT;
    public ProcessedInput ProcessInput(string input, GameParameters gameParameters)
    {
        if (int.TryParse(input, out var numberOfGeneration) &&
            (gameParameters.MaxNumberOfGeneration == 0 || numberOfGeneration <= gameParameters.MaxNumberOfGeneration) &&
            numberOfGeneration >= gameParameters.MinNumberOfGeneration)
        {
            return ProcessedInput.ValidAndExit(gameParameters with {NumberOfGeneration = numberOfGeneration});
        }
        
        return ProcessedInput.Invalid(gameParameters);
    }
}