﻿using Conway.Main.Tools;

namespace Conway.Main.Game;

public class GameController
{
    public const string WelcomeMessage = "Welcome to Conway's Game of Life!";
    public const string ThankYouMessage = "Thank you for playing Conway's Game of Life!";
    private readonly IUserInputOutput _userInputOutput;
    private readonly IAction[] _actions;
    private readonly Func<GameParameters, bool> _endCondition;

    public GameController(IUserInputOutput userInputOutput, IAction[] actions, Func<GameParameters, bool>? endCondition = null)
    {
        _userInputOutput = userInputOutput;
        _actions = actions;
        _endCondition = endCondition ?? (gs => gs.IsEnd);
    }

    public void Run(GameParameters initialParameters)
    {
        _userInputOutput.WriteLine(WelcomeMessage);
        var gameParameters = initialParameters;
        do
        {
            foreach (var action in _actions)
            {
                _userInputOutput.WriteLine($"[{action.Id}] {action.Description}");
            }

            _userInputOutput.WriteLine("Please enter your selection");

            var selectedActionId = _userInputOutput.ReadLine();
            if (!string.IsNullOrEmpty(selectedActionId))
            {
                var selectedAction = _actions.Single(a => string.Equals(a.Id, selectedActionId, StringComparison.InvariantCultureIgnoreCase));
                gameParameters = selectedAction.Execute(gameParameters);
            }
        } while (!_endCondition(gameParameters));
        
        _userInputOutput.WriteLine(ThankYouMessage);
    }
}