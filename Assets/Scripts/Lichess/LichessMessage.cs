public class LichessMessage
{
    public string t;
    public int v;
    public MessagePayload d;

    public LichessMessage()
    {
    }

    public LichessMessage(string t)
    {
        this.t = t;
    }
}

public class MessagePayload
{
    public string uci;
    public string san;
    public string url;
    public string winner;
    public bool black;
    public bool white;
    public Promotion promotion;
    public ClockData clock;
}

public class Promotion
{
    public string pieceClass;
}

public class ClockData
{
    public float white;
    public float black;
}