using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        return new List<MoveData>
        {
            new(xy => new Tuple<int, int>(xy.Item1 - 2, xy.Item2 + 1), 1),
            new(xy => new Tuple<int, int>(xy.Item1 - 1, xy.Item2 + 2), 1),
            new(xy => new Tuple<int, int>(xy.Item1 + 1, xy.Item2 + 2), 1),
            new(xy => new Tuple<int, int>(xy.Item1 + 2, xy.Item2 + 1), 1),

            new(xy => new Tuple<int, int>(xy.Item1 + 2, xy.Item2 - 1), 1),
            new(xy => new Tuple<int, int>(xy.Item1 + 1, xy.Item2 - 2), 1),
            new(xy => new Tuple<int, int>(xy.Item1 - 1, xy.Item2 - 2), 1),
            new(xy => new Tuple<int, int>(xy.Item1 - 2, xy.Item2 - 1), 1),
        };
    }

    protected override void StartMoveAnimation(Vector3 targetPosition)
    {
        StartCoroutine(MoveAnimation(targetPosition));
        StartCoroutine(JumpAnimation(0.008f, targetPosition));
    }
}