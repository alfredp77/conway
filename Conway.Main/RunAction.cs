namespace Conway.Main;

public class RunAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IGameRunner _gameRunner;

    public RunAction(IUserInputOutput userInputOutput, IGameRunner gameRunner)
    {
        _userInputOutput = userInputOutput;
        _gameRunner = gameRunner;
    }

    public string Id => "4";
    public string Description => "Run";
    public GameState Execute(GameState gameState)
    {
        _gameRunner.Run(gameState);
        _userInputOutput.WriteLine("Enter > to go to next generation or # to go back to main menu");
        return gameState;
    }
}