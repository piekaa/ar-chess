public class InitialData
{
    public InitialDataData data;
}

public class InitialDataData
{
    public InitialDataPlayer player;
    public InitialDataUrl url;
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