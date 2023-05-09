namespace conway;

public class GameController
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IAction[] _actions;

    public GameController(IUserInputOutput userInputOutput, IAction[] actions)
    {
        _userInputOutput = userInputOutput;
        _actions = actions;
    }

    public void Run()
    {
        _userInputOutput.WriteLine("Welcome to Conway's Game of Life!");
        foreach (var action in _actions)
        {
            _userInputOutput.WriteLine($"[{action.Id}] {action.Description}");
        }
        _userInputOutput.WriteLine("Please enter your selection");
    }
}