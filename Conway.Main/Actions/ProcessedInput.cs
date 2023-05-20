using Conway.Main.Game;

namespace Conway.Main.Actions;

public record ProcessedInput(GameParameters GameParameters, bool ShouldGetInput, bool IsValid=true);