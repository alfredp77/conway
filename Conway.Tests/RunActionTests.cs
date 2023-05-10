using Conway.Main;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class RunActionTests
{
    [Fact]
    public void Should_Generate_Initial_Cell_State_And_Prompt()
    {
        var userInputOutput = Substitute.For<IUserInputOutput>();
        var gameRunner = Substitute.For<IGameRunner>();
        userInputOutput.ReadLine().Returns("#");
        
        var action = new RunAction(userInputOutput, gameRunner);
        
        action.Execute(GameState.Initial);
        
        gameRunner.Received(1).Run(GameState.Initial);
        userInputOutput.Received(1).WriteLine("Enter > to go to next generation or # to go back to main menu");
    }
}