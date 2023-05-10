using Conway.Main;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class InputLiveCellActionTests
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly InputLiveCellAction _action;

    public InputLiveCellActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _action = new InputLiveCellAction(_userInputOutput);
    }

    [Fact]
    public void Should_Prompt_For_Live_Cell()
    {
        _userInputOutput.ReadLine().Returns(InputLiveCellAction.BackToMenu);
        
        _action.Execute(GameState.Initial);
        
        _userInputOutput.Received(1).WriteLine(InputLiveCellAction.InputLiveCellPrompt);
    }
    
    [Fact]
    public void Should_Return_GameState_With_Live_Cell_Positions()
    {
        _userInputOutput.ReadLine()
            .Returns("1 2", "3 10", 
                InputLiveCellAction.BackToMenu);
        
        var result = _action.Execute(GameState.Initial);
        
        _userInputOutput.Received(3).WriteLine(InputLiveCellAction.InputLiveCellPrompt);
        Assert.Equal(2, result.LiveCells.Count);
        Assert.Equal(1, result.LiveCells[0].X);
        Assert.Equal(2, result.LiveCells[0].Y);
        Assert.Equal(3, result.LiveCells[1].X);
        Assert.Equal(10, result.LiveCells[1].Y);
    }
    
    [Fact]
    public void Should_Return_GameState_With_Empty_Live_Cell_Positions_When_Input_Is_Star()
    {
        _userInputOutput.ReadLine()
            .Returns("1 2", "3 10", 
                InputLiveCellAction.ClearAll, 
                "5 7", 
                InputLiveCellAction.BackToMenu);
        
        var result = _action.Execute(GameState.Initial);
        
        _userInputOutput.Received(5).WriteLine(InputLiveCellAction.InputLiveCellPrompt);
        Assert.Single(result.LiveCells);
        Assert.Equal(5, result.LiveCells[0].X);
        Assert.Equal(7, result.LiveCells[0].Y);
    }
    
    [Fact]
    public void Should_Prompt_For_Invalid_Input()
    {
        _userInputOutput.ReadLine()
            .Returns("1 2", "invalid input", 
                InputLiveCellAction.BackToMenu);
        
        var result = _action.Execute(GameState.Initial);
        
        _userInputOutput.Received(3).WriteLine(InputLiveCellAction.InputLiveCellPrompt);
        Assert.Single(result.LiveCells);
        Assert.Equal(1, result.LiveCells[0].X);
        Assert.Equal(2, result.LiveCells[0].Y);
    }
}