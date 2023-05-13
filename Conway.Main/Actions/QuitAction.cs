namespace Conway.Main.Actions;

public class QuitAction : IAction
{
    public const string ID = "Q";

    public string Id => ID;
    public string Description => "Quit";
    public GameParameters Execute(GameParameters gameParameters)
    {
        return new GameParameters {IsEnd = true};
    }
}