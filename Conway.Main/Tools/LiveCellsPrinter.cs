using System.Drawing;
using System.Text;
using Conway.Main.Game;

namespace Conway.Main.Tools;

public class LiveCellsPrinter : ILiveCellsPrinter
{
    private readonly IUserInputOutput _userInputOutput;

    public LiveCellsPrinter(IUserInputOutput userInputOutput)
    {
        _userInputOutput = userInputOutput;
    }

    public void Print(string prompt, GameState gameState)
    {
        var builder = new StringBuilder();
        builder.AppendLine();
        builder.AppendLine(prompt);
        for (var y = 1; y <= gameState.Parameters.Height; y++)
        {
            for (var x = 1; x <= gameState.Parameters.Width; x++)
            {
                if (x > 1)
                {
                    builder.Append(' ');
                }
                builder.Append(gameState.LiveCells.Contains(new Point(x, y)) ? 'o' : '.');
            }

            builder.AppendLine();
        }
        _userInputOutput.WriteLine(builder.ToString().Trim());
    }
}