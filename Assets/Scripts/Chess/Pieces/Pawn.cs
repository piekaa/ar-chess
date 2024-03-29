﻿using System;
using System.Collections.Generic;

public class Pawn : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        var row = Board.Row(currentIndex);
        var limit = row is 2 or 7 ? 2 : 1;
        return black
            ? new List<MoveData> { new(xy => new Tuple<int, int>(xy.Item1, xy.Item2 - 1), limit) }
            : new List<MoveData> { new(xy => new Tuple<int, int>(xy.Item1, xy.Item2 + 1), limit) };
    }

    public override List<MoveData> CaptureMoves(int currentIndex)
    {
        return black
            ? new List<MoveData>
            {
                new(xy => new Tuple<int, int>(xy.Item1 - 1, xy.Item2 - 1), 1),
                new(xy => new Tuple<int, int>(xy.Item1 + 1, xy.Item2 - 1), 1)
            }
            : new List<MoveData>
            {
                new(xy => new Tuple<int, int>(xy.Item1 - 1, xy.Item2 + 1), 1),
                new(xy => new Tuple<int, int>(xy.Item1 + 1, xy.Item2 + 1), 1)
            };
    }

    public override List<MoveData> EnPassantMoves(int currentIndex)
    {
        var line = Board.IndexToPosition(currentIndex)[1];

        if (black)
        {
            if (line == '4') return CaptureMoves(currentIndex);
        }
        else
        {
            if (line == '5') return CaptureMoves(currentIndex);
        }

        return new();
    }
}