using Unity.VisualScripting;
using UnityEngine;

//todo rename
public class PiecesController : MonoBehaviour
{
    [SerializeField] private Pieces whitePieces;

    [SerializeField] private Pieces blackPieces;

    [SerializeField] private Board board;

    private PiecesPositions piecesPositions = new();

    private int capturedBlack;
    private int capturedWhite;

    private void Start()
    {
        new PiecesSpawner(whitePieces, blackPieces, board, piecesPositions).SpawnPiecesAtBeginningPositions();
    }

    public int CurrentPieceBoardIndex(Piece piece)
    {
        return piecesPositions.GetIndex(piece);
    }

    public void MovePiece(Piece piece, string position)
    {
        //todo animation
        piece.transform.position = board.GetPosition(position);
        piecesPositions.SetIndex(piece, Board.PositionToIndex(position));
        piece.Deselect();
        piece.position = position;
    }

    public void CapturePiece(Piece piece)
    {
        //todo animation
        piece.transform.position = piece.black ? board.CaptureSpotWhite(capturedBlack++) 
            : board.CaptureSpotBlack(capturedWhite++);
        piece.position = "xx";
    }

    public bool IsFree(int index)
    {
        return GetPiece(index) == null;
    }
    
    public Piece GetPiece(int index)
    {
        return piecesPositions.GetPiece(index);
    }
}