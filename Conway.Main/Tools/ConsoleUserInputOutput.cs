namespace Conway.Main.Tools;

public class ConsoleUserInputOutput : IUserInputOutput
{
    public void WriteLine(string textToDisplay="")
    {
        Console.WriteLine(textToDisplay);
    }

    public string ReadLine()
    {
        return Console.ReadLine() ?? "";
    }
}