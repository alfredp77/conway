using Conway.Main;
using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.Tools;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class InputNumberOfGenerationActionTests
{
    private readonly IUserInputOutput _userInputOutput;
    private readonly InputNumberOfGenerationAction _action;

    public InputNumberOfGenerationActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _action = new InputNumberOfGenerationAction(_userInputOutput);
    }

    [Fact]
    public void Should_Prompt_For_Number_Of_Generation()
    {
        _action.Execute(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine("Please enter number of generation (10-20):");
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
    [InlineData("10", 10)]
    [InlineData("20", 20)]
    public void Should_Parse_Input_To_Number_Of_Generation(string input, int expectedNumberOfGeneration)
    {
        _userInputOutput.ReadLine().Returns(input);
        
        var result = _action.Execute(GameParameters.Initial);
        
        Assert.Equal(expectedNumberOfGeneration, result.NumberOfGeneration);
    }
}