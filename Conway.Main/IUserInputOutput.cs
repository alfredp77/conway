namespace Conway.Main;

public interface IUserInputOutput
{
    void WriteLine(string textToDisplay="");
    string ReadLine();
}

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