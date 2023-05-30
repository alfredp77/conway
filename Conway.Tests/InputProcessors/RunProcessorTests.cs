using System.Collections.Generic;
using System.Drawing;
using Conway.Main.Game;
using Conway.Main.InputProcessors;
using Conway.Main.Tools;
using NSubstitute;
using Xunit;

namespace Conway.Tests.InputProcessors;

public class RunProcessorTests
{
    private readonly RunProcessor _processor;
    private readonly ILiveCellsPrinter _printer;
    private readonly IGameRunner _gameRunner;
    public RunProcessorTests()
    {
        _printer = Substitute.For<ILiveCellsPrinter>();
        _gameRunner = Substitute.For<IGameRunner>();
        _processor = new RunProcessor(_gameRunner, _printer);
    }
    
    [Fact]
    public void Should_Generate_Initial_Cell_State_On_Initialize()
    {
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        
        var initialProcessedInput = _processor.Initialize(parameters);

        Assert.Equal(initialState, _processor.CurrentState);
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _printer.Received(1).Print("Initial position", initialState);
        Assert.Equal(RunProcessor.NextGenerationPrompt, initialProcessedInput.Prompt);
        Assert.True(initialProcessedInput.Continue);
        Assert.True(initialProcessedInput.IsValid);
    }
 
    [Fact]
    public void Should_Generate_Next_State_When_Requested()
    {
        var initialState = new GameState { LiveCells = new List<Point>()};
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var nextState = new GameState { LiveCells = new List<Point>(), NumberOfGenerations = 1};
        _gameRunner.GenerateNextState(initialState).Returns(nextState);

        _processor.Initialize(parameters);
        var processedInput = _processor.ProcessInput(Command.Next.Value, parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _gameRunner.Received(1).GenerateNextState(initialState);
        _printer.Received(1).Print("Generation 1", nextState);
        Assert.Equal(RunProcessor.NextGenerationPrompt, processedInput.Prompt);
        Assert.True(processedInput.IsValid);
        Assert.True(processedInput.Continue);
    }
    
    [Fact]
    public void Should_Prompt_Again_When_Invalid_Input_Is_Received()
    {
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        _gameRunner.GenerateInitialState(parameters).Returns(new GameState());
        
        _processor.Initialize(parameters);
        _printer.ClearReceivedCalls();
        var processedInput = _processor.ProcessInput("x", parameters);
        
        _gameRunner.Received(1).GenerateInitialState(parameters);
        _gameRunner.DidNotReceiveWithAnyArgs().GenerateNextState(Arg.Any<GameState>());
        _printer.DidNotReceiveWithAnyArgs().Print(Arg.Any<string>(), Arg.Any<GameState>());
        Assert.Empty(processedInput.Prompt);
        Assert.False(processedInput.IsValid);
        Assert.True(processedInput.Continue);
    }
    
    [Fact]
    public void Should_Stop_When_State_Reaches_Number_Of_Generation()
    {
        var parameters = GameParameters.Initial with {NumberOfGeneration = 2};
        var initialState = new GameState { LiveCells = new List<Point>()};
        _gameRunner.GenerateInitialState(parameters).Returns(initialState);
        var state1 = new GameState { LiveCells = new List<Point>(), NumberOfGenerations = 2};
        _gameRunner.GenerateNextState(initialState).Returns(state1);

        _processor.Initialize(parameters);
        var processedInput = _processor.ProcessInput(Command.Next.Value, parameters);
        
        _printer.Received(1).Print("Initial position", initialState);
        _printer.Received(1).Print("Generation 2", state1);
        Assert.Equal(RunProcessor.EndOfGenerationPrompt, processedInput.Prompt);
        Assert.True(processedInput.IsValid);
        Assert.False(processedInput.Continue);
        
    }
    
   
}