using Conway.Main.Actions;
using Conway.Main.Tools;

namespace Conway.Main.Game;

public static class GameStarter
{
    public static void Run(IUserInputOutput userInputOutput, GameParameters? initialParameters = null)
    {
        var controller = new GameController(userInputOutput,
            new IAction[]
            {
                new InputGridSizeAction(userInputOutput),
                new InputNumberOfGenerationAction(userInputOutput),
                new InputLiveCellAction(userInputOutput),
                new RunAction(userInputOutput, new GameRunner(), new LiveCellsPrinter((userInputOutput))),
                new QuitAction()
            });
        controller.Run(initialParameters ?? GameParameters.Initial);
    }    
}