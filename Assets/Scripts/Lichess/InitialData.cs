using System.Collections.Generic;

public class InitialData
{
    public InitialDataData data;
}

public class InitialDataData
{
    public InitialDataPlayer player;
    public InitialDataPlayer opponent;
    public InitialDataUrl url;
    public ClockData clock;
    public List<InitialDataStep> steps;
}

public class InitialDataPlayer
{
    public InitialDataColor color;
    public InitialDataUser user;
}

public enum InitialDataColor
{
    black,
    white
}

public class InitialDataUser
{
    public string id;
    public string username;
}


public class InitialDataUrl
{
    public string socket;
}

public class InitialDataStep
{
    public int ply;
    public string fen;
}