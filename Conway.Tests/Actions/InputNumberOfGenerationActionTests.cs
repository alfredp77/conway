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
    private readonly GameParameters _initialParameters = GameParameters.Initial with
    {
        MinNumberOfGeneration = 10,
        MaxNumberOfGeneration = 20
    };
    public InputNumberOfGenerationActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _action = new InputNumberOfGenerationAction(_userInputOutput);
    }

    [Fact]
    public void Should_Prompt_For_Number_Of_Generation()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);
        
        _action.Execute(_initialParameters);
        
        _userInputOutput.Received(1).WriteLine("Please enter number of generation (10-20):");
    }
    
    [Fact]
    public void Should_Return_Same_GameState_When_Input_Cannot_Be_Parsed()
    {
        _userInputOutput.ReadLine().Returns("invalid input", Command.Exit.Value);
        
        var result = _action.Execute(_initialParameters);
        
        Assert.Equal(_initialParameters, result);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }
    
    [Theory]
    [InlineData("10", 10)]
    [InlineData("20", 20)]
    public void Should_Parse_Input_To_Number_Of_Generation(string input, int expectedNumberOfGeneration)
    {
        _userInputOutput.ReadLine().Returns(input);
        
        var result = _action.Execute(_initialParameters);
        
        Assert.Equal(expectedNumberOfGeneration, result.NumberOfGeneration);
    }
    
    [Fact]
    public void Should_Validate_Max_Number_Of_Generation()
    {
        _userInputOutput.ReadLine().Returns("21", "20", Command.Exit.Value);
        
        var result = _action.Execute(_initialParameters);
        
        Assert.Equal(20, result.NumberOfGeneration);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }

    [Fact]
    public void Should_Validate_Min_Number_Of_Generation()
    {
        _userInputOutput.ReadLine().Returns("8", "10", Command.Exit.Value);
        
        var result = _action.Execute(_initialParameters);
        
        Assert.Equal(10, result.NumberOfGeneration);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
    }

    [Fact]
    public void Should_Not_Validate_Max_Number_When_It_Is_Zero()
    {
        var parameters = GameParameters.Initial with
        {
            MaxNumberOfGeneration = 0,
            MinNumberOfGeneration = 6
        };
        
        _userInputOutput.ReadLine().Returns("8", Command.Exit.Value);
        
        var result = _action.Execute(parameters);
        
        Assert.Equal(8, result.NumberOfGeneration);
        _userInputOutput.DidNotReceive().WriteLine(CommonMessages.InvalidInputMessage);
    }
}