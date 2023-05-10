using System.Drawing;

namespace Conway.Main;

public class InputLiveCellAction : IAction
{
    public const string ClearAll = "*";
    public const string BackToMenu = "#";
    public const string InputLiveCellPrompt = "Please enter live cell position in x y format (example: 1 2), * to clear all the previously entered cells or # to go back to main menu:";
    private readonly IUserInputOutput _userInputOutput;

    public InputLiveCellAction(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public string Id => "3";
    public string Description => "Specify live cells";
    public GameState Execute(GameState gameState)
    {
        var liveCells = new List<Point>();
        var input = "";
        while (input != "#")
        {
            _userInputOutput.WriteLine(InputLiveCellPrompt);
            input = _userInputOutput.ReadLine();

            if (input == "*")
            {
                liveCells.Clear();
                continue;
            }
            
            var position = ParsePosition(input);
            if (position != null)
            {
                liveCells.Add(position.Value);
            }
            else
            {
                _userInputOutput.WriteLine("Invalid input. Please try again.");
            }
            
        }
        
        return gameState with { LiveCells = liveCells };    
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