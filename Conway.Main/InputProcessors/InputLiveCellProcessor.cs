using System.Drawing;
using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.Tools;

namespace Conway.Main.InputProcessors;

public class InputLiveCellProcessor : IInputProcessor
{
    public const string ID = "3";
    public const string PROMPT = "Please enter live cell position in x y format (example: 1 2), * to clear all the previously entered cells";

    public string Id => ID;
    public string Description => "Specify initial live cells";
    public string Prompt => PROMPT;
    public ProcessedInput ProcessInput(string input, GameParameters gameParameters)
    {
        if (input == Command.ClearCells)
        {
            return ProcessedInput.ValidAndContinue(gameParameters with {InitialLiveCells = new List<Point>()});
        }
        
        if (input != "")
        {
            var position = ParsePosition(input);
            if (position != null)
            {
                return ProcessedInput.ValidAndContinue(gameParameters with {InitialLiveCells = gameParameters.InitialLiveCells.Union(new[]{position.Value}).ToList()});
            }
        }

        return ProcessedInput.Invalid(gameParameters);
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