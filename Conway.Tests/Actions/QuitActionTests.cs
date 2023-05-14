using Conway.Main;
using Conway.Main.Actions;
using Conway.Main.Game;
using NSubstitute;
using Xunit;

namespace Conway.Tests.Actions;

public class QuitActionTests
{
    [Fact]
    public void Should_Return_End_GameState()
    {
        var action = new QuitAction();

        var state = action.Execute(GameParameters.Initial);
        
        Assert.True(state.IsEnd);
    }
}