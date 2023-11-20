using System.Collections.Generic;

public class Rook : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        return StandardMoves.StraightMoves(currentIndex);
    }
}
