using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Conway.Main;
using Conway.Main.Game;
using Conway.Tests.Tools;
using Xunit;
using Xunit.Abstractions;

namespace Conway.Tests;

public class GameScenario : IDisposable
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestUserInputOutput _userInputOutputMock;
    private readonly Task _gameTask;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly StringBuilder _allLines = new();
    public static GameScenario WhenGameStartsWith(ITestOutputHelper testOutputHelper, GameParameters? initialParameters=null)
    {
        return new GameScenario(testOutputHelper, initialParameters);
    }

    private GameScenario(ITestOutputHelper testOutputHelper, GameParameters? initialParameters)
    {
        _testOutputHelper = testOutputHelper;
        _cancellationTokenSource = new CancellationTokenSource();
        _userInputOutputMock = new TestUserInputOutput(_cancellationTokenSource.Token);
        _gameTask = Task.Run(() => GameStarter.Run(_userInputOutputMock, initialParameters), _cancellationTokenSource.Token);
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
            // ignored
        }
    }
}