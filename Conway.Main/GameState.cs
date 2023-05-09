namespace Conway.Main;

public record GameState
{
    public static GameState Initial => new();
    public bool IsEnd { get; set; }
}