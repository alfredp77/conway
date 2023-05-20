using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.Tools;
using Xunit;
using Xunit.Abstractions;

namespace Conway.Tests;

public class IntegrationTests
{
    private const string MainMenu = @"[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
[Q] Quit
Please enter your selection";

    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Should_Run_Sample_Scenario_Successfully()
    {
        GameScenario.WhenGameStartsWith(_testOutputHelper)
            .ThenScreenDisplays(
$@"{GameController.WelcomeMessage}
{MainMenu}"
)
            .WhenUserEnters(InputGridSizeProcessor.ID)
            .ThenScreenDisplays(MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT))
            .WhenUserEnters("5 5")
            .ThenScreenDisplays(MainMenu)
            .WhenUserEnters(InputNumberOfGenerationProcessor.ID)
            .ThenScreenDisplays(MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT))
            .WhenUserEnters("3")
            .ThenScreenDisplays(MainMenu)
            .WhenUserEnters(InputLiveCellAction.ID)
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("2 4")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 4")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("3 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("4 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("3 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("4 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters(Command.Exit.Value)
            .ThenScreenDisplays(MainMenu)
            .WhenUserEnters(RunAction.ID)
            .ThenScreenDisplays(
                $@"Initial position
. . . . .
. . o o o
. . o o o
. o . . o
. . . . .
{RunAction.NextGenerationPrompt}")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                $@"Generation 1
. . . o .
. . o . o
. o . . .
. . o . o
. . . . .
{RunAction.NextGenerationPrompt}")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                $@"Generation 2
. . . o .
. . o o .
. o o . .
. . . . .
. . . . .
{RunAction.NextGenerationPrompt}")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                $@"Generation 3
. . o o .
. o . o .
. o o o .
. . . . .
. . . . .
{RunAction.EndOfGenerationPrompt}")
            .WhenUserEnters(Command.Exit.Value)
            .ThenScreenDisplays(MainMenu)
            .WhenUserEnters(QuitAction.ID)
            .ThenScreenDisplays("Thank you for playing Conway's Game of Life!")
            .Dispose();
    }

    [Fact]
    public void Should_Validate_Grid_Size()
    {
        var initialParameters = new GameParameters {MaxWidth = 10, MaxHeight = 15};
        GameScenario.WhenGameStartsWith(_testOutputHelper, initialParameters)
            .ThenScreenDisplays(
$@"{GameController.WelcomeMessage}
{MainMenu}"
            )
            .WhenUserEnters(InputGridSizeProcessor.ID)
            .ThenScreenDisplays(MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT))
            .WhenUserEnters("11 5")
            .ThenScreenDisplays(
$@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT)}"
            )
            .WhenUserEnters("7 16")
            .ThenScreenDisplays(
$@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT)}"
            )
            .WhenUserEnters("0 9")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT)}"
            )
            .WhenUserEnters("7 0")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputGridSizeProcessor.PROMPT)}"
            )
            .WhenUserEnters("7 5")
            .ThenScreenDisplays(MainMenu)
            .Dispose();
            
    }

    [Fact]
    public void Should_Validate_Number_Of_Generations()
    {
        var initialParameters = new GameParameters
        {
            MinNumberOfGeneration = 6,
            MaxNumberOfGeneration = 10
        };
        GameScenario.WhenGameStartsWith(_testOutputHelper, initialParameters)
            .ThenScreenDisplays(
                $@"{GameController.WelcomeMessage}
{MainMenu}"
            )
            .WhenUserEnters(InputNumberOfGenerationProcessor.ID)
            .ThenScreenDisplays(MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT))
            .WhenUserEnters("xyz")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT)}"
            )
            .WhenUserEnters("  ")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT)}"
            )
            .WhenUserEnters("11")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT)}"
            )
            .WhenUserEnters("5")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{MenuAction.GetPrompt(InputNumberOfGenerationProcessor.PROMPT)}"
            )
            .WhenUserEnters("10")
            .ThenScreenDisplays(MainMenu)
            .Dispose();
    }
}