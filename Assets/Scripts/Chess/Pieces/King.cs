using System;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        var moves = StandardMoves.CrossMoves(currentIndex, 1);
        moves.AddRange(StandardMoves.StraightMoves(currentIndex, 1));
        return moves;
    }

    public override List<MoveData> CastleMoves(int currentIndex)
    {
        if (moved)
        {
            return new();
        }

        return new List<MoveData>
        {
            new(xy => new Tuple<int, int>(xy.Item1-2, xy.Item2), 1),
            new(xy => new Tuple<int, int>(xy.Item1+2, xy.Item2), 1),
        };
    }
}