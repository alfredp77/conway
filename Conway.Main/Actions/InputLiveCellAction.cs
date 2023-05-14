using System.Drawing;
using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class InputLiveCellAction : IAction
{
    public const string InputLiveCellPrompt = "Please enter live cell position in x y format (example: 1 2), * to clear all the previously entered cells or # to go back to main menu:";
    public const string ID = "3";
    private readonly IUserInputOutput _userInputOutput;

    public InputLiveCellAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => ID;
    public string Description => "Specify initial live cells";
    public GameParameters Execute(GameParameters gameParameters)
    {
        var liveCells = new List<Point>();
        var input = "";
        while (input != Command.Exit)
        {
            if (input == Command.ClearCells)
            {
                liveCells.Clear();
            }
            else if (input != "")
            {
                var position = ParsePosition(input);
                if (position != null)
                {
                    liveCells.Add(position.Value);
                }
                else
                {
                    _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
                }
            }
            
            _userInputOutput.WriteLine(InputLiveCellPrompt);
            input = _userInputOutput.ReadLine();
        }
        
        return gameParameters with { InitialLiveCells = liveCells };    
    }

    private Point? ParsePosition(string input)
    {
        var parts = input.Split(' ');
        if (parts.Length != 2)
        {
            return null;
        }

        if (!int.TryParse(parts[0], out var x))
        {
            return null;
        }

        if (!int.TryParse(parts[1], out var y))
        {
            return null;
        }

        return new Point(x, y);
    }
}