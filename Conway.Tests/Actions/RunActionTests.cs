using System.Collections.Generic;
using System.Drawing;
using Conway.Main;
using Conway.Main.Actions;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class RunActionTests
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly IGameRunner _gameRunner;

    public RunActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _gameRunner = Substitute.For<IGameRunner>();
    }

    [Fact]
    public void Should_Generate_Initial_Cell_State_And_Prompt()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);
        
        var action = new RunAction(_userInputOutput, _gameRunner);
        action.Execute(GameParameters.Initial);
        
        _gameRunner.Received(1).GenerateInitialState(GameParameters.Initial);
        _userInputOutput.Received(1).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Generate_Next_State_When_Requested()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value,Command.Exit.Value);
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(GameParameters.Initial).Returns(initialState);
        var nextState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(initialState).Returns(nextState);
        
        var action = new RunAction(_userInputOutput, _gameRunner);
        action.Execute(GameParameters.Initial);
        
        _gameRunner.Received(1).GenerateInitialState(GameParameters.Initial);
        _gameRunner.Received(1).GenerateNextState(initialState);
        _userInputOutput.Received(2).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Prompt_Again_When_Invalid_Input_Is_Received()
    {
        _userInputOutput.ReadLine().Returns("x",Command.Exit.Value);
        
        var action = new RunAction(_userInputOutput, _gameRunner);
        action.Execute(GameParameters.Initial);
        
        _gameRunner.Received(1).GenerateInitialState(GameParameters.Initial);
        _gameRunner.DidNotReceiveWithAnyArgs().GenerateNextState(Arg.Any<GameState>());
        _userInputOutput.Received(2).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }
}