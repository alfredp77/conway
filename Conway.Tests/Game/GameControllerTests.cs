using Conway.Main.Game;
using Conway.Main.Tools;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Game;

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

        _controller = new GameController(_userInputOutput, new[] {_action1, _action2});
    }

    [Fact]
    public void Should_Take_List_Of_Actions_And_Display_Them()
    {
        var controller = new GameController(_userInputOutput, new[] {_action1, _action2}, _ => true);
        controller.Run(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(GameController.WelcomeMessage);
        _userInputOutput.Received(1).WriteLine("[1] Action 1");
        _userInputOutput.Received(1).WriteLine("[2] Action 2");
        _userInputOutput.Received(1).WriteLine("Please enter your selection");
    }
    
    [Fact]
    public void Should_Execute_Selected_Action()
    {
        var controller = new GameController(_userInputOutput, new[] {_action1, _action2}, _ => true);
        _userInputOutput.ReadLine().Returns("2");
        
        controller.Run(GameParameters.Initial);

        _action2.Received(1).Execute(GameParameters.Initial);
    }
    
    [Fact]
    public void Should_Terminate_When_Default_End_Condition_Is_Satisfied()
    {
        _userInputOutput.ReadLine().Returns("2");
        _action2.Execute(GameParameters.Initial).Returns(new GameParameters{ IsEnd = true });

        _controller.Run(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(GameController.ThankYouMessage);
    }

    [Fact]
    public void Should_Not_Prompt_Anymore_After_Termination()
    {
        var controller = new GameController(_userInputOutput, new[] {_action1, _action2}, _ => true);
        _userInputOutput.ReadLine().Returns("1");
        
        controller.Run(GameParameters.Initial);

        _userInputOutput.Received(1).ReadLine();
        _userInputOutput.Received(1).WriteLine(GameController.ThankYouMessage);
    }

    [Fact]
    public void Should_Keep_Displaying_Menu_And_Prompt_When_End_Condition_Not_Satisfied()
    {
        _userInputOutput.ReadLine().Returns("1", "1", "2");
        _action1.Execute(Arg.Any<GameParameters>()).Returns(new GameParameters());
        _action2.Execute(Arg.Any<GameParameters>()).Returns(new GameParameters{ IsEnd = true});
        
        _controller.Run(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(GameController.WelcomeMessage);
        _userInputOutput.Received(3).WriteLine("[1] Action 1");
        _userInputOutput.Received(3).WriteLine("[2] Action 2");
        _userInputOutput.Received(3).WriteLine("Please enter your selection");
        _userInputOutput.Received(1).WriteLine(GameController.ThankYouMessage);
    }
    
    [Fact]
    public void Should_Pass_GameState_To_The_Next_Action()
    {
        _userInputOutput.ReadLine().Returns("1",  "2");
        var stateFromAction1 = new GameParameters();
        _action1.Execute(GameParameters.Initial).Returns(stateFromAction1);
        _action2.Execute(stateFromAction1).Returns(new GameParameters{ IsEnd = true});
        
        _controller.Run(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(GameController.ThankYouMessage);
    }

    [Fact]
    public void Command_Should_Be_Case_Insensitive()
    {
        _action2.Id.Returns("x");
        _userInputOutput.ReadLine().Returns("X");
        _action2.Execute(GameParameters.Initial).Returns(new GameParameters{ IsEnd = true });

        _controller.Run(GameParameters.Initial);
        
        _action2.Received(1).Execute(GameParameters.Initial);
        _userInputOutput.Received(1).WriteLine(GameController.ThankYouMessage);
    }
}