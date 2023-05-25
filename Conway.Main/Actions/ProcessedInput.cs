using Conway.Main.Game;

namespace Conway.Main.Actions;

public record ProcessedInput
{
    public static ProcessedInput ValidAndContinue(GameParameters gameParameters,
        string prompt = "")
    {
        return new ProcessedInput
        {
            GameParameters = gameParameters, IsValid = true, Continue = true,
            Prompt = prompt
        };
    }
    
    public static ProcessedInput ValidAndExit(GameParameters gameParameters,
        string prompt = "")
    {
        return new ProcessedInput 
        { 
            GameParameters = gameParameters, 
            IsValid = true, Continue = false,
            Prompt = prompt
        };
    }

    public static ProcessedInput Invalid(GameParameters gameParameters,
        string prompt = "")
    {
        return new ProcessedInput
        {
            GameParameters = gameParameters, IsValid = false, Continue = true,
            Prompt = prompt
        };
    }
    
    public GameParameters GameParameters { get; init; }
    public bool Continue { get; init; }
    
    public bool IsValid { get; init; }
    public string Prompt { get; init; }
};