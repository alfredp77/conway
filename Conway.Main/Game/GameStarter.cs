using Conway.Main.Actions;
using Conway.Main.InputProcessors;
using Conway.Main.Tools;

namespace Conway.Main.Game;

public static class GameStarter
{
    public static void Run(IUserInputOutput userInputOutput, GameParameters? initialParameters = null)
    {
        var controller = new GameController(userInputOutput,
            new IAction[]
            {
                new MenuAction(userInputOutput, new InputGridSizeProcessor()),
                new MenuAction(userInputOutput, new InputNumberOfGenerationProcessor()),
                new MenuAction(userInputOutput, new InputLiveCellProcessor()),
                new MenuAction(userInputOutput, new RunProcessor(new GameRunner(), new LiveCellsPrinter((userInputOutput)))),
                new QuitAction()
            });
        controller.Run(initialParameters ?? GameParameters.Initial);
    }    
}