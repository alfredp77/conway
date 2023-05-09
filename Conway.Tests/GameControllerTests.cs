using conway;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class GameControllerTests
{
    private readonly IAction _action1;
    private readonly IAction _action2;
    private readonly IUserInputOutput _userInputOutput;
    private readonly GameController _controller;

    public GameControllerTests()
    {
        _action1 = Substitute.For<IAction>();
        _action1.Id.Returns("1");
        _action1.Description.Returns("Action 1");
        
        _action2 = Substitute.For<IAction>();
        _action2.Id.Returns("2");
        _action2.Description.Returns("Action 2");
        
        _userInputOutput = Substitute.For<IUserInputOutput>();

        _controller = new GameController(_userInputOutput, new[] {_action1, _action2}, _ => true);
    }

    [Fact]
    public void Should_Take_List_Of_Actions_And_Display_Them()
    {
        _controller.Run();
        
        _userInputOutput.Received(1).WriteLine("Welcome to Conway's Game of Life!");
        _userInputOutput.Received(1).WriteLine("[1] Action 1");
        _userInputOutput.Received(1).WriteLine("[2] Action 2");
        _userInputOutput.Received(1).WriteLine("Please enter your selection");
    }
    
    [Fact]
    public void Should_Execute_Selected_Action()
    {
        _userInputOutput.ReadLine().Returns("2");
        
        _controller.Run();

        _action2.Received(1).Execute();
    }

    [Fact]
    public void Should_Terminate_When_Default_End_Condition_Is_Satisfied()
    {
        _userInputOutput.ReadLine().Returns("2");
        _action2.Execute().Returns(new GameState{ IsEnd = true });
        var controller = new GameController(_userInputOutput, new[] {_action1, _action2});   
        controller.Run();
        
        _userInputOutput.Received(1).WriteLine("Thank you for playing Conway's Game of Life!");
    }

    [Fact]
    public void Should_Not_Prompt_Anymore_After_Termination()
    {
        _userInputOutput.ReadLine().Returns("1");
        
        _controller.Run();

        _userInputOutput.Received(1).ReadLine();
        _userInputOutput.Received(1).WriteLine("Thank you for playing Conway's Game of Life!");
    }

    [Fact]
    public void Should_Keep_Displaying_Menu_And_Prompt_When_End_Condition_Not_Satisfied()
    {
        var controller = new GameController(_userInputOutput, new[] {_action1, _action2});   
        _userInputOutput.ReadLine().Returns("1", "1", "2");
        _action1.Execute().Returns(new GameState());
        _action2.Execute().Returns(new GameState{ IsEnd = true});
        
        controller.Run();
        
        _userInputOutput.Received(3).WriteLine("Welcome to Conway's Game of Life!");
        _userInputOutput.Received(3).WriteLine("[1] Action 1");
        _userInputOutput.Received(3).WriteLine("[2] Action 2");
        _userInputOutput.Received(3).WriteLine("Please enter your selection");
        _userInputOutput.Received(1).WriteLine("Thank you for playing Conway's Game of Life!");
    }
}