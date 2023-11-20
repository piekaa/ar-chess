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
    public Game game;
}

public class Game
{
    public string id;
}

public class InitialDataPlayer
{
    public InitialDataColor color;
    public int rating;
    public InitialDataUser user;
    public string id;
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