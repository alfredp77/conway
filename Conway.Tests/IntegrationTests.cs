using Conway.Main;
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
            .WhenUserEnters(InputGridSizeAction.ID)
            .ThenScreenDisplays(InputGridSizeAction.Prompt)
            .WhenUserEnters("5 5")
            .ThenScreenDisplays(MainMenu)
            .WhenUserEnters(InputNumberOfGenerationAction.ID)
            .ThenScreenDisplays(InputNumberOfGenerationAction.Prompt)
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
            .WhenUserEnters(InputGridSizeAction.ID)
            .ThenScreenDisplays(InputGridSizeAction.Prompt)
            .WhenUserEnters("11 5")
            .ThenScreenDisplays(
$@"{CommonMessages.InvalidInputMessage}
{InputGridSizeAction.Prompt}"
            )
            .WhenUserEnters("7 16")
            .ThenScreenDisplays(
$@"{CommonMessages.InvalidInputMessage}
{InputGridSizeAction.Prompt}"
            )
            .WhenUserEnters("0 9")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputGridSizeAction.Prompt}"
            )
            .WhenUserEnters("7 0")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputGridSizeAction.Prompt}"
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
            .WhenUserEnters(InputNumberOfGenerationAction.ID)
            .ThenScreenDisplays(InputNumberOfGenerationAction.Prompt)
            .WhenUserEnters("xyz")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputNumberOfGenerationAction.Prompt}"
            )
            .WhenUserEnters("  ")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputNumberOfGenerationAction.Prompt}"
            )
            .WhenUserEnters("11")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputNumberOfGenerationAction.Prompt}"
            )
            .WhenUserEnters("5")
            .ThenScreenDisplays(
                $@"{CommonMessages.InvalidInputMessage}
{InputNumberOfGenerationAction.Prompt}"
            )
            .WhenUserEnters("10")
            .ThenScreenDisplays(MainMenu)
            .Dispose();
    }
}