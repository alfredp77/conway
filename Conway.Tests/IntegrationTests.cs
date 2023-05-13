using Conway.Main;
using Conway.Main.Actions;
using NSubstitute;
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
            @"Welcome to Conway's Game of Life!
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
[Q] Quit
Please enter your selection";

        GameScenario.WhenGameStartsWith(_testOutputHelper)
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("1")
            .ThenScreenDisplays(
@"Please enter grid size in w h format (example: 10 15):"
            )

            .WhenUserEnters("5 5")
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("2")
            .ThenScreenDisplays(
@"Please enter number of generation (10-20):")
            .WhenUserEnters("3")
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("3")
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
            .WhenUserEnters("4")
            .ThenScreenDisplays(
                @"Initial position
. . . . .
. . o o o
. . o o o
. o . . o
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 1
. . . o .
. . o . o
. o . . .
. . o . o
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 2
. . . o .
. . o o .
. o o . .
. . . . .
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 3
. . o o .
. o . o .
. o o o .
. . . . .
. . . . .
End of generation. Press any key to return to main menu")
            .WhenUserEnters(Command.Exit.Value)
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("Q")
            .ThenScreenDisplays("Thank you for playing Conway's Game of Life!")
            .Dispose();
    }
    
}