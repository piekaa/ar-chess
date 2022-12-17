using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PieceMovementController : EventListener
{
    [SerializeField] private PiecesController piecesController;
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private Board board;


    [Listen(EventName.SelectedPiece)]
    private void ShowAvailableMovements(EventData eventData)
    {
        var piece = eventData.GameObject.GetComponent<Piece>();
        var moves = new List<int>();
        var currentIndex = piecesController.CurrentPieceBoardIndex(piece);
        var moveFunctions = piece.AvailableMoves(currentIndex);

        var startXY = Board.IndexToXY(currentIndex);

        foreach (var moveFunction in moveFunctions)
        {
            var currentXY = startXY;

            for (int i = 0; i < moveFunction.Limit; i++)
            {
                currentXY = moveFunction.NextMove(currentXY);
                if (Overflow(currentXY) || !piecesController.IsFree(Board.XYToIndex(currentXY)))
                {
                    break;
                }

                moves.Add(Board.XYToIndex(currentXY));
            }
        }

        moves.AddRange(AvailableCaptures(piece, currentIndex, startXY));
        board.SelectSquares(moves);
    }
    
    private List<int> AvailableCaptures(Piece piece, int currentIndex, Tuple<int, int> startXY)
    {
        var moves = new List<int>();
        var moveFunctions = piece.CaptureMoves(currentIndex);

        foreach (var moveFunction in moveFunctions)
        {
            var currentXY = startXY;

            for (int i = 0; i < moveFunction.Limit; i++)
            {
                currentXY = moveFunction.NextMove(currentXY);
                if (Overflow(currentXY))
                {
                    break;
                }
                var otherPiece = piecesController.GetPiece(Board.XYToIndex(currentXY));
                
                if (otherPiece)
                {
                    if (piece.black != otherPiece.black)
                    {
                        moves.Add(Board.XYToIndex(currentXY));    
                    }
                    break;
                }

                
            }
        }
        return moves;
    }

    private bool Overflow(Tuple<int, int> xy)
    {
        int x = xy.Item1;
        int y = xy.Item2;
        return x < 0 || y < 0 || x >= 8 || y >= 8;
    }

    [Listen(EventName.ChosenSquare)]
    private void MovePiece(EventData eventData)
    {
        var square = eventData.GameObject.GetComponent<BoardSquare>();
        
        var otherPiece = piecesController.GetPiece(Board.PositionToIndex(square.name));

        if (otherPiece)
        {
            piecesController.CapturePiece(otherPiece);
        }

        var startPosition = Board.IndexToPosition(piecesController.CurrentPieceBoardIndex(gameInfo.SelectedPiece));
        var endPosition = square.name;
        
        EventSystem.Fire(EventName.PlayerMove, new EventData((startPosition+endPosition).ToLower()));

        piecesController.MovePiece(gameInfo.SelectedPiece, square.name);
        board.DeselectSquares();
    }

    [Listen(EventName.EnemyMove)]
    private void MoveEnemyPiece(EventData eventData)
    {
        var move = eventData.Text;
        
        var firstSquare = move[..2].ToUpper();
        var secondSquare = move.Substring(2, 2).ToUpper();

        var pieceToMove = piecesController.GetPiece(Board.PositionToIndex(firstSquare));
        var otherPiece = piecesController.GetPiece(Board.PositionToIndex(secondSquare));

        if (otherPiece)
        {
            piecesController.CapturePiece(otherPiece);
        }
        
        piecesController.MovePiece(pieceToMove, secondSquare);
    }
}