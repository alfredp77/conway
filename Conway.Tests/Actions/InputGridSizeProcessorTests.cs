using Conway.Main.Actions;
using Conway.Main.Game;
using Xunit;

namespace Conway.Tests.Actions;

public class InputGridSizeProcessorTests
{
    private readonly InputGridSizeProcessor _processor;

    public InputGridSizeProcessorTests()
    {
        _processor = new InputGridSizeProcessor();
    }
    
    [Fact]
    public void Should_Return_Same_GameState_When_Input_Cannot_Be_Parsed()
    {
        var result = _processor.ProcessInput("invalid input", GameParameters.Initial);
        
        Assert.Equal(GameParameters.Initial, result.GameParameters);
        Assert.False(result.IsValid);
        Assert.True(result.ShouldGetInput);
    }
    
    [Theory]
    [InlineData("10 15", 10, 15)]
    [InlineData("7 11", 7, 11)]
    public void Should_Parse_Input_To_Grid_Size(string input, int expectedWidth, int expectedHeight)
    {
        var result = _processor.ProcessInput(input, GameParameters.Initial);
        
        Assert.Equal(expectedWidth, result.GameParameters.Width);
        Assert.Equal(expectedHeight, result.GameParameters.Height);
        Assert.True(result.IsValid);
        Assert.False(result.ShouldGetInput);
    }
    
    [Theory]
    [InlineData("10 13")]
    [InlineData("11 12")]
    [InlineData("0 10")]
    [InlineData("8 0")]
    public void Should_Validate_Grid_Size(string input)
    {
        var parameters = new GameParameters {MaxWidth = 10, MaxHeight = 12};

        var result = _processor.ProcessInput(input, parameters);
        
        Assert.Equal(parameters, result.GameParameters);
        Assert.False(result.IsValid);
        Assert.True(result.ShouldGetInput);
    }
}