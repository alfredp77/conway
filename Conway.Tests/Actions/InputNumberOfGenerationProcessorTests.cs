using Conway.Main.Actions;
using Conway.Main.Game;
using Xunit;

namespace Conway.Tests.Actions;

public class InputNumberOfGenerationProcessorTests
{
    private readonly InputNumberOfGenerationProcessor _processor;
    private readonly GameParameters _initialParameters = GameParameters.Initial with
    {
        MinNumberOfGeneration = 10,
        MaxNumberOfGeneration = 20
    };
    
    public InputNumberOfGenerationProcessorTests()
    {
        _processor = new InputNumberOfGenerationProcessor();
    }
    
    [Theory]
    [InlineData("10", 10)]
    [InlineData("20", 20)]
    public void Should_Parse_Input_To_Number_Of_Generation(string input, int expectedNumberOfGeneration)
    {
        var result = _processor.ProcessInput(input, _initialParameters);
        
        Assert.Equal(expectedNumberOfGeneration, result.GameParameters.NumberOfGeneration);
        Assert.True(result.IsValid);
        Assert.False(result.Continue);
    }
    
    [Theory]
    [InlineData("21")]
    [InlineData("8")]
    public void Should_Validate_Number_Of_Generation(string input)
    {
        var result = _processor.ProcessInput(input, _initialParameters);

        Assert.False(result.IsValid);
        Assert.True(result.Continue);
    }
    
    
    [Fact]
    public void Should_Not_Validate_Max_Number_When_It_Is_Zero()
    {
        var parameters = GameParameters.Initial with
        {
            MaxNumberOfGeneration = 0,
            MinNumberOfGeneration = 6
        };
        
        var result = _processor.ProcessInput("8", parameters);

        Assert.Equal(8, result.GameParameters.NumberOfGeneration);
        Assert.True(result.IsValid);
        Assert.False(result.Continue);
    }
}