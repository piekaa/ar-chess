using System;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{

    [SerializeField]
    private float CollapseForce;
    
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
            new(xy => new Tuple<int, int>(xy.Item1 - 2, xy.Item2), 1),
            new(xy => new Tuple<int, int>(xy.Item1 + 2, xy.Item2), 1),
        };
    }
    
    [Listen(EventName.GameEnd)]
    private void OnGameEnd(EventData eventData)
    {
        if (!black && eventData.Text == "black" || black && eventData.Text == "white")
        {
            var rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.AddTorque(Vector3.left * CollapseForce, ForceMode.Impulse);
        }
    }
}