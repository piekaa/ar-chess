using System.Collections.Generic;
using UnityEngine;

public class PiecesSpawner
{
    private Pieces whitePieces;
    private Pieces blackPieces;
    private Board board;
    private PiecesPositions piecesPositions;

    public PiecesSpawner(Pieces whitePieces, Pieces blackPieces, Board board, PiecesPositions piecesPositions)
    {
        this.whitePieces = whitePieces;
        this.blackPieces = blackPieces;
        this.board = board;
        this.piecesPositions = piecesPositions;
    }

    public void SpawnPieces(Fen fen)
    {
        
        foreach (var piece in piecesPositions.GetAllPieces())
        {
            Object.Destroy(piece.gameObject);
        }
        
        piecesPositions.Clear();
        
        var piecesByLetters = new Dictionary<char, Piece>()
        {
            { 'r', blackPieces.Rook },
            { 'n', blackPieces.Knight },
            { 'b', blackPieces.Bishop },
            { 'q', blackPieces.Queen },
            { 'k', blackPieces.King },
            { 'p', blackPieces.Pawn },

            { 'R', whitePieces.Rook },
            { 'N', whitePieces.Knight },
            { 'B', whitePieces.Bishop },
            { 'Q', whitePieces.Queen },
            { 'K', whitePieces.King },
            { 'P', whitePieces.Pawn },
        };
        

        var fenIndexer = new FenIndexer();
        
        foreach (var placementInfo in fen.placements)
        {
            if (char.IsDigit(placementInfo))
            {
                for (int i = 0; i < placementInfo - '0'; i++)
                {
                    fenIndexer.Next();
                }
                continue;
            }
            Instantiate(piecesByLetters[placementInfo], fenIndexer.Next(), char.IsLower(placementInfo));
        }
    }

    public void SpawnPiece(string position, char pieceType, bool black)
    {
        var piecesDeck = black ? blackPieces : whitePieces;

        switch (pieceType)
        {
            case 'Q':
                Instantiate(piecesDeck.Queen, position, black);
                break;
            case 'R':
                Instantiate(piecesDeck.Rook, position, black);
                break;
            case 'N':
                Instantiate(piecesDeck.Knight, position, black);
                break;
            case 'B':
                Instantiate(piecesDeck.Bishop, position, black);
                break;
        }
    }

    private void Instantiate(Piece piece, int index, bool black = false)
    {
        Instantiate(piece, Board.IndexToPosition(index), black);
    }


    private void Instantiate(Piece piece, string position, bool black = false)
    {
        var p = Object.Instantiate(piece, board.GetPosition(position),
            Quaternion.Euler(0, black ? 0 : 180, 0));
        p.black = black;
        p.position = position;
        piecesPositions.AddNewPiece(p, Board.PositionToIndex(position));
    }
}