using System.Collections.Generic;
using System.Drawing;
using Conway.Main;
using Xunit;

namespace Conway.Tests;

public class GameRunnerTests
{
    private readonly GameRunner _runner = new();
    private readonly GameParameters _testParameters = new()
    {
        NumberOfGeneration = 10,
        Width = 10,
        Height = 10,
        InitialLiveCells = new List<Point> {new(1, 1), new(2, 2)}
    };
    
    [Fact]
    public void Should_Generate_Initial_State()
    {
        var state = _runner.GenerateInitialState(_testParameters);

        Assert.Equal(_testParameters, state.Parameters);
        Assert.Equal(new List<Point> {new (1, 1), new (2, 2)}, state.LiveCells);
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
}