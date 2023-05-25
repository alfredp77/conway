﻿namespace Conway.Main.Tools;

public interface IUserInputOutput
{
    void WriteLine(string textToDisplay="");
    string ReadLine();
    void ReadKey(string textToDisplay="");
}