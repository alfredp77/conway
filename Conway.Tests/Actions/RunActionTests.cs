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
    private readonly ILiveCellsPrinter _printer;
    private readonly IGameRunner _gameRunner;
    private readonly RunAction _action;

    public RunActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _printer = Substitute.For<ILiveCellsPrinter>();
        _gameRunner = Substitute.For<IGameRunner>();
        _action = new RunAction(_userInputOutput, _gameRunner, _printer);
    }

    [Fact]
    public void Should_Generate_Initial_Cell_State_And_Prompt()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        
        _action.Execute(parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _userInputOutput.Received(1).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Print_Initial_State()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(GameParameters.Initial).Returns(initialState);
        
        _action.Execute(GameParameters.Initial);
        
        _printer.Received(1).Print("Initial position", initialState);
    }
    
    [Fact]
    public void Should_Generate_Next_State_When_Requested()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value,Command.Exit.Value);
        var initialState = new GameState { LiveCells = new List<Point>()};
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var nextState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(initialState).Returns(nextState);
        
        _action.Execute(parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _gameRunner.Received(1).GenerateNextState(initialState);
        _userInputOutput.Received(2).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Print_Next_State_When_Requested()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value, Command.Next.Value, Command.Exit.Value);
        var initialState = new GameState { LiveCells = new List<Point>()};
        var parameters = GameParameters.Initial with {NumberOfGeneration = 3};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var state1 = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(initialState).Returns(state1);
        var state2 = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(state1).Returns(state2);
        
        _action.Execute(parameters);
        
        _printer.Received(1).Print("Initial position", initialState);
        _printer.Received(1).Print("Generation 1", state1);
        _printer.Received(1).Print("Generation 2", state2);
    }
    
    [Fact]
    public void Should_Prompt_Again_When_Invalid_Input_Is_Received()
    {
        _userInputOutput.ReadLine().Returns("x",Command.Exit.Value);
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        _action.Execute(parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _gameRunner.DidNotReceiveWithAnyArgs().GenerateNextState(Arg.Any<GameState>());
        _userInputOutput.Received(2).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }

    [Fact]
    public void Should_Stop_When_Number_Of_Generation_Is_Reached()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value, Command.Next.Value, Command.Next.Value, Command.Exit.Value);
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var state1 = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(initialState).Returns(state1);
        var state2 = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateNextState(state1).Returns(state2);

        _action.Execute(parameters);
        
        _printer.Received(1).Print("Initial position", initialState);
        _printer.Received(1).Print("Generation 1", state1);
        _printer.Received(1).Print("Generation 2", state2);
        _printer.DidNotReceive().Print("Generation 3", Arg.Any<GameState>());
        _userInputOutput.Received(2).ReadLine();
    }
}