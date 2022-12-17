using System.Collections.Generic;

public class Queen : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        var moves = StandardMoves.CrossMoves(currentIndex);
        moves.AddRange(StandardMoves.StraightMoves(currentIndex));
        return moves;
    }
}