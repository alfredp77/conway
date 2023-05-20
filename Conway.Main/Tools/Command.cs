namespace Conway.Main.Tools;

public record Command
{
    private Command(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static readonly Command Exit = "#";
    public static readonly Command Next = ">";
    public static readonly Command ClearCells = "*";
    
    public static implicit operator Command(string value) => new(value);

    public override string ToString()
    {
        return Value;
    }
}