using System.Collections.Generic;
using System.Drawing;
using Conway.Main.Game;
using Xunit;

namespace Conway.Tests.Game;

public class GameRunnerTests
{
    private readonly GameRunner _runner = new();
    private readonly GameParameters _testParameters = new()
    {
        NumberOfGeneration = 10,
        Width = 10,
        Height = 10,
        InitialLiveCells = new List<Point> {new(1, 1), new(2, 2), new (1, 2)}
    };
    
    [Fact]
    public void Should_Generate_Initial_State()
    {
        var state = _runner.GenerateInitialState(_testParameters);

        Assert.Equal(_testParameters, state.Parameters);
        Assert.Equal(_testParameters.InitialLiveCells, state.LiveCells);
        Assert.Equal(0, state.NumberOfGenerations);
    }

    [Fact]
    public void Should_Increment_Number_Of_Generations()
    {
        var state = _runner.GenerateInitialState(_testParameters);
        var nextState = _runner.GenerateNextState(state);
        var lastState = _runner.GenerateNextState(nextState);
        
        Assert.Equal(1, nextState.NumberOfGenerations);
        Assert.Equal(2, lastState.NumberOfGenerations);
    }
    
    [Fact]
    public void Cell_With_Less_Than_Two_Neighbours_Should_Die_In_Next_State()
    {
        var state = new GameState
        {
            Parameters = _testParameters,
            LiveCells = new List<Point> { new(2, 2)}
        };

        var nextState = _runner.GenerateNextState(state);

        Assert.Empty(nextState.LiveCells);
    }
    
    [Fact]
    public void Cell_With_Two_Neighbours_Should_Survive_In_Next_State()
    {
        var state = new GameState
        {
            Parameters = _testParameters,
            LiveCells = new List<Point> { new(2, 2), new(2, 3), new(3, 2)}
        };

        var nextState = _runner.GenerateNextState(state);

        Assert.Contains(new Point(2,2), nextState.LiveCells);
        Assert.Contains(new Point(2,3), nextState.LiveCells);
        Assert.Contains(new Point(3,2), nextState.LiveCells);
    }
    
    [Fact]
    public void Cell_With_Three_Neighbours_Should_Survive_In_Next_State()
    {
        var state = new GameState
        {
            Parameters = _testParameters,
            LiveCells = new List<Point> { new(2, 2), new(2, 3), new(3, 2), new(3, 3)}
        };

        var nextState = _runner.GenerateNextState(state);

        Assert.Equal(new List<Point> { new(2, 2), new(2, 3), new(3, 2), new(3, 3)}, nextState.LiveCells);
    }
    
    [Fact]
    public void Cell_With_More_Than_Three_Neighbours_Should_Die_In_Next_State()
    {
        var state = new GameState
        {
            Parameters = _testParameters,
            LiveCells = new List<Point> { new(2, 2), new(2, 3), new(3, 2), new(3, 3), new(4, 2)}
        };

        var nextState = _runner.GenerateNextState(state);

        Assert.DoesNotContain(new Point(3,2), nextState.LiveCells);
        Assert.DoesNotContain(new Point(3,3), nextState.LiveCells);
    }
    
    [Fact]
    public void Dead_Cell_With_Three_Neighbours_Should_Become_Alive_In_Next_State()
    {
        var state = new GameState
        {
            Parameters = _testParameters,
            LiveCells = new List<Point> { new(2, 2), new(2, 3), new(3, 2)}
        };

        var nextState = _runner.GenerateNextState(state);

        Assert.Equal(new List<Point> { new(2, 2), new(2, 3), new(3, 2), new(3, 3)}, nextState.LiveCells);
    }
    
}