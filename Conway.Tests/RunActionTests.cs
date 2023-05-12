using System.Collections.Generic;
using System.Drawing;
using Conway.Main;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

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
        _userInputOutput.ReadLine().Returns("#");
        
        var action = new RunAction(_userInputOutput, _gameRunner);
        action.Execute(GameState.NoLiveCells);
        
        _gameRunner.Received(1).Run(GameState.NoLiveCells);
        _userInputOutput.Received(1).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Generate_Next_State_When_Requested()
    {
        _userInputOutput.ReadLine().Returns(">","#");
        var initial = new GameState { CurrentLiveCells = new List<Point>()};
        _gameRunner.Run(GameState.NoLiveCells).Returns(initial);
        var next = new GameState { CurrentLiveCells = new List<Point>()};
        _gameRunner.Run(initial).Returns(next);
        
        var action = new RunAction(_userInputOutput, _gameRunner);
        action.Execute(GameState.NoLiveCells);
        
        _gameRunner.Received(1).Run(GameState.NoLiveCells);
        _gameRunner.Received(1).Run(initial);
        _userInputOutput.Received(2).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }
}