﻿using System.Collections.Generic;

public class GameMode
{
    public readonly int TimeSeconds;
    public readonly List<int> IncrementsSeconds;

    public GameMode(int timeMinutes, int timeSeconds, List<int> incrementsSeconds)
    {
        TimeSeconds = timeMinutes * 60 + timeSeconds;
        IncrementsSeconds = incrementsSeconds;
    }
    
    public GameMode(int timeMinutes, List<int> incrementsSeconds)
    {
        TimeSeconds = timeMinutes * 60;
        IncrementsSeconds = incrementsSeconds;
    }
}
