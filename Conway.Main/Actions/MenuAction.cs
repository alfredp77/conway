using Conway.Main.Game;
using Conway.Main.InputProcessors;
using Conway.Main.Tools;

namespace Conway.Main.Actions;

public class MenuAction : IAction
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IInputProcessor _inputProcessor;
    private readonly string _defaultPrompt;

    public MenuAction(IUserInputOutput userInputOutput, IInputProcessor inputProcessor)
    {
        _userInputOutput = userInputOutput;
        _inputProcessor = inputProcessor;
        _defaultPrompt = GetPrompt(_inputProcessor.Prompt);
    }

    public static string GetPrompt(string inputProcessorPrompt)
    {
        return $"{inputProcessorPrompt} or {Command.Exit} to go back to main menu:";
    }

    public static string GetExitPrompt(string inputProcessorPrompt)
    {
        return $"{inputProcessorPrompt}. Press any key to go back to main menu:";
    }
    public string Id => _inputProcessor.Id;
    public string Description => _inputProcessor.Description;
    

    public GameParameters Execute(GameParameters gameParameters)
    {
        var processedInput = _inputProcessor.Initialize(gameParameters);
        while (processedInput.Continue)
        {
            var prompt = string.IsNullOrEmpty(processedInput.Prompt) ? _defaultPrompt : 
                GetPrompt(processedInput.Prompt);
            _userInputOutput.WriteLine(prompt);
            var input = _userInputOutput.ReadLine();
            if (input == Command.Exit)
            {
                break;
            }

            processedInput = _inputProcessor.ProcessInput(input, processedInput.GameParameters);
            if (!processedInput.IsValid)
            {
                _userInputOutput.WriteLine(CommonMessages.InvalidInputMessage);
            }
        }

        if (!string.IsNullOrEmpty(processedInput.Prompt))
        {
            _userInputOutput.ReadKey(GetExitPrompt(processedInput.Prompt));
        }
        return processedInput.GameParameters;
    }
}