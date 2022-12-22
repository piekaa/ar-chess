using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

public class BoardAnalyzer
{
    private PiecesController piecesController;

    public BoardAnalyzer(PiecesController piecesController)
    {
        this.piecesController = piecesController;
    }

    public King GetKingInCheckOrNull()
    {
        foreach (var piece in piecesController.GetAllPieces())
        {
            foreach (var captureMove in AvailableCaptures(piece))
            {
                var king = piecesController.GetPiece(captureMove) as King;
                if (king != null)
                {
                    return king;
                }
            }
        }
        return null;
    }
    
    public List<int> GetAvailableMovements(Piece piece)
    {
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
        moves.AddRange(AvailableCastleMoves(piece, currentIndex, startXY));

        return moves;
    }

    private List<int> AvailableCaptures(Piece piece)
    {
        var currentIndex = piecesController.CurrentPieceBoardIndex(piece);
        var startXY = Board.IndexToXY(currentIndex);
        return AvailableCaptures(piece, currentIndex, startXY);
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

    private List<int> AvailableCastleMoves(Piece piece, int currentIndex, Tuple<int, int> startXY)
    {
        var moves = new List<int>();
        var castleMoves = piece.CastleMoves(currentIndex);

        foreach (var castleMove in castleMoves)
        {
            var move = castleMove.NextMove(startXY);
            var direction = (move.Item1 - startXY.Item1) / 2;

            for (var currentXY = NextCastleCheck(startXY, direction);
                 !Overflow(currentXY);
                 currentXY = NextCastleCheck(currentXY, direction))
            {
                var pieceAtCurrentPosition = piecesController.GetPiece(Board.XYToIndex(currentXY));
                var rook = pieceAtCurrentPosition as Rook;

                if (pieceAtCurrentPosition != null)
                {
                    if (rook == null)
                    {
                        break;
                    }

                    if (!rook.Moved)
                    {
                        moves.Add(Board.XYToIndex(move));
                        break;
                    }
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
    
    private Tuple<int, int> NextCastleCheck(Tuple<int, int> xy, int direction)
    {
        return new Tuple<int, int>(xy.Item1 + direction, xy.Item2);
    }
}
