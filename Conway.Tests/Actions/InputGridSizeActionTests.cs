using Conway.Main;
using Conway.Main.Actions;
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
        _action = new InputGridSizeAction(_userInputOutput);
    }

    [Fact]
    public void Should_Prompt_For_Grid_Size()
    {
        _action.Execute(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(InputGridSizeAction.Prompt);
    }
    
    [Fact]
    public void Should_Return_Same_GameState_When_Input_Cannot_Be_Parsed()
    {
        _userInputOutput.ReadLine().Returns("invalid input");
        
        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(GameParameters.Initial, result);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }

    [Theory]
    [InlineData("10 15", 10, 15)]
    [InlineData("7 11", 7, 11)]
    public void Should_Parse_Input_To_Grid_Size(string input, int expectedWidth, int expectedHeight)
    {
        _userInputOutput.ReadLine().Returns(input);
        
        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(expectedWidth, result.Width);
        Assert.Equal(expectedHeight, result.Height);
    }
}