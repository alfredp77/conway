namespace Conway.Main;

public record GameState
{
    public static GameState Initial => new();
    public bool IsEnd { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int NumberOfGeneration { get; set; }
}