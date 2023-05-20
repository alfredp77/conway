using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.Tools;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class InputGridSizeActionTests
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly InputGridSizeAction _action;

    public InputGridSizeActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _action = new InputGridSizeAction(_userInputOutput, new InputGridSizeProcessor());
    }

    [Fact]
    public void Should_Prompt_For_Grid_Size()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);
        
        _action.Execute(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(InputGridSizeAction.Prompt);
    }

    [Fact]
    public void Should_Go_Back_To_Main_Menu()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);

        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(GameParameters.Initial, result);
        _userInputOutput.Received(1).WriteLine(InputGridSizeAction.Prompt);
    }
    [Fact]
    public void Should_Return_Same_GameState_When_Input_Cannot_Be_Parsed()
    {
        _userInputOutput.ReadLine().Returns("invalid input", Command.Exit.Value);
        
        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(GameParameters.Initial, result);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }

    [Theory]
    [InlineData("10 15", 10, 15)]
    [InlineData("7 11", 7, 11)]
    public void Should_Parse_Input_To_Grid_Size(string input, int expectedWidth, int expectedHeight)
    {
        _userInputOutput.ReadLine().Returns(input, Command.Exit.Value);
        
        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(expectedWidth, result.Width);
        Assert.Equal(expectedHeight, result.Height);
    }

    [Fact]
    public void Should_Validate_Max_Grid_Size()
    {
        var parameters = new GameParameters {MaxWidth = 10, MaxHeight = 12};
        _userInputOutput.ReadLine().Returns("10 13", "11 12", "10 12", Command.Exit.Value);

        var result = _action.Execute(parameters);
        
        Assert.Equal(10, result.Width);
        Assert.Equal(12, result.Height);
        _userInputOutput.Received(2).WriteLine(CommonMessages.InvalidInputMessage);
    }
    
    [Theory]
    [InlineData("0 10")]
    [InlineData("8 0")]
    public void Grid_Dimension_Should_Be_Greater_Than_Zero(string input)
    {
        var parameters = new GameParameters {MaxWidth = 10, MaxHeight = 12};
        _userInputOutput.ReadLine().Returns(input, Command.Exit.Value);

        var result = _action.Execute(parameters);
        
        Assert.Equal(parameters, result);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }
}