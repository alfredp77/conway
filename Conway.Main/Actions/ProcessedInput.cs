using Conway.Main.Game;

namespace Conway.Main.Actions;

public record ProcessedInput
{
    public static ProcessedInput ValidAndContinue(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = true, Continue = true };
    }
    
    public static ProcessedInput ValidAndExit(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = true, Continue = false };
    }

    public static ProcessedInput Invalid(GameParameters gameParameters)
    {
        return new ProcessedInput { GameParameters = gameParameters, IsValid = false, Continue = true };
    }
    
    public GameParameters GameParameters { get; init; }
    public bool Continue { get; init; }
    
    public bool IsValid { get; init; }
};