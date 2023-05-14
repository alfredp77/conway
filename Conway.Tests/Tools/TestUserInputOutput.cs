using System;
using System.Collections.Generic;
using System.Threading;
using Conway.Main.Tools;

namespace Conway.Tests.Tools;

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