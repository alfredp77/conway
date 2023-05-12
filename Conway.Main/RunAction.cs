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
    public GameParameters Execute(GameParameters gameParameters)
    {
        var userInput = "";
        var gameState = _gameRunner.GenerateInitialState(gameParameters);
        while (userInput != "#")
        {
            _userInputOutput.WriteLine("Enter > to go to next generation or # to go back to main menu");
            userInput = _userInputOutput.ReadLine();
            gameState = _gameRunner.GenerateNextState(gameState);
        }
        
        return gameParameters;
    }
}