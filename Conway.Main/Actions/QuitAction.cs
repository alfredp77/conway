namespace Conway.Main.Actions;

public class QuitAction : IAction
{
    public string Id => "Q";
    public string Description => "Quit";
    public GameParameters Execute(GameParameters gameParameters)
    {
        return new GameParameters {IsEnd = true};
    }
}