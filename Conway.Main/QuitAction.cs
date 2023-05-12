namespace Conway.Main;

public class QuitAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;

    public QuitAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => "Q";
    public string Description => "Quit";
    public GameParameters Execute(GameParameters gameParameters)
    {
        _userInputOutput.WriteLine("Thank you for playing Conway's Game of Life!");
        return new GameParameters {IsEnd = true};
    }
}