using Conway.Main;
using Conway.Main.Actions;
using Xunit;
using Xunit.Abstractions;

namespace Conway.Tests;

public class IntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void Should_Run_Sample_Scenario_Successfully()
    {
        const string mainMenu =
            @"[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
[Q] Quit
Please enter your selection";

        GameScenario.WhenGameStartsWith(_testOutputHelper)
            .ThenScreenDisplays(
                $@"{GameController.WelcomeMessage}
{mainMenu}")
            .WhenUserEnters(InputGridSizeAction.ID)
            .ThenScreenDisplays(InputGridSizeAction.Prompt)
            .WhenUserEnters("5 5")
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters(InputNumberOfGenerationAction.ID)
            .ThenScreenDisplays(InputNumberOfGenerationAction.Prompt)
            .WhenUserEnters("3")
            .ThenScreenDisplays(mainMenu)
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
            .ThenScreenDisplays(mainMenu)
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
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters(QuitAction.ID)
            .ThenScreenDisplays("Thank you for playing Conway's Game of Life!")
            .Dispose();
    }
    
    
    
}