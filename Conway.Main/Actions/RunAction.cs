namespace Conway.Main.Actions;

public class RunAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IGameRunner _gameRunner;
    private readonly ILiveCellsPrinter _liveCellsPrinter;

    public RunAction(IUserInputOutput userInputOutput, IGameRunner gameRunner, ILiveCellsPrinter liveCellsPrinter)
    {
        _userInputOutput = userInputOutput;
        _gameRunner = gameRunner;
        _liveCellsPrinter = liveCellsPrinter;
    }

    public string Id => "4";
    public string Description => "Run";
    public GameParameters Execute(GameParameters gameParameters)
    {
        var userInput = "";
        var gameState = _gameRunner.GenerateInitialState(gameParameters);
        _liveCellsPrinter.Print("Initial position", gameState);
        var generationCount = 0;
        while (userInput != Command.Exit && generationCount < gameParameters.NumberOfGeneration)
        {
            _userInputOutput.WriteLine("Enter > to go to next generation or # to go back to main menu");
            userInput = _userInputOutput.ReadLine();
            if (userInput != Command.Next)
            {
                continue;
            }
            gameState = _gameRunner.GenerateNextState(gameState);
            generationCount++;
            _liveCellsPrinter.Print($"Generation {generationCount}", gameState);
        }
        
        return gameParameters;
    }
}