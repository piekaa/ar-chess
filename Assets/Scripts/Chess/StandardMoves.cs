using System;
using System.Collections.Generic;

public class StandardMoves
{
    public static List<MoveData> CrossMoves(int currentIndex, int limit=8)
    {
        return new List<MoveData>
        {
            new(xy => new Tuple<int, int>(xy.Item1+1, xy.Item2+1), limit),
            new(xy => new Tuple<int, int>(xy.Item1-1, xy.Item2+1), limit),
            new(xy => new Tuple<int, int>(xy.Item1+1, xy.Item2-1), limit),
            new(xy => new Tuple<int, int>(xy.Item1-1, xy.Item2-1), limit),
        };
    }
    
    public static List<MoveData> StraightMoves(int currentIndex, int limit=8)
    {
        return new List<MoveData>
        {
            new(xy => new Tuple<int, int>(xy.Item1+1, xy.Item2), limit),
            new(xy => new Tuple<int, int>(xy.Item1-1, xy.Item2), limit),
            new(xy => new Tuple<int, int>(xy.Item1, xy.Item2+1), limit),
            new(xy => new Tuple<int, int>(xy.Item1, xy.Item2-1), limit),
        };
    }
}