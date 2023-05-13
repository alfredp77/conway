using Conway.Main.Actions;

namespace Conway.Main;

public static class GameStarter
{
    public static void Run(IUserInputOutput userInputOutput)
    {
        var controller = new GameController(userInputOutput,
            new IAction[]
            {
                new InputGridSizeAction(userInputOutput),
                new InputNumberOfGenerationAction(userInputOutput),
                new InputLiveCellAction(userInputOutput),
                new RunAction(userInputOutput, new GameRunner(), new LiveCellsPrinter((userInputOutput))),
                new QuitAction(userInputOutput)
            });
        controller.Run();
    }    
}