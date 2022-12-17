using System.Collections.Generic;

public class King : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        var moves = StandardMoves.CrossMoves(currentIndex, 1);
        moves.AddRange(StandardMoves.StraightMoves(currentIndex, 1));
        return moves;
    }
}