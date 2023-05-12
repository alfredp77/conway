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
        var userInput = "";
        GameState liveState = gameState;
        while (userInput != "#")
        {
            liveState = _gameRunner.Run(liveState);
            _userInputOutput.WriteLine("Enter > to go to next generation or # to go back to main menu");
            userInput = _userInputOutput.ReadLine();
        }
        
        return gameState;
    }
}