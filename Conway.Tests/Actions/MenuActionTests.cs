using Conway.Main.Actions;
using Conway.Main.Game;
using Conway.Main.InputProcessors;
using Conway.Main.Tools;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class MenuActionTests
{
    private const string ExpectedPrompt = "Test Prompt or # to go back to main menu:";
    private readonly IUserInputOutput _userInputOutput;
    private readonly MenuAction _action;
    private readonly IInputProcessor _inputProcessor;

    public MenuActionTests()
    {
        _userInputOutput = Substitute.For<IUserInputOutput>();
        _inputProcessor = Substitute.For<IInputProcessor>();
        _inputProcessor.Prompt.Returns("Test Prompt");
        _inputProcessor.Initialize(Arg.Any<GameParameters>())
            .Returns(c => ProcessedInput.ValidAndContinue(c.Arg<GameParameters>()));
        _inputProcessor.ProcessInput(Arg.Any<string>(), Arg.Any<GameParameters>())
            .Returns(new ProcessedInput
                {IsValid = false, Continue = false, GameParameters = GameParameters.Initial});
        _action = new MenuAction(_userInputOutput, _inputProcessor);
    }

    [Fact]
    public void Should_Use_Id_And_Description_From_Processor()
    {
        _inputProcessor.Id.Returns("my id");
        _inputProcessor.Description.Returns("blah blah");
        
        Assert.Equal(_inputProcessor.Id, _action.Id);
        Assert.Equal(_inputProcessor.Description, _action.Description);
    }

    [Fact]
    public void Should_Build_Correct_Prompt()
    {
        _action.Execute(GameParameters.Initial);
        
        _userInputOutput.Received(1).WriteLine(ExpectedPrompt);
    }

    [Fact]
    public void Should_Initialize_Processor()
    {
        _action.Execute(GameParameters.Initial);

        _inputProcessor.Received(1).Initialize(GameParameters.Initial);
    }
    
    [Fact]
    public void Should_Go_Back_To_Main_Menu_On_Exit()
    {
        _userInputOutput.ReadLine().Returns(Command.Exit.Value);

        var gameParameters = GameParameters.Initial with { Width = 1, Height = 1};
        var result = _action.Execute(gameParameters);
        
        Assert.Equal(gameParameters, result);
        _inputProcessor.DidNotReceiveWithAnyArgs().ProcessInput(Arg.Any<string>(), Arg.Any<GameParameters>());
    }
    
    [Fact]
    public void Should_Prompt_Again_When_Input_Is_Invalid()
    {
        _userInputOutput.ReadLine().Returns("invalid", Command.Exit.Value);
        _inputProcessor.ProcessInput(Arg.Any<string>(), Arg.Any<GameParameters>())
            .Returns(c => ProcessedInput.Invalid(c.Arg<GameParameters>()));
        
        var gameParameters = GameParameters.Initial with { Width = 1, Height = 1};
        var result = _action.Execute(gameParameters);
        
        Assert.Equal(gameParameters, result);
        _inputProcessor.Received(1).ProcessInput("invalid", gameParameters);
        _userInputOutput.Received(1).WriteLine(CommonMessages.InvalidInputMessage);
        _userInputOutput.Received(2).WriteLine(ExpectedPrompt);
    }

    [Fact]
    public void Should_Exit_When_Next_Input_Is_No_Longer_Required()
    {
        _userInputOutput.ReadLine().Returns("input1", "input2", "input3");
        var gameParameters = GameParameters.Initial with { Width = 1, Height = 1};
        var nextParameters = gameParameters with { Width = 2, Height = 2};
        var lastParameters = nextParameters with{ Width = 3, Height = 3};
        _inputProcessor.ProcessInput("input1", gameParameters)
            .Returns(ProcessedInput.ValidAndContinue(nextParameters));
        _inputProcessor.ProcessInput("input2", nextParameters)
            .Returns(ProcessedInput.ValidAndExit(lastParameters));
        
        var result = _action.Execute(gameParameters);
        
        Assert.Equal(lastParameters, result);
        _inputProcessor.DidNotReceive().ProcessInput("input3", Arg.Any<GameParameters>());
        _userInputOutput.Received(2).WriteLine(ExpectedPrompt);
    }

    [Fact]
    public void Should_Use_Prompt_From_ProcessedInput_If_Not_Empty()
    {
        _userInputOutput.ReadLine().Returns("input1", "input2", "input3");
        var gameParameters = GameParameters.Initial with { Width = 1, Height = 1};
        var nextParameters = gameParameters with { Width = 2, Height = 2};
        var lastParameters = nextParameters with{ Width = 3, Height = 3};
        _inputProcessor.ProcessInput("input1", gameParameters)
            .Returns(ProcessedInput.ValidAndContinue(nextParameters, "test 123"));
        _inputProcessor.ProcessInput("input2", nextParameters)
            .Returns(ProcessedInput.ValidAndExit(lastParameters));
        
        _action.Execute(gameParameters);
        
        _userInputOutput.Received(1).WriteLine(MenuAction.GetPrompt("test 123"));
    }

    [Fact]
    public void Should_Use_Prompt_From_Processed_Input_And_Wait_For_Any_Key_When_Exit_With_Prompt()
    {
        _userInputOutput.ReadLine().Returns("input1", "input2");
        var gameParameters = GameParameters.Initial with { Width = 1, Height = 1};
        var nextParameters = gameParameters with { Width = 2, Height = 2};
        var lastParameters = nextParameters with{ Width = 3, Height = 3};
        _inputProcessor.ProcessInput("input1", gameParameters)
            .Returns(ProcessedInput.ValidAndContinue(nextParameters));
        _inputProcessor.ProcessInput("input2", nextParameters)
            .Returns(ProcessedInput.ValidAndExit(lastParameters, "bye bye"));
        
        _action.Execute(gameParameters);
        
        _userInputOutput.Received(1).ReadKey(MenuAction.GetExitPrompt("bye bye"));
    }
}