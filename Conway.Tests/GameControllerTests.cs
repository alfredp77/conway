using conway;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class GameControllerTests
{
    private readonly IAction _action1;
    private readonly IAction _action2;
    private readonly IUserInputOutput _console;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _action1 = Substitute.For<IAction>();
        _action2 = Substitute.For<IAction>();
        _console = Substitute.For<IUserInputOutput>();
        
        _action1.Id.Returns("1");
        _action1.Description.Returns("Action 1");
        _action2.Id.Returns("2");
        _action2.Description.Returns("Action 2");

        _controller = new GameController(_console, new[] {_action1, _action2});
    }

    [Fact]
    public void Should_Take_List_Of_Actions_And_Display_Them()
    {
        _controller.Run();
        
        _console.Received(1).WriteLine("Welcome to Conway's Game of Life!");
        _console.Received(1).WriteLine("[1] Action 1");
        _console.Received(1).WriteLine("[2] Action 2");
        _console.Received(1).WriteLine("Please enter your selection");
    }
    
    [Fact]
    public void Should_Execute_Selected_Action()
    {
        _console.ReadLine().Returns("2");
        
        _controller.Run();

        _action2.Received(1).Execute();
    }
}