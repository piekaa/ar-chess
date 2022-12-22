using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PieceMovementController : EventListener
{
    [SerializeField] private PiecesController piecesController;
    [SerializeField] private Board board;

    private BoardAnalyzer boardAnalyzer;
    private King kingInCheck;
    private void Awake()
    {
        boardAnalyzer = new BoardAnalyzer(piecesController);
    }

    [Listen(EventName.SelectedPiece)]
    private void ShowAvailableMovements(EventData eventData)
    {
        var piece = eventData.GameObject.GetComponent<Piece>();
        board.SelectSquares(boardAnalyzer.GetAvailableMovements(piece));
    }
    
    
    [Listen(EventName.Move)]
    private void MovePiece(EventData eventData)
    {
        var move = eventData.Text.ToUpper();

        var firstSquare = move[..2];
        var secondSquare = move.Substring(2, 2);

        var pieceToMove = piecesController.GetPiece(Board.PositionToIndex(firstSquare));

        var king = pieceToMove as King;

        if (move is "E1G1" or "E1C1" && king != null)
        {
            WhiteCastleMove(king, secondSquare);
            return;
        }
        
        if (move is "E8G8" or "E8C8" && king != null)
        {
            BlackCastleMove(king, secondSquare);
            return;
        }
        
        RegularMove(firstSquare, secondSquare);
        
        CheckChecks();
    }

    private void CheckChecks()
    {
        kingInCheck?.GetComponent<VisualChanger>().Uncheck();
        var king = boardAnalyzer.GetKingInCheckOrNull();
        king?.GetComponent<VisualChanger>()?.Check();
        kingInCheck = king;
    }
    
    private void RegularMove(string firstSquare, string secondSquare)
    {
        var pieceToMove = piecesController.GetPiece(Board.PositionToIndex(firstSquare));
        var otherPiece = piecesController.GetPiece(Board.PositionToIndex(secondSquare));

        if (otherPiece)
        {
            piecesController.CapturePiece(otherPiece);
        }

        piecesController.MovePiece(pieceToMove, secondSquare);
    }

    private void WhiteCastleMove(King king, string secondSquare)
    {
        Piece rook = piecesController.GetPiece(Board.PositionToIndex("A1"));
        var rookNewPosition = "D1";
        if (secondSquare == "G1")
        {
            rook = piecesController.GetPiece(Board.PositionToIndex("H1"));
            rookNewPosition = "F1";
        }

        piecesController.MovePiece(king, secondSquare);
        piecesController.MovePiece(rook, rookNewPosition);
    }
    
    private void BlackCastleMove(King king, string secondSquare)
    {
        Piece rook = piecesController.GetPiece(Board.PositionToIndex("A8"));
        var rookNewPosition = "D8";
        if (secondSquare == "G8")
        {
            rook = piecesController.GetPiece(Board.PositionToIndex("H8"));
            rookNewPosition = "F8";
        }

        piecesController.MovePiece(king, secondSquare);
        piecesController.MovePiece(rook, rookNewPosition);
    }
}