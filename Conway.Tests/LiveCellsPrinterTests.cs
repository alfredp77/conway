using System.Collections.Generic;
using System.Drawing;
using Conway.Main;
using NSubstitute;
using Xunit;

namespace Conway.Tests;

public class LiveCellsPrinterTests
{
    [Fact]
    public void Should_Display_Live_And_Dead_Cells()
    {
        var userInputOutput = Substitute.For<IUserInputOutput>();
        var printer = new LiveCellsPrinter(userInputOutput);

        var gameState = new GameState
        {
            Parameters = new GameParameters {Width = 5, Height = 6},
            LiveCells = new List<Point>
            {
                new (2, 5),
                new (5, 5),
                new (3,4),
                new (4,4),
                new (5, 4),
                new (3,3),
                new (4,3),
                new (5, 3),
            }
        };

        printer.Print("Initial position", gameState);

        var expected = 
@"Initial position
. . . . .
. . . . .
. . o o o
. . o o o
. o . . o
. . . . .";
        userInputOutput.Received(1).WriteLine(expected);
    }
}