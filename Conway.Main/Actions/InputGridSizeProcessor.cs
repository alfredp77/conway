using Conway.Main.Game;

namespace Conway.Main.Actions;

public class InputGridSizeProcessor : IInputProcessor
{
    public const string ID = "1";
    public const string PROMPT = "Please enter grid size in w h format (example: 10 15)";
    
    public string Id => ID;
    public string Description => "Specify grid size";
    public string Prompt => PROMPT;
    public ProcessedInput ProcessInput(string input, GameParameters gameParameters)
    {
        var tokens = input.Split(' ');
        if (int.TryParse(tokens[0], out var parsedWidth) && int.TryParse(tokens[1], out var parsedHeight))
        {
            if ((gameParameters.MaxWidth == 0 || (parsedWidth <= gameParameters.MaxWidth && parsedWidth > 0)) &&
                (gameParameters.MaxHeight == 0 || (parsedHeight <= gameParameters.MaxHeight && parsedHeight > 0)))
            {
                return ProcessedInput.ValidAndExit(gameParameters with {Width = parsedWidth, Height = parsedHeight});
            }
        }
        
        return ProcessedInput.Invalid(gameParameters);
    }
}