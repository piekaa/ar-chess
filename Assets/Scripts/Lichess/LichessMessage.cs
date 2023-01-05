public class LichessMessage
{
    public string t;
    public int v;
    public MovePayload d;
}

public class MovePayload
{
    public string uci;
    public string san;
    public bool black;
    public bool white;
    public Promotion promotion;
}

public class Promotion
{
    public string pieceClass;
}