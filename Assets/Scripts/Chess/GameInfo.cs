using UnityEngine;

public delegate void OnChosenSquare(BoardSquare square);

[CreateAssetMenu(fileName = "GameInfo", menuName = "Piekoszek/GameInfo")]
public class GameInfo : ScriptableObject
{
    private OnChosenSquare onChosenSquare;

    private Piece selectedPiece;
    
    public void PieceSelected(Piece piece)
    {
        EventSystem.Fire(EventName.SelectedPiece, new EventData(piece.gameObject));
        selectedPiece = piece;
    }
    
    public void SquareChosen(BoardSquare square)
    {
        EventSystem.Fire(EventName.ChosenSquare, new EventData(square.gameObject));
        selectedPiece = null;
    }

    public Piece SelectedPiece => selectedPiece;
}
