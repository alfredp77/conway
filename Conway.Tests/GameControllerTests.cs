using conway;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class GameControllerTests
{
    [Fact]
    public void Should_Take_List_Of_Actions_And_Display_Them()
    {
        var console = Substitute.For<IUserInputOutput>();
        var action1 = Substitute.For<IAction>();
        action1.Id.Returns("1");
        action1.Description.Returns("Action 1");
        var action2 = Substitute.For<IAction>();
        action2.Id.Returns("2");
        action2.Description.Returns("Action 2");
        
        var controller = new GameController(console, new[] {action1, action2});

        controller.Run();
        
        console.Received(1).WriteLine("Welcome to Conway's Game of Life!");
        console.Received(1).WriteLine("[1] Action 1");
        console.Received(1).WriteLine("[2] Action 2");
        console.Received(1).WriteLine("Please enter your selection");
    }
}