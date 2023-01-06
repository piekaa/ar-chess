﻿public class LichessMessage
{
    public string t;
    public int v;
    public MessagePayload d;
}

public class MessagePayload
{
    public string uci;
    public string san;
    public string winner;
    public bool black;
    public bool white;
    public Promotion promotion;
}

public class Promotion
{
    public string pieceClass;
}