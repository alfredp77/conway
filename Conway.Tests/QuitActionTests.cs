using Conway.Main;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class QuitActionTests
{
    [Fact]
    public void Should_Return_End_GameState()
    {
        var userInputOutput = Substitute.For<IUserInputOutput>();
        var action = new QuitAction(userInputOutput);

        var state = action.Execute(GameState.NoLiveCells);
        
        Assert.True(state.IsEnd);
        userInputOutput.Received(1).WriteLine("Thank you for playing Conway's Game of Life!");
    }
}