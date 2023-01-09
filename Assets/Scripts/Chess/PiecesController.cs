using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//todo rename
public class PiecesController : MonoBehaviour
{
    [SerializeField] private Pieces whitePieces;

    [SerializeField] private Pieces blackPieces;

    [SerializeField] private Board board;

    private PiecesSpawner spawner;
    
    private PiecesPositions piecesPositions = new();

    private int capturedBlack;
    private int capturedWhite;

    private int moveNumber;

    public int MoveNumber => moveNumber;

    private void Start()
    {
        spawner = new PiecesSpawner(whitePieces, blackPieces, board, piecesPositions);
        spawner.SpawnPiecesAtBeginningPositions();
    }

    public int GetPiecePositionIndex(Piece piece)
    {
        return piecesPositions.GetIndex(piece);
    }

    public void SavePiecesPositions()
    {
        piecesPositions.Save();
    }

    public void RollbackPiecesPositions()
    {
        piecesPositions.Rollback();
        foreach (var piece in piecesPositions.GetAllPieces())
        {
            MovePiece(piece, Board.IndexToPosition(piecesPositions.GetIndex(piece)), true);
        }
    }

    public void MovePiece(Piece piece, string position, bool forAnalyze = false)
    {
        var currentLine = Board.IndexToPosition(piecesPositions.GetIndex(piece))[1];
        var newLine = position[0];

        if (!forAnalyze)
        {
            moveNumber++;
        }

        piece.Move(board.GetPosition(position), moveNumber, Math.Abs(newLine - currentLine), forAnalyze);

        piecesPositions.SetIndex(piece, Board.PositionToIndex(position));

        if (!forAnalyze)
        {
            piece.Deselect();
        }
        
        piece.position = position;
    }

    public void CapturePiece(Piece piece, bool forAnalyze = false)
    {
        if (!forAnalyze)
        {
            //todo animation
            piece.transform.position = piece.black
                ? board.CaptureSpotWhite(capturedBlack++)
                : board.CaptureSpotBlack(capturedWhite++);    
        }
        
        piece.position = "xx";
        piecesPositions.SetIndex(piece, -1);
    }

    public bool IsFree(int index)
    {
        return GetPiece(index) == null;
    }

    public Piece GetPiece(int index)
    {
        return piecesPositions.GetPiece(index);
    }

    public List<Piece> GetAllPieces()
    {
        return piecesPositions.GetAllPieces();
    }

    public void PromoteToPiece(string position, char pieceType, bool black)
    {
        spawner.SpawnPiece(position, pieceType, black);
    }
}