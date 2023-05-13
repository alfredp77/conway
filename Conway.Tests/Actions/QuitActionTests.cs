using Conway.Main;
using Conway.Main.Actions;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class QuitActionTests
{
    [Fact]
    public void Should_Return_End_GameState()
    {
        var userInputOutput = Substitute.For<IUserInputOutput>();
        var action = new QuitAction(userInputOutput);

        var state = action.Execute(GameParameters.Initial);
        
        Assert.True(state.IsEnd);
        userInputOutput.Received(1).WriteLine("Thank you for playing Conway's Game of Life!");
    }
}