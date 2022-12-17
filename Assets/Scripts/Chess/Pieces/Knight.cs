using System;
using System.Collections.Generic;

public class Knight : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        return new List<MoveData>
        {
            new(xy => new Tuple<int, int>(xy.Item1-2, xy.Item2+1), 1),
            new(xy => new Tuple<int, int>(xy.Item1-1, xy.Item2+2), 1),
            new(xy => new Tuple<int, int>(xy.Item1+1, xy.Item2+2), 1),
            new(xy => new Tuple<int, int>(xy.Item1+2, xy.Item2+1), 1),
            
            new(xy => new Tuple<int, int>(xy.Item1+2, xy.Item2-1), 1),
            new(xy => new Tuple<int, int>(xy.Item1+1, xy.Item2-2), 1),
            new(xy => new Tuple<int, int>(xy.Item1-1, xy.Item2-2), 1),
            new(xy => new Tuple<int, int>(xy.Item1-2, xy.Item2-1), 1),
        };
    }
}