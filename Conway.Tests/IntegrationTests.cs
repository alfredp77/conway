using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Conway.Main;
using Conway.Main.Actions;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace Conway.Tests;

public class IntegrationTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public IntegrationTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    [Fact]
    public void Should_Run_Sample_Scenario_Successfully()
    {
        const string mainMenu =
            @"Welcome to Conway's Game of Life!
[1] Specify grid size
[2] Specify number of generation
[3] Specify initial live cells
[4] Run
[Q] Quit
Please enter your selection";

        GameScenario.WhenGameStartsWith(_testOutputHelper)
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("1")
            .ThenScreenDisplays(
@"Please enter grid size in w h format (example: 10 15):"
            )

            .WhenUserEnters("5 5")
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("2")
            .ThenScreenDisplays(
@"Please enter number of generation (10-20):")
            .WhenUserEnters("3")
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("2 4")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 4")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("3 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("4 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 3")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("3 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("4 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters("5 2")
            .ThenScreenDisplays(InputLiveCellAction.InputLiveCellPrompt)
            .WhenUserEnters(Command.Exit.Value)
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("4")
            .ThenScreenDisplays(
                @"Initial position
. . . . .
. . o o o
. . o o o
. o . . o
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 1
. . . o .
. . o . o
. o . . .
. . o . o
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 2
. . . o .
. . o o .
. o o . .
. . . . .
. . . . .
Enter > to go to next generation or # to go back to main menu")
            .WhenUserEnters(">")
            .ThenScreenDisplays(
                @"Generation 3
. . o o .
. o . o .
. o o o .
. . . . .
. . . . .
End of generation. Press any key to return to main menu")
            .WhenUserEnters(Command.Exit.Value)
            .ThenScreenDisplays(mainMenu)
            .WhenUserEnters("Q")
            .ThenScreenDisplays("Thank you for playing Conway's Game of Life!")
            .Dispose();
    }
    
}


public class TestUserInputOutput : IUserInputOutput, IDisposable
{
    private readonly CancellationToken _token;
    private readonly List<string> _lines = new();
    public IReadOnlyList<string> Lines => _lines;

    public TestUserInputOutput(CancellationToken token)
    {
        _token = token;
    }
    public void ClearLines()
    {
        lock (_lines)
        {
            _lines.Clear();   
        }
    }
    
    public void WriteLine(string textToDisplay = "")
    {
        lock (_lines)
        {
            _lines.Add(textToDisplay);   
        }
    }

    private string _lineToReturn = string.Empty;
    public string ReadLine()
    {
        _waitForInput.Set();
        while (_waitForInput.IsSet && !_token.IsCancellationRequested)
        {
            Thread.Sleep(100);
        }

        if (_token.IsCancellationRequested)
        {
            throw new ApplicationException("Test cancelled");
        }
        return _lineToReturn;
    }

    private readonly ManualResetEventSlim _waitForInput = new();

    public bool IsWaitingForInput => _waitForInput.IsSet;
    public void SetLineToReturn(string lineToReturn)
    {
        _lineToReturn = lineToReturn;
        _waitForInput.Reset();
    }

    public void Dispose()
    {
        _waitForInput.Dispose();
    }
}

public class GameScenario : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestUserInputOutput _userInputOutputMock;
    private readonly Task _gameTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly StringBuilder _allLines = new();
    public static GameScenario WhenGameStartsWith(ITestOutputHelper testOutputHelper)
    {
        return new GameScenario(testOutputHelper);
    }

    private GameScenario(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _cancellationTokenSource = new CancellationTokenSource();
        _userInputOutputMock = new TestUserInputOutput(_cancellationTokenSource.Token);
        _gameTask = Task.Run(() => GameStarter.Run(_userInputOutputMock), _cancellationTokenSource.Token);
    }
    
    public GameScenario ThenScreenDisplays(string expected)
    {
        while (!_userInputOutputMock.IsWaitingForInput && !_gameTask.IsCompleted)
        {
            Thread.Sleep(100);
        }
        var sb = new StringBuilder();
        foreach (var line in _userInputOutputMock.Lines)
        {
            _allLines.AppendLine(line);
            sb.AppendLine(line);
        }
        var actual = sb.ToString().Trim();
        _userInputOutputMock.ClearLines();
        
        try
        {
            Assert.Equal(expected, actual);
            return this;
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    public GameScenario WhenUserEnters(string input)
    {
        _allLines.Append("> ");
        _allLines.Append(input);
        _allLines.AppendLine();
        _userInputOutputMock.SetLineToReturn(input);
        return this;
    }

    public void Dispose()
    {
        try
        {
            _testOutputHelper.WriteLine("All lines:");
            _testOutputHelper.WriteLine(_allLines.ToString());
            
            _cancellationTokenSource.Cancel();
            _userInputOutputMock.Dispose();
            _gameTask.Wait();
        }
        catch
        {
            
        }
    }
}