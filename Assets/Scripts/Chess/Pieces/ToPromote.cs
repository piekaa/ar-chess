using System.Collections.Generic;
using UnityEngine;

public class ToPromote : Piece
{
    public char pieceFirstLetter = 'X';
    
    public override List<MoveData> AvailableMoves(int currentIndex)
    {
        return new();
    }

    public override void Select()
    {
        // EventSystem.Fire(EventName.PromotionSelect, new EventData(""));
    }

    public override void Deselect()
    {
    }
}