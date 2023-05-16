using System.Collections.Generic;
using System.Drawing;
using Conway.Main;
using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.Tools;
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
        _gameRunner.GenerateInitialState(parameters).Returns(new GameState());
        
        _action.Execute(parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _userInputOutput.Received(1).WriteLine(RunAction.NextGenerationPrompt);
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
        _userInputOutput.Received(2).WriteLine(RunAction.NextGenerationPrompt);
    }

    [Fact]
    public void Should_Print_Next_State_When_Requested()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value, Command.Next.Value, Command.Exit.Value);
        var initialState = new GameState { LiveCells = new List<Point>()};
        var parameters = GameParameters.Initial with {NumberOfGeneration = 3};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var state1 = new GameState { LiveCells = new List<Point>(), NumberOfGenerations = 1};
        _gameRunner.GenerateNextState(initialState).Returns(state1);
        var state2 = new GameState { LiveCells = new List<Point>(), NumberOfGenerations = 2};
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
        _gameRunner.GenerateInitialState(parameters).Returns(new GameState());
        
        _action.Execute(parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _gameRunner.DidNotReceiveWithAnyArgs().GenerateNextState(Arg.Any<GameState>());
        _userInputOutput.Received(2).WriteLine(RunAction.NextGenerationPrompt);
    }

    [Fact]
    public void Should_Stop_When_State_Reaches_The_Max_Number_Of_Generation()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value, Command.Next.Value, Command.Next.Value, Command.Exit.Value);
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var state1 = new GameState { LiveCells = new List<Point>(), NumberOfGenerations = 2};
        _gameRunner.GenerateNextState(initialState).Returns(state1);

        _action.Execute(parameters);
        
        _printer.Received(1).Print("Initial position", initialState);
        _printer.Received(1).Print("Generation 2", state1);
        _userInputOutput.Received(1).WriteLine(RunAction.EndOfGenerationPrompt);
        _userInputOutput.Received(2).ReadLine();
    }
    
    
    [Fact]
    public void Should_Not_Print_EndOfGeneration_Message_When_Exit_Before_The_End()
    {
        _userInputOutput.ReadLine().Returns(Command.Next.Value, Command.Exit.Value);
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var nextState = new GameState {LiveCells = new List<Point>(), NumberOfGenerations = 1};
        _gameRunner.GenerateNextState(initialState).Returns(nextState);
        
        _action.Execute(parameters);
        
        _userInputOutput.DidNotReceive().WriteLine(RunAction.EndOfGenerationPrompt);
    }
}