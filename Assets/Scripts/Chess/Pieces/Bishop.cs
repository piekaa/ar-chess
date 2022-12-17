using System.Collections.Generic;

public class Bishop : Piece
{
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        return StandardMoves.CrossMoves(currentIndex);
    }
}
