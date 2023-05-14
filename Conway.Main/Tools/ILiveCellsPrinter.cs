using Conway.Main.Game;

namespace Conway.Main.Tools;

public interface ILiveCellsPrinter
{
    void Print(string prompt, GameState gameState);
}