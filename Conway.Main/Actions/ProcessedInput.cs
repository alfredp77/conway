using Conway.Main.Game;

namespace Conway.Main.Actions;

public record ProcessedInput
{
    public static ProcessedInput ValidAndContinue(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = true, ShouldGetInput = true };
    }
    
    public static ProcessedInput ValidAndExit(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = true, ShouldGetInput = false };
    }

    public static ProcessedInput Invalid(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = false, ShouldGetInput = true };
    }
    
    public GameParameters GameParameters { get; init; }
    public bool ShouldGetInput { get; init; }
    
    public bool IsValid { get; init; }
};