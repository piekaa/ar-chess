using System.Linq;

public class Fen
{
    public readonly string placements;
    public readonly Color whoseMove;

    public Fen(string fen)
    {
        var parts = fen.Split(" ");
        placements = string.Join("", parts[0].ToCharArray().Where(p => p != '/'));
        whoseMove = parts[1] == "w" ? Color.White : Color.Black;
    }
}

public enum Color
{
    White,
    Black
}